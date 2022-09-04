using System.IO;
using System.Linq;

namespace YuGiOh_DeckBuilder.Extensions;

/// <summary>
/// Extensions methods for <see cref="DirectoryInfo"/>
/// </summary>
public static class DirectoryInfoExtensions
{
    #region Methods
    /// <summary>
    /// Copies all directories and files from this <see cref="DirectoryInfo"/> to the target <see cref="DirectoryInfo"/>
    /// </summary>
    /// <param name="sourceDirectory">Directory to copy from</param>
    /// <param name="targetDirectory">Directory to copy to</param>
    public static void DeepCopy(this DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
    {
        Directory.CreateDirectory(targetDirectory.FullName);

        // Copy each file into the new directory
        foreach (var fileInfo in sourceDirectory.GetFiles())
        {
            var filePath = Path.Combine(targetDirectory.FullName, fileInfo.Name);
            fileInfo.CopyTo(filePath, true);
        }

        // Copy each subdirectory using recursion
        foreach (var childDirectory in sourceDirectory.GetDirectories())
        {
            var childTargetDirectory = targetDirectory.CreateSubdirectory(childDirectory.Name);
            childDirectory.DeepCopy(childTargetDirectory);
        }
    }
    
    /// <summary>
    /// Copies all directories and files from this <see cref="DirectoryInfo"/> to the target <see cref="DirectoryInfo"/>
    /// </summary>
    /// <param name="sourceDirectory">Directory to copy from</param>
    /// <param name="targetDirectory">Directory to copy to</param>
    /// <param name="excludePaths">Absolute paths Directories/Files to exclude from the deep copy</param>
    public static void DeepCopy(this DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory, params string[] excludePaths)
    {
        Directory.CreateDirectory(targetDirectory.FullName);

        // Copy each file into the new directory
        foreach (var fileInfo in sourceDirectory.GetFiles())
        {
            if (excludePaths.Contains(fileInfo.FullName)) continue;
            
            var filePath = Path.Combine(targetDirectory.FullName, fileInfo.Name);
            fileInfo.CopyTo(filePath, true);
        }

        // Copy each subdirectory using recursion
        foreach (var childDirectory in sourceDirectory.GetDirectories())
        {
            if (excludePaths.Contains(childDirectory.FullName)) continue;
            
            var childTargetDirectory = targetDirectory.CreateSubdirectory(childDirectory.Name);
            childDirectory.DeepCopy(childTargetDirectory);
        }
    }
    #endregion
}