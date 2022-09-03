using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

internal static partial class Translator
{
    #region Properties
    /// <summary>
    /// Contains <see cref="Attribute"/> translations for every <see cref="Language"/> <br/>
    /// https://yugioh.fandom.com/wiki/Attribute
    /// </summary>
    internal static ReadOnlyDictionary<Language, IDictionary<Attribute, string>> Attributes { get; } = new
    (
        new Dictionary<Language, IDictionary<Attribute, string>>
        {
            {
                Language.en, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "DARK" },
                    { Attribute.DIVINE, "DIVINE" },
                    { Attribute.EARTH, "EARTH" },
                    { Attribute.FIRE, "FIRE" },
                    { Attribute.LIGHT, "LIGHT" },
                    { Attribute.WATER, "WATER" },
                    { Attribute.WIND, "WIND" }
                }
            },
            {
                Language.de, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "FINSTERNIS" },
                    { Attribute.DIVINE, "GÖTTLICH" },
                    { Attribute.EARTH, "ERDE" },
                    { Attribute.FIRE, "FEUER" },
                    { Attribute.LIGHT, "LICHT" },
                    { Attribute.WATER, "WASSER" },
                    { Attribute.WIND, "WIND" }
                }
            },
            {
                Language.es, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "OSCURIDAD" },
                    { Attribute.DIVINE, "DIVINIDAD" },
                    { Attribute.EARTH, "TIERRA" },
                    { Attribute.FIRE, "FUEGO" },
                    { Attribute.LIGHT, "LUZ" },
                    { Attribute.WATER, "AGUA" },
                    { Attribute.WIND, "VIENTO" }
                }
            },
            {
                Language.fr, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "TÉNÈBRES" },
                    { Attribute.DIVINE, "DIVIN" },
                    { Attribute.EARTH, "TERRE" },
                    { Attribute.FIRE, "FEU" },
                    { Attribute.LIGHT, "LUMIÈRE" },
                    { Attribute.WATER, "EAU" },
                    { Attribute.WIND, "VENT" }
                }
            },
            {
                Language.it, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "OSCURITÀ" },
                    { Attribute.DIVINE, "DIVINO" },
                    { Attribute.EARTH, "TERRA" },
                    { Attribute.FIRE, "FUOCO" },
                    { Attribute.LIGHT, "LUCE" },
                    { Attribute.WATER, "ACQUA" },
                    { Attribute.WIND, "VENTO" }
                }
            },
            {
                Language.ja, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "闇属性" },
                    { Attribute.DIVINE, "神属性" },
                    { Attribute.EARTH, "地属性" },
                    { Attribute.FIRE, "炎属性" },
                    { Attribute.LIGHT, "光属性" },
                    { Attribute.WATER, "水属性" },
                    { Attribute.WIND, "風属性" }
                }
            },
            {
                Language.ko, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "FINSTERNIS" },
                    { Attribute.DIVINE, "GÖTTLICH" },
                    { Attribute.EARTH, "ERDE" },
                    { Attribute.FIRE, "FEUER" },
                    { Attribute.LIGHT, "LICHT" },
                    { Attribute.WATER, "WASSER" },
                    { Attribute.WIND, "WIND" }
                }
            },
            {
                Language.pt, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "어둠" },
                    { Attribute.DIVINE, "신" },
                    { Attribute.EARTH, "땅" },
                    { Attribute.FIRE, "화염" },
                    { Attribute.LIGHT, "빛" },
                    { Attribute.WATER, "물" },
                    { Attribute.WIND, "바람" }
                }
            },
            {
                Language.zh, new Dictionary<Attribute, string>
                {
                    { Attribute.DARK, "黑暗的" },
                    { Attribute.DIVINE, "神圣的" },
                    { Attribute.EARTH, "地球" },
                    { Attribute.FIRE, "火" },
                    { Attribute.LIGHT, "光" },
                    { Attribute.WATER, "水" },
                    { Attribute.WIND, "风" }
                }
            }
        }
    );
    #endregion
}
