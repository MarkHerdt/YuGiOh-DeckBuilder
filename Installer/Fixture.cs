using System.IO.Compression;
using System.Text;
using Installer.Extensions;

namespace Installer;

/// <summary>
/// <see cref="Fixture"/> for <see cref="Installer"/>
/// </summary>
internal static class Fixture
{
    #region Methods
    /// <summary>
    /// Updates the YuGiOh-DeckBuilder app
    /// </summary>
    /// <param name="folderPath">Path fo the folder to update</param>
    /// <param name="excludeFolder">Name of a folder to exclude</param>
    /// <param name="rarFile">Name of the .rar file to update the folder with</param>
    /// <param name="tempFolder">Name of the folder, the .rar contents are extracted to</param>
    internal static void Update(string folderPath, string excludeFolder, string rarFile, out string tempFolder)
    {
        tempFolder = "temp";
        
        var paths = Directory.GetFileSystemEntries(folderPath, "*.*", SearchOption.TopDirectoryOnly);
        var excludeFolderPath = Path.Combine(folderPath, excludeFolder);
        var rarFilePath = Path.Combine(folderPath, rarFile);
        var tempFolderPath = Path.Combine(folderPath, tempFolder);

        foreach (var path in paths)
        {
            if (path != excludeFolderPath && path != rarFilePath)
            {
                try
                {
                    if (Path.HasExtension(path))
                    {
                        File.Delete(path);

                    }
                    else
                    {
                        Directory.Delete(path, true);
                    }
                }
                catch { /* Ignored */ }
            }
        }
        
        try
        {
            ZipFile.ExtractToDirectory(rarFilePath, tempFolderPath, Encoding.UTF8, true);

            var yuGiOhDirectory = new DirectoryInfo(folderPath);
            var tempDirectory = new DirectoryInfo(tempFolderPath);

            var installerFolderPath = Path.Combine(folderPath, excludeFolder, nameof(Installer));
            
            tempDirectory.DeepCopy(yuGiOhDirectory, installerFolderPath);
        }
        catch { /* Ignored */ }
        
        File.Delete(Path.Combine(folderPath, rarFile));
    }
    #endregion
}