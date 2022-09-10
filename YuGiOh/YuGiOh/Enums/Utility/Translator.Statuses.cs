using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

internal static partial class Translator
{
    #region Properties
    /// <summary>
    /// Contains <see cref="Status"/> translations for every <see cref="Language"/> <br/>
    /// https://yugioh.fandom.com/wiki/Status
    /// </summary>
    internal static ReadOnlyDictionary<Language, IDictionary<Status, string>> Statuses { get; } = new
    (
        new Dictionary<Language, IDictionary<Status, string>>
        {
            {
                Language.en, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "Not yet released" },
                    { Status.Unlimited, "Unlimited" },
                    { Status.SemiLimited, "Semi-Limited" },
                    { Status.Limited, "Limited" },
                    { Status.Forbidden, "Forbidden" },
                    { Status.Illegal, "Illegal" }
                }
            },
            {
                Language.de, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "Noch nicht veröffentlicht" },
                    { Status.Unlimited, "Unbegrenzt" },
                    { Status.SemiLimited, "Semi-Limitiert" },
                    { Status.Limited, "Begrenzt" },
                    { Status.Forbidden, "Verboten" },
                    { Status.Illegal, "Illegal" }
                }
            },
            {
                Language.es, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "Aún no publicado" },
                    { Status.Unlimited, "Ilimitada" },
                    { Status.SemiLimited, "Semi-Limitado" },
                    { Status.Limited, "Limitada" },
                    { Status.Forbidden, "Prohibida" },
                    { Status.Illegal, "Ilegal" }
                }
            },
            {
                Language.fr, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "Pas encore publié" },
                    { Status.Unlimited, "Illimité" },
                    { Status.SemiLimited, "Semi-Limité" },
                    { Status.Limited, "Limitée" },
                    { Status.Forbidden, "Interdite" },
                    { Status.Illegal, "Illégale" }
                }
            },
            {
                Language.it, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "Non ancora pubblicato" },
                    { Status.Unlimited, "Illimitata" },
                    { Status.SemiLimited, "Semilimitato" },
                    { Status.Limited, "Limitata" },
                    { Status.Forbidden, "Proibita" },
                    { Status.Illegal, "Illegale" }
                }
            },
            {
                Language.ja, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "まだ発売されていない" },
                    { Status.Unlimited, "無制限" },
                    { Status.SemiLimited, "セミリミテッド" },
                    { Status.Limited, "限定" },
                    { Status.Forbidden, "禁断" },
                    { Status.Illegal, "違法" }
                }
            },
            {
                Language.ko, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "아직 출시되지 않음" },
                    { Status.Unlimited, "제한 없는" },
                    { Status.SemiLimited, "세미 리미티드" },
                    { Status.Limited, "제한된" },
                    { Status.Forbidden, "금지" },
                    { Status.Illegal, "불법적인" }
                }
            },
            {
                Language.pt, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "Ainda não lançado" },
                    { Status.Unlimited, "Ilimitada" },
                    { Status.SemiLimited, "Semilimitado" },
                    { Status.Limited, "Limitada" },
                    { Status.Forbidden, "Proibida" },
                    { Status.Illegal, "Ilegal" }
                }
            },
            {
                Language.zh, new Dictionary<Status, string>
                {
                    { Status.NotYetReleased, "尚未公布" },
                    { Status.Unlimited, "无限" },
                    { Status.SemiLimited, "半有限公司" },
                    { Status.Limited, "有限的" },
                    { Status.Forbidden, "禁止的" },
                    { Status.Illegal, "非法的" }
                }
            }
        }
    );
    #endregion
}