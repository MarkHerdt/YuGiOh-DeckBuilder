using System;
using System.IO;

namespace YuGiOh_DeckBuilder.Utility.Project;

public static class Structure
{
    #region Methods
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
        var fileStream = File.Open(filePath, fileMode);
        
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
    /// <returns>An absolute path to a file</returns>
    internal static string BuildPath(Folder folder, string fileName, Extension fileExtension)
    {
        var file = string.Concat(fileName, ".", fileExtension);
        var folderPath = BuildPath(folder);
        var filePath = Path.Combine(folderPath, file);

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
    #endregion
}