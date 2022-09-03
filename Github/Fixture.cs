using System.IO.Compression;
using Octokit;
using Octokit.Internal;

namespace Github;

internal static class Fixture
{
    #region Constants
    /// <summary>
    /// Id of the repository on github <br/>
    /// <i>Search for: <b>octolytics-dimension-repository_id</b> <br/> in page source code</i>
    /// </summary>
    private const uint repositoryId = 531676282;
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
    /// Name of the Github.cs project
    /// </summary>
    private const string githubProject = "Github";
    /// <summary>
    /// Name of the YuGiOh.cs project
    /// </summary>
    private const string yuGiOhProject = "YuGiOh";
    /// <summary>
    /// Name of the Debug build folder
    /// </summary>
    private const string debugFolder = "Debug";
    /// <summary>
    /// Name of the Release build folder
    /// </summary>
    private const string releaseFolder = "Release";
    /// <summary>
    /// Name + file extension of the release build .rar file 
    /// </summary>
    private const string releaseBuildFile = "YuGiOh-DeckBuilder.rar";
    /// <summary>
    /// Name + file extension of the last update .txt file
    /// </summary>
    private const string lastUpdateFile = "Last-Update.txt";
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
    private static readonly string buildFolder = AppDomain.CurrentDomain.BaseDirectory;
    /// <summary>
    /// Absolute path to the root folder of this solution
    /// </summary>
    private static readonly string rootFolder = Directory.GetParent(buildFolder)!.Parent!.Parent!.Parent!.Parent!.FullName;
    /// <summary>
    /// Absolute path to the release build .rar file
    /// </summary>
    private static readonly string releaseBuildFilePath = Path.Combine(rootFolder, releaseBuildFile);
    #endregion

    #region Constructor
    static Fixture()
    {
        var gitHubToken = GetTokenAsync().Result;
        gitHubClient = new GitHubClient(new ProductHeaderValue(name), new InMemoryCredentialStore(new Credentials(gitHubToken, AuthenticationType.Bearer)));
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// Creates a .rar file of the last release build
    /// </summary>
    internal static void CreateRarFile()
    {
        var releaseBuildFolderPath = buildFolder.Replace(githubProject, yuGiOhProject).Replace(debugFolder, releaseFolder);

        if (File.Exists(releaseBuildFilePath))
        {
            File.Delete(releaseBuildFilePath);
        }
        
        ZipFile.CreateFromDirectory(releaseBuildFolderPath, releaseBuildFilePath);
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
        await using var releaseBuildFileStream = File.OpenRead(releaseBuildFilePath);
        var releaseAssetUpload = new ReleaseAssetUpload
        {
            FileName = releaseBuildFile,
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
        var timeStamp = DateTime.UtcNow.ToString("g");
        
        await using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream);
        await streamWriter.WriteAsync(timeStamp);
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