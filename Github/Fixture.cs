using System.IO.Compression;
using System.Text;
using Octokit;
using Octokit.Internal;
using YuGiOh_DeckBuilder;
using YuGiOh_DeckBuilder.Extensions;

namespace Github;

/// <summary>
/// <see cref="Fixture"/> for <see cref="Github"/>
/// </summary>
internal static class Fixture
{
    #region Constants
    /// <summary>
    /// Id of the repository on github <br/>
    /// <i>Search for: <b>octolytics-dimension-repository_id</b> <br/> in page source code</i>
    /// </summary>
    private const uint repositoryId = 532227886;
    /// <summary>
    /// Name + file extension of the .txt file that holds the github access token
    /// </summary>
    private const string tokenFile = "Token.txt";
    /// <summary>
    /// <see cref="ProductHeaderValue.Name"/>
    /// </summary>
    private const string name = "YuGiOh-DeckBuilder";
    /// <summary>
    /// Description text of the release
    /// </summary>
    private const string description = "Deck builder for YuGiOh";
    /// <summary>
    /// Tag og the release on github
    /// </summary>
    private const string releaseTag = "YGO-DB";
    /// <summary>
    /// Name of the Github.csproj project
    /// </summary>
    private const string github = "Github";
    /// <summary>
    /// Name of the YuGiOh.csproj project
    /// </summary>
    private const string yuGiOh = "YuGiOh";
    /// <summary>
    /// Name of the Installer.csproj project
    /// </summary>
    private const string installer = "Installer";
    /// <summary>
    /// Name of the Debug build folder
    /// </summary>
    internal const string Debug = "Debug";
    /// <summary>
    /// Name of the Release build folder
    /// </summary>
    internal const string Release = "Release";
    /// <summary>
    /// <see cref="App.RarFile"/>
    /// </summary>
    private const string rarFile = App.RarFile;
    /// <summary>
    /// <see cref="App.LastUpdateFile"/>
    /// </summary>
    private const string lastUpdateFile = App.LastUpdateFile;
    /// <summary>
    /// MIME type for .rar files
    /// </summary>
    private const string mimeTypeRar = "application/x-rar-compressed";
    /// <summary>
    /// MIME type for .txt files
    /// </summary>
    private const string mimeTypeText = "text/plain";
    #endregion
    
    #region Members
    /// <summary>
    /// <see cref="GitHubClient"/>
    /// </summary>
    private static readonly GitHubClient gitHubClient;
    /// <summary>
    /// Absolute path to the build folder of this project
    /// </summary>
    private static readonly string buildFolder = AppContext.BaseDirectory;
    /// <summary>
    /// Absolute path to the root folder of this solution
    /// </summary>
    private static readonly string rootFolder = Directory.GetParent(buildFolder)!.Parent!.Parent!.Parent!.Parent!.FullName;
    /// <summary>
    /// Absolute path to the build .rar file
    /// </summary>
    private static readonly string rarFilePath = Path.Combine(rootFolder, rarFile);
    
    /// <summary>
    /// <see cref="DateTime"/>.<see cref="DateTime.UtcNow"/>
    /// </summary>
    private static readonly string currentUtcTime;
    #endregion

    #region Constructor
    static Fixture()
    {
        var gitHubToken = GetTokenAsync().Result;
        gitHubClient = new GitHubClient(new ProductHeaderValue(name), new InMemoryCredentialStore(new Credentials(gitHubToken, AuthenticationType.Bearer)));
        
        currentUtcTime = DateTime.UtcNow.ToString("F");
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// Creates a .rar file of the last build
    /// </summary>
    /// <param name="commandLineArgument">Command-line argument for the current process</param>
    internal static async Task CreateRarFileAsync(string commandLineArgument)
    {
        var yuGiOhBuildFolderPath = buildFolder.Replace(github, yuGiOh).Replace(Release, commandLineArgument);
        var installerBuildFolderPath = buildFolder.Replace(github, installer).Replace(Release, commandLineArgument);
        
        await CreateLastUpdateFile(yuGiOhBuildFolderPath);
        CopyInstaller(installerBuildFolderPath, Path.Combine(yuGiOhBuildFolderPath, yuGiOh, installer));
        
        if (commandLineArgument == Release) // The debug build has all downloaded data (multiple GB)
        {
            if (File.Exists(rarFilePath))
            {
                File.Delete(rarFilePath);
            }
            
            ZipFile.CreateFromDirectory(yuGiOhBuildFolderPath, rarFilePath, CompressionLevel.SmallestSize, false, Encoding.UTF8);
        }
    }

    /// <summary>
    /// Deletes the latest release on github
    /// </summary>
    internal static async Task DeleteLatestReleaseAsync()
    {
        try
        {
            var latestRelease = await gitHubClient.Repository.Release.GetLatest(repositoryId);
            
            await gitHubClient.Repository.Release.Delete(repositoryId, latestRelease.Id);
        }
        catch { /* Ignored */ }
    }
    
    /// <summary>
    /// Creates a new release on github
    /// </summary>
    internal static async Task CreateNewReleaseAsync()
    {
        var newRelease = new NewRelease(releaseTag)
        {
            Name = name,
            Body = description
        };

        await gitHubClient.Repository.Release.Create(repositoryId, newRelease);
    }

    /// <summary>
    /// Uploads the last release build .rar to github
    /// </summary>
    internal static async Task UploadLastReleaseBuildAsync()
    {
        await using var releaseBuildFileStream = File.OpenRead(rarFilePath);
        var releaseAssetUpload = new ReleaseAssetUpload
        {
            FileName = rarFile,
            ContentType = mimeTypeRar,
            RawData = releaseBuildFileStream
        };
        
        await UploadReleaseAsync(releaseAssetUpload);
    }

    /// <summary>
    /// Uploads the last update info to github
    /// </summary>
    internal static async Task UploadLastUpdateInfoAsync()
    {
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream);
        await streamWriter.WriteAsync(currentUtcTime);
        await streamWriter.FlushAsync();
        
        memoryStream.Seek(0, SeekOrigin.Begin);

        var releaseAssetUpload = new ReleaseAssetUpload
        {
            FileName = lastUpdateFile,
            ContentType = mimeTypeText,
            RawData = memoryStream
        };
        
        await UploadReleaseAsync(releaseAssetUpload);
    }
    
    /// <summary>
    /// Uploads the given <see cref="ReleaseAssetUpload"/> to github
    /// </summary>
    /// <param name="releaseAssetUpload">The <see cref="ReleaseAssetUpload"/> to upload to github</param>
    private static async Task UploadReleaseAsync(ReleaseAssetUpload releaseAssetUpload)
    {
        var release = await gitHubClient.Repository.Release.Get(repositoryId, releaseTag);
        await gitHubClient.Repository.Release.UploadAsset(release, releaseAssetUpload);
    }

    /// <summary>
    /// Creates a .txt file with <see cref="currentUtcTime"/> at the given folder
    /// </summary>
    /// <param name="folderPath">The folder path to create the .txt file at</param>
    private static async Task CreateLastUpdateFile(string folderPath)
    {
        var filePath = Path.Combine(folderPath, lastUpdateFile);
        await using var fileStream = File.Create(filePath);
        await using var streamWriter = new StreamWriter(fileStream);
        await streamWriter.WriteAsync(currentUtcTime);
    }

    /// <summary>
    /// Copies the Installer build to the YuGiOh release build folder
    /// </summary>
    /// <param name="installerBuildFolderPath">Folder path to the installer build</param>
    /// <param name="yuGiOhInstallerFolder">Folder path to the installer folder, in the YuGiOh release build</param>
    private static void CopyInstaller(string installerBuildFolderPath, string yuGiOhInstallerFolder)
    {
        if (Directory.Exists(yuGiOhInstallerFolder))
        {
            Directory.Delete(yuGiOhInstallerFolder, true);
        }
        
        var sourceDirectory = new DirectoryInfo(installerBuildFolderPath);
        var targetDirectory = new DirectoryInfo(yuGiOhInstallerFolder);
        
        sourceDirectory.DeepCopy(targetDirectory);
    }
    
    /// <summary>
    /// Gets the github access token <i>(<see cref="tokenFile"/>)</i>
    /// </summary>
    /// <returns>The github access token</returns>
    private static async Task<string> GetTokenAsync()
    {
        var filePath = Path.Combine(rootFolder, tokenFile);

        await using var fileStream = File.OpenRead(filePath);
        using var streamReader = new StreamReader(fileStream);

        return (await streamReader.ReadLineAsync())!.Replace(" ", string.Empty).Replace("\n", string.Empty);
    }
    #endregion
}