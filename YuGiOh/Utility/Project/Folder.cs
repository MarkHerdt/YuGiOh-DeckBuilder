// ReSharper disable InconsistentNaming
namespace YuGiOh_DeckBuilder.Utility.Project;

/// <summary>
/// Represent folders in this solution <br/>
/// <i>Folder in YuGiOh.cs have values greater than 0</i> <br/>
/// <i>Folder in YuGiOh.Tests.cs have values less than 0</i>
/// </summary>
internal enum Folder
{
    /* Folder in YuGiOh.cs have values > 0 */
    
    /// <summary>
    /// YuGiOh/YuGiOh/YuGiOh/Data/
    /// </summary>
    Data = 1,
    /// <summary>
    /// YuGiOh/YuGiOh/YuGiOh/Data/Packs/
    /// </summary>
    Packs = 2,
    /// <summary>
    /// YuGiOh/YuGiOh/YuGiOh/Data/Cards/
    /// </summary>
    Cards = 3,
    /// <summary>
    /// YuGiOh/YuGiOh/YuGiOh/Data/Images/
    /// </summary>
    Images = 4,
    /// <summary>
    /// YuGiOh/YuGiOh/YuGiOh/Data/Localization/
    /// </summary>
    Localization = 5,
    /// <summary>
    /// YuGiOh/YuGiOh/YuGiOh/Data/Export/
    /// </summary>
    Export = 6,
    /// <summary>
    /// YuGiOh/YuGiOh/YuGiOh/Data/Logging/
    /// </summary>
    Logging = 7,
    
    /* Folder in YuGiOh.Tests.cs have values < 0 */
    
    /// <summary>
    /// YuGiOh.Tests/Data/
    /// </summary>
    Data_TEST = -1,
    /// <summary>
    /// YuGiOh.Tests/Data/Packs/
    /// </summary>
    Packs_TEST = -2,
    /// <summary>
    /// YuGiOh.Tests/Data/Cards/
    /// </summary>
    Cards_TEST = -3,
    /// <summary>
    /// YuGiOh.Tests/Data/Images/
    /// </summary>
    Images_TEST = -4,
    /// <summary>
    /// YuGiOh.Tests/Data/Localization/
    /// </summary>
    Localization_TEST = -5,
    /// <summary>
    /// YuGiOh.Tests/Data/Websites/
    /// </summary>
    Websites_TEST = -6,
    /// <summary>
    /// YuGiOh.Tests/Data/Serialize/
    /// </summary>
    Serialize_TEST = -7
}