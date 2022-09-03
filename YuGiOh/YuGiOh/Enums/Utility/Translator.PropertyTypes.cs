using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

internal static partial class Translator
{
    #region Properties
    /// <summary>
    /// Contains <see cref="PropertyType"/> translations for every <see cref="Language"/> <br/>
    /// https://yugioh.fandom.com/wiki/Spell_Card?so=search <br/>
    /// https://yugioh.fandom.com/wiki/Trap_Card?so=search
    /// </summary>
    internal static ReadOnlyDictionary<Language, IDictionary<PropertyType, string>> PropertyTypes { get; } = new
    (
        new Dictionary<Language, IDictionary<PropertyType, string>>
        {
            {
                Language.en, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "Normal" },
                    { PropertyType.Continuous, "Continuous" },
                    { PropertyType.Counter, "Counter" },
                    { PropertyType.Equip, "Equip" },
                    { PropertyType.Field, "Field" },
                    { PropertyType.QuickPlay, "Quick-Play" },
                    { PropertyType.Ritual, "Ritual" }
                }
            },
            {
                Language.de, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "Normal" },
                    { PropertyType.Continuous, "Permanent" },
                    { PropertyType.Counter, "Konter" },
                    { PropertyType.Equip, "Ausrüstung" },
                    { PropertyType.Field, "Spielfeld" },
                    { PropertyType.QuickPlay, "Schnell" },
                    { PropertyType.Ritual, "Ritual" }
                }
            },
            {
                Language.es, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "Normales" },
                    { PropertyType.Continuous, "Continua" },
                    { PropertyType.Counter, "Contraefecto" },
                    { PropertyType.Equip, "Equipo" },
                    { PropertyType.Field, "Campo" },
                    { PropertyType.QuickPlay, "Juego Rápido" },
                    { PropertyType.Ritual, "Ritual" }
                }
            },
            {
                Language.fr, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "Normale" },
                    { PropertyType.Continuous, "Continu" },
                    { PropertyType.Counter, "Contre" },
                    { PropertyType.Equip, "Équipement" },
                    { PropertyType.Field, "Terrain" },
                    { PropertyType.QuickPlay, "Jeu-Rapide" },
                    { PropertyType.Ritual, "Rituel" }
                }
            },
            {
                Language.it, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "Normali" },
                    { PropertyType.Continuous, "Continua" },
                    { PropertyType.Counter, "Contro" },
                    { PropertyType.Equip, "Equipaggiamento" },
                    { PropertyType.Field, "Terreno" },
                    { PropertyType.QuickPlay, "Rapida" },
                    { PropertyType.Ritual, "Rituale" }
                }
            },
            {
                Language.ja, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "通常" },
                    { PropertyType.Continuous, "永続" },
                    { PropertyType.Counter, "カウンター" },
                    { PropertyType.Equip, "装備" },
                    { PropertyType.Field, "フィールド" },
                    { PropertyType.QuickPlay, "速攻" },
                    { PropertyType.Ritual, "儀式" }
                }
            },
            {
                Language.ko, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "일반" },
                    { PropertyType.Continuous, "지속" },
                    { PropertyType.Counter, "카운터" },
                    { PropertyType.Equip, "장착" },
                    { PropertyType.Field, "필드" },
                    { PropertyType.QuickPlay, "속공" },
                    { PropertyType.Ritual, "의식" }
                }
            },
            {
                Language.pt, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "Normal" },
                    { PropertyType.Continuous, "Contínua" },
                    { PropertyType.Counter, "Marcador" },
                    { PropertyType.Equip, "Equipamento" },
                    { PropertyType.Field, "Campo" },
                    { PropertyType.QuickPlay, "Rápida" },
                    { PropertyType.Ritual, "Ritual" }
                }
            },
            {
                Language.zh, new Dictionary<PropertyType, string>
                {
                    { PropertyType.Normal, "普通的" },
                    { PropertyType.Continuous, "连续的" },
                    { PropertyType.Counter, "连续的" },
                    { PropertyType.Equip, "装备" },
                    { PropertyType.Field, "场地" },
                    { PropertyType.QuickPlay, "快玩" },
                    { PropertyType.Ritual, "仪式" }
                }
            }
        }
    );
    #endregion
}
