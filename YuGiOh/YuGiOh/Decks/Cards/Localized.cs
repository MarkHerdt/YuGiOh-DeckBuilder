using System;
using System.Collections.Generic;
using System.Linq;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;

/// <summary>
/// Contains localizations for a <see cref="ACard"/>
/// </summary>
internal class Localized
{
    #region Statics
    /// <summary>
    /// Error messages for <see cref="MainWindow.Cards"/> that are present in <see cref="MainWindow.Packs"/> but not in <see cref="MainWindow.Cards"/>
    /// </summary>
    internal static readonly Localized Error = new()
    {
        Name = Enum.GetValues<Language>().ToDictionary(@enum => @enum, @enum => Translator.Translate(@enum, ErrorMessage.Name)),
        Description = Enum.GetValues<Language>().ToDictionary(@enum => @enum, @enum => Translator.Translate(@enum, ErrorMessage.Description))
    };
    #endregion
    
    #region Properties
    /// <summary>
    /// Localized names
    /// </summary>
    public Dictionary<Language, string> Name { get; private init; } = new();
    /// <summary>
    /// Localized description
    /// </summary>
    public Dictionary<Language, string> Description { get; private init; } = new();
    #endregion

    #region Constructor
    internal Localized()
    {
        foreach (var language in Enum.GetValues<Language>())
        {
            Name.Add(language, string.Empty);
            Description.Add(language, string.Empty);
        }
    }
    #endregion

    #region Methods
    public override string ToString()
    {
        return  "{\n" +
               $"  \"{nameof(this.Name)}\": " + "{\n" +
               $"{string.Join(",\n", this.Name.Select(kvp => string.Concat($"    \"{kvp.Key.ToString()}\": ", $"\"{kvp.Value}\"")))}\n" +
                "  },\n" +
               $"{string.Join(",\n", this.Description.Select(kvp => string.Concat($"    \"{kvp.Key.ToString()}\": ", $"\"{kvp.Value}\"")))}\n" +
                "  }\n" +
                "}";
    }
    #endregion
}