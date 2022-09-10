using System;
using System.IO;
using System.Text.RegularExpressions;

namespace YuGiOh_DeckBuilder.Utility.Project;

public static class Structure
{
    #region Members
    /// <summary>
    /// Finds all prohibited file name characters
    /// </summary>
    private static readonly Regex forbiddenFileNameCharacters = new("[\\/:*?\"<>|\\s]");
    #endregion
    
    #region Methods
    /// <summary>
    /// Replaces all characters in the given fileName, that match <see cref="forbiddenFileNameCharacters"/> with '_'
    /// </summary>
    /// <param name="fileName">File name to replace the forbidden characters in</param>
    /// <returns>The given fileName, where all forbidden characters are replaces with '_'</returns>
    internal static string ReplaceForbiddenFileNameCharacters(string fileName)
    {
        return forbiddenFileNameCharacters.Replace(fileName, "_");
    }
    
    /// <summary>
    /// Gets all files in the given <see cref="Folder"/> with the given file <see cref="Extension"/>
    /// </summary>
    /// <param name="folder">The <see cref="Folder"/> to get all files in</param>
    /// <param name="fileExtension">The <see cref="Extension"/>, the searched files must have</param>
    /// <returns>The paths to all files in the given <see cref="Folder"/> with the given file <see cref="Extension"/></returns>
    internal static string[] GetFilePaths(Folder folder, Extension fileExtension)
    {
        var folderPath = BuildPath(folder);
        var filePaths = Directory.GetFiles(folderPath, $"*.{fileExtension}");

        return filePaths;
    }
    
    /// <summary>
    /// Opens a <see cref="Stream"/> to the given file
    /// </summary>
    /// <param name="folder">Folder to get the path to</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="fileExtension">Extension of the file</param>
    /// <param name="fileMode"><see cref="FileMode"/></param>
    /// <returns>The file content in a <see cref="Stream"/></returns>
    internal static Stream OpenStream(Folder folder, string fileName, Extension fileExtension, FileMode fileMode)
    {
        var filePath = BuildPath(folder, fileName, fileExtension);
        var fileStream = File.Open(filePath, fileMode, FileAccess.ReadWrite, FileShare.ReadWrite);
        
        return fileStream;
    }

    /// <summary>
    /// Builds a path to the given folder
    /// </summary>
    /// <param name="folder">Folder to get the path to</param>
    /// <returns>An absolute path to a folder</returns>
    internal static string BuildPath(Folder folder)
    {
        // ReSharper disable once JoinDeclarationAndInitializer
        string directoryPath;
        
#if DEBUG
        if ((int)folder < 0)
        {
            directoryPath = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;
            directoryPath = Path.Combine(directoryPath, "YuGiOh.Tests");
        }
        else
        {
            directoryPath = AppContext.BaseDirectory;
        }
#else
        directoryPath = AppContext.BaseDirectory;
#endif
        
        var folderPath = Path.Combine(directoryPath, GetFolderPath(folder));

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        
        return folderPath;
    }
    
    /// <summary>
    /// Builds a path to the given file
    /// </summary>
    /// <param name="folder">Folder to get the path to</param>
    /// <param name="fileName">The name of the file</param>
    /// <param name="fileExtension">The extension of the file</param>
    /// <returns>An absolute path to the file</returns>
    internal static string BuildPath(Folder folder, string fileName, Extension fileExtension) // TODO: Replace all static/known file names with enum entries
    {
        var file = string.Concat(fileName, ".", fileExtension);
        var folderPath = BuildPath(folder);
        var filePath = Path.Combine(folderPath, file);

        return filePath;
    }

    /// <summary>
    /// Builds a path to the given file
    /// </summary>
    /// <param name="fileName"><see cref="FileName"/></param>
    /// <returns>An absolute path to the file</returns>
    internal static string BuildPath(FileName fileName)
    {
        var (folder, file) = GetFolderPath(fileName);
        var folderPath = BuildPath(folder);
        var filePath = Path.Combine(folderPath, file);
        
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file at the given path doesn't exist: {filePath}");
        }
        
        return filePath;
    }
    
    /// <summary>
    /// Returns the relative path to the given folder
    /// </summary>
    /// <param name="folder">Folder to get the path to</param>
    /// <returns>A relative path</returns>
    /// <exception cref="ArgumentOutOfRangeException">When <see cref="Folder"/> doesn't contain the given enum value</exception>
    private static string GetFolderPath(Folder folder)
    {
        const string yuGiOh = "YuGiOh";
        const string temp = "temp";
        
        return folder switch
        {
            Folder.Data => Path.Combine(yuGiOh, nameof(Folder.Data)),
            Folder.Packs => Path.Combine(yuGiOh, nameof(Folder.Data), nameof(Folder.Packs)),
            Folder.Cards => Path.Combine(yuGiOh, nameof(Folder.Data), nameof(Folder.Cards)),
            Folder.Images => Path.Combine(yuGiOh, nameof(Folder.Data), nameof(Folder.Images)),
            Folder.Localization => Path.Combine(yuGiOh, nameof(Folder.Data), nameof(Folder.Localization)),
            Folder.Export => Path.Combine(yuGiOh, nameof(Folder.Data), nameof(Folder.Export)),
            Folder.Logging => Path.Combine(yuGiOh, nameof(Folder.Data), nameof(Folder.Logging)),
            Folder.Installer => Path.Combine(yuGiOh, nameof(Folder.Installer)),
            Folder.TempInstaller => Path.Combine(temp, yuGiOh, nameof(Folder.Installer)),
            
            Folder.Data_TEST => Path.Combine(nameof(Folder.Data)),
            Folder.Packs_TEST => Path.Combine(nameof(Folder.Data), nameof(Folder.Packs)),
            Folder.Cards_TEST => Path.Combine(nameof(Folder.Data), nameof(Folder.Cards)),
            Folder.Images_TEST => Path.Combine(nameof(Folder.Data), nameof(Folder.Images)),
            Folder.Localization_TEST => Path.Combine(nameof(Folder.Data), nameof(Folder.Localization)),
            Folder.Websites_TEST => Path.Combine(nameof(Folder.Data), nameof(Folder.Websites_TEST)[..Folder.Websites_TEST.ToString().IndexOf('_')]),
            Folder.Serialize_TEST => Path.Combine(nameof(Folder.Data), nameof(Folder.Serialize_TEST)[..Folder.Serialize_TEST.ToString().IndexOf('_')]),
            
            _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, $"The enum [{nameof(Folder)}] doesn't contain {folder}")
        };
    }

    /// <summary>
    /// Returns the relative path to the given file's folder
    /// </summary>
    /// <param name="fileName">File to get the path to</param>
    /// <returns>
    /// <b>Item1: </b> The <see cref="Folder"/> <br/>
    /// <b>Item2: </b> Filename + extension
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">When <see cref="FileName"/> doesn't contain the given enum value</exception>
    private static (Folder folder, string file) GetFolderPath(FileName fileName)
    {
        return fileName switch
        {
            FileName.MissingPacks => (Folder.Packs, string.Concat(nameof(FileName.MissingPacks), ".", nameof(Extension.txt))),
            FileName.CardBack => (Folder.Images, string.Concat("0", ".", nameof(Extension.jpg))),
            FileName.KnownErrors => (Folder.Logging, string.Concat(nameof(FileName.KnownErrors), ".", nameof(Extension.csv))),
            
            _ => throw new ArgumentOutOfRangeException(nameof(fileName), fileName, $"The enum [{nameof(FileName)}] doesn't contain {fileName}")
        };
    }
    #endregion
}