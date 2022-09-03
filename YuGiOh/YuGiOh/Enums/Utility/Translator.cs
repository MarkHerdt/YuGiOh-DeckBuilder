using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

/// <summary>
/// Translates values of the enums in <see cref="YuGiOh"/>.<see cref="YuGiOh.Enums"/>
/// </summary>
internal static partial class Translator
{
    #region Properties
    /// <summary>
    /// <see cref="Language"/> abbreviations in readable form (english)
    /// </summary>
    internal static readonly ReadOnlyDictionary<Language, string> Languages = new
    (
        new Dictionary<Language, string>
        {
            { Language.en, "English" },
            { Language.de, "German" },
            { Language.es, "Spanish" },
            { Language.fr, "French" },
            { Language.it, "Italian" },
            { Language.ja, "Japanese" },
            { Language.ko, "Korean" },
            { Language.pt, "Portuguese" },
            { Language.zh, "Chinese" }
        }
    );
    #endregion

    #region Methods
    /// <summary>
    /// Translates the given enumValue into the given language
    /// </summary>
    /// <param name="language">The language to translate to</param>
    /// <param name="enumValue">The value to translate</param>
    /// <typeparam name="T">Must be an <see cref="Enum"/> in the namespace <see cref="YuGiOh"/>.<see cref="YuGiOh.Enums"/></typeparam>
    /// <returns>The translation of the given value</returns>
    internal static string Translate<T>(Language language, T enumValue) where T : Enum
    {
        if (typeof(T) == typeof(Attribute))
        {
            return Attributes[language][(Attribute)(object)enumValue];
        }
        if (typeof(T) == typeof(LinkArrow))
        {
            return LinkArrows[language][(LinkArrow)(object)enumValue];
        }
        if (typeof(T) == typeof(MonsterType))
        {
            return MonsterTypes[language][(MonsterType)(object)enumValue];
        }
        if (typeof(T) == typeof(PropertyType))
        {
            return PropertyTypes[language][(PropertyType)(object)enumValue];
        }
        if (typeof(T) == typeof(Rarity))
        {
            return Rarities[language][(Rarity)(object)enumValue];
        }
        if (typeof(T) == typeof(Status))
        {
            return Statuses[language][(Status)(object)enumValue];
        }
        if (typeof(T) == typeof(ErrorMessage))
        {
            return Errors[language][(ErrorMessage)(object)enumValue];
        }

        return string.Empty;
    }

    /// <summary>
    /// Translates the given value into the given language
    /// </summary>
    /// <param name="language">The language to translate to</param>
    /// <param name="enum">The value to translate</param>
    /// <returns>The translation of the given value</returns>
    internal static string Translate(Language language, string @enum)
    {
        return Enums[language].TryGetValue(@enum, out var translation) ? translation : string.Empty;
    }
    #endregion
}