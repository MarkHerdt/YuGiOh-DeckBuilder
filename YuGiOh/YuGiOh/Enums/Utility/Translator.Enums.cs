using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

internal static partial class Translator
{
    #region Properties
    /// <summary>
    /// Contains <see cref="Enum"/> translations for every <see cref="Language"/>
    /// </summary>
    internal static ReadOnlyDictionary<Language, IDictionary<string, string>> Enums { get; } = new
    (
        new Dictionary<Language, IDictionary<string, string>>
        {
            {
                Language.en, new Dictionary<string, string>
                {
                    { nameof(Attribute), "Attribute" },
                    { nameof(LinkArrow), "Link-Arrows" },
                    { nameof(MonsterType), "Monster Type/s" },
                    { nameof(PropertyType), "Type" },
                    { nameof(Status), "Status" }
                }
            },
            {
                Language.de, new Dictionary<string, string>
                {
                    { nameof(Attribute), "Attribut" },
                    { nameof(LinkArrow), "Link-Pfeile" },
                    { nameof(MonsterType), "Monstertyp/en" },
                    { nameof(PropertyType), "Typ" },
                    { nameof(Status), "Status" }
                }
            },
            {
                Language.es, new Dictionary<string, string>
                {
                    { nameof(Attribute), "Atributo" },
                    { nameof(LinkArrow), "Flechas de enlace" },
                    { nameof(MonsterType), "Tipo/s de monstruo" },
                    { nameof(PropertyType), "Escribe" },
                    { nameof(Status), "Estado" }
                }
            },
            {
                Language.fr, new Dictionary<string, string>
                {
                    { nameof(Attribute), "Attribut" },
                    { nameof(LinkArrow), "Lien-Flèches" },
                    { nameof(MonsterType), "Type de monstre/s" },
                    { nameof(PropertyType), "Taper" },
                    { nameof(Status), "Statut" }
                }
            },
            {
                Language.it, new Dictionary<string, string>
                {
                    { nameof(Attribute), "Attributo" },
                    { nameof(LinkArrow), "Link-Frecce" },
                    { nameof(MonsterType), "Tipo/i di mostro" },
                    { nameof(PropertyType), "Tipa" },
                    { nameof(Status), "Stato" }
                }
            },
            {
                Language.ja, new Dictionary<string, string>
                {
                    { nameof(Attribute), "属性" },
                    { nameof(LinkArrow), "リンク矢印" },
                    { nameof(MonsterType), "モンスターの種類" },
                    { nameof(PropertyType), "タイプ" },
                    { nameof(Status), "状態" }
                }
            },
            {
                Language.ko, new Dictionary<string, string>
                {
                    { nameof(Attribute), "기인하다" },
                    { nameof(LinkArrow), "링크 화살표" },
                    { nameof(MonsterType), "몬스터 유형" },
                    { nameof(PropertyType), "유형" },
                    { nameof(Status), "상태" }
                }
            },
            {
                Language.pt, new Dictionary<string, string>
                {
                    { nameof(Attribute), "Atributo" },
                    { nameof(LinkArrow), "Setas de Link" },
                    { nameof(MonsterType), "Tipos de monstros" },
                    { nameof(PropertyType), "Modelo" },
                    { nameof(Status), "Status" }
                }
            },
            {
                Language.zh, new Dictionary<string, string>
                {
                    { nameof(Attribute), "屬性" },
                    { nameof(LinkArrow), "鏈接箭頭" },
                    { nameof(MonsterType), "怪物類型" },
                    { nameof(PropertyType), "类型" },
                    { nameof(Status), "地位" }
                }
            }
        }
    );
    #endregion
}