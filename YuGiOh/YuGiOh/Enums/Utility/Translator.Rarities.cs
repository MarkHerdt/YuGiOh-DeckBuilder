using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

internal static partial class Translator
{
    #region Properties
    /// <summary>
    /// Contains <see cref="Rarity"/> translations for every <see cref="Language"/> <br/>
    /// https://yugioh.fandom.com/wiki/Rarity
    /// </summary>
    internal static ReadOnlyDictionary<Language, IDictionary<Rarity, string>> Rarities { get; } = new
    (
        new Dictionary<Language, IDictionary<Rarity, string>>
        {
            {
                Language.en, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "Normal" },
                    { Rarity.C, "Common" },
                    { Rarity.NR, "Normal Rare" },
                    { Rarity.SP, "Short Print" },
                    { Rarity.SSP, "Super Short Print" },
                    { Rarity.R, "Rare" },
                    { Rarity.SR, "Super Rare" },
                    { Rarity.HFR, "Holofoil Rare" },
                    { Rarity.UR, "Ultra Rare" },
                    { Rarity.UtR, "Ultimate Rare" },
                    { Rarity.ScR, "Secret Rare" },
                    { Rarity.UScR, "Ultra Secret Rare" },
                    { Rarity.ScUR, "Secret Ultra Rare" },
                    { Rarity.PScR, "Prismatic Secret Rare" },
                    { Rarity.HGR, "Holographic Rare" },
                    { Rarity.GR, "Ghost Rare" },
                    { Rarity.PR, "Parallel Rare" },
                    { Rarity.NPR, "Normal Parallel Rare" },
                    { Rarity.PC, "Parallel Common" },
                    { Rarity.SPR, "Super Parallel Rare" },
                    { Rarity.UPR, "Ultra Parallel Rare" },
                    { Rarity.DNPR, "Duel Terminal Normal Parallel Rare" },
                    { Rarity.DPC, "Duel Terminal Parallel Common" },
                    { Rarity.DRPR, "Duel Terminal Rare Parallel Rare" },
                    { Rarity.DSPR, "Duel Terminal Super Parallel Rare" },
                    { Rarity.DUPR, "Duel Terminal Ultra Parallel Rare" },
                    { Rarity.DScPR, "Duel Terminal Secret Parallel Rare" },
                    { Rarity.GUR, "Gold Rare" },
                    { Rarity.STR, "Starlight Rare" }
                }
            },
            {
                Language.de, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "Normal" },
                    { Rarity.C, "Verbreitet" },
                    { Rarity.NR, "Normal selten" },
                    { Rarity.SP, "Kurzdruck" },
                    { Rarity.SSP, "Super kurzer Druck" },
                    { Rarity.R, "Selten" },
                    { Rarity.SR, "Sehr selten" },
                    { Rarity.HFR, "Holofoil Selten" },
                    { Rarity.UR, "Sehr selten" },
                    { Rarity.UtR, "Ultimativ selten" },
                    { Rarity.ScR, "Geheim selten" },
                    { Rarity.UScR, "Ultrageheim selten" },
                    { Rarity.ScUR, "Geheim Ultra selten" },
                    { Rarity.PScR, "Prismatisch Geheim Selten" },
                    { Rarity.HGR, "Holographisch selten" },
                    { Rarity.GR, "Geist Selten" },
                    { Rarity.PR, "Parallel Selten" },
                    { Rarity.NPR, "Normal Parallel Selten" },
                    { Rarity.PC, "Parallel Verbreitet" },
                    { Rarity.SPR, "Superparallel Selten" },
                    { Rarity.UPR, "Ultraparallel Selten" },
                    { Rarity.DNPR, "Duel Terminal Normal Parallel Selten" },
                    { Rarity.DPC, "Duel Terminal Parallel Verbreitet" },
                    { Rarity.DRPR, "Duel Terminal Selten Parallel Selten" },
                    { Rarity.DSPR, "Duel Terminal Super Parallel Selten" },
                    { Rarity.DUPR, "Duel Terminal Ultra Parallel Selten" },
                    { Rarity.DScPR, "Duel Terminal Geheim Parallel Selten" },
                    { Rarity.GUR, "Gold selten" },
                    { Rarity.STR, "Sternenlicht Selten" }
                }
            },
            {
                Language.es, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "Normal" },
                    { Rarity.C, "Común" },
                    { Rarity.NR, "Rara normal" },
                    { Rarity.SP, "Impresión corta" },
                    { Rarity.SSP, "Estampado súper corto" },
                    { Rarity.R, "Extraña" },
                    { Rarity.SR, "Super rara" },
                    { Rarity.HFR, "Holofoil rara" },
                    { Rarity.UR, "Ultra raro" },
                    { Rarity.UtR, "Última rara" },
                    { Rarity.ScR, "Rara secreta" },
                    { Rarity.UScR, "Raro Ultra Secreto" },
                    { Rarity.ScUR, "Secreto ultra raro" },
                    { Rarity.PScR, "Secreto Prismático Raro" },
                    { Rarity.HGR, "Rara holográfica" },
                    { Rarity.GR, "Fantasma rara" },
                    { Rarity.PR, "Paralelo raro" },
                    { Rarity.NPR, "Rara paralela normal" },
                    { Rarity.PC, "Paralelo Común" },
                    { Rarity.SPR, "Super paralela rara" },
                    { Rarity.UPR, "Ultra Paralelo Raro" },
                    { Rarity.DNPR, "Duelo Terminal Normal Paralela Rara" },
                    { Rarity.DPC, "Duel Terminal Paralelo Común" },
                    { Rarity.DRPR, "Duelo Terminal Rara Paralela Rara" },
                    { Rarity.DSPR, "Duelo Terminal Super Paralela Rara" },
                    { Rarity.DUPR, "Duel Terminal Ultra Paralelo Raro" },
                    { Rarity.DScPR, "Duel Terminal Secreto Paralelo Raro" },
                    { Rarity.GUR, "Oro raro" },
                    { Rarity.STR, "Luz estelar rara" }
                }
            },
            {
                Language.fr, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "Normale" },
                    { Rarity.C, "Commune" },
                    { Rarity.NR, "Normal Rare" },
                    { Rarity.SP, "Impression courte" },
                    { Rarity.SSP, "Impression super courte" },
                    { Rarity.R, "Rare" },
                    { Rarity.SR, "Super rare" },
                    { Rarity.HFR, "Holofil Rare" },
                    { Rarity.UR, "Ultra rare" },
                    { Rarity.UtR, "Rare ultime" },
                    { Rarity.ScR, "Secrète rare" },
                    { Rarity.UScR, "Rare ultra secrète" },
                    { Rarity.ScUR, "Secrète Ultra Rare" },
                    { Rarity.PScR, "Secrète Prismatique Rare" },
                    { Rarity.HGR, "Rare holographique" },
                    { Rarity.GR, "Fantôme rare" },
                    { Rarity.PR, "Parallèle Rare" },
                    { Rarity.NPR, "Normal Parallèle Rare" },
                    { Rarity.PC, "Commun parallèle" },
                    { Rarity.SPR, "Super Parallèle Rare" },
                    { Rarity.UPR, "Ultra Parallèle Rare" },
                    { Rarity.DNPR, "Duel Terminal Normal Parallèle Rare" },
                    { Rarity.DPC, "Duel Terminal Parallèle Commun" },
                    { Rarity.DRPR, "Duel Terminal Rare Parallèle Rare" },
                    { Rarity.DSPR, "Duel Terminal Super Parallèle Rare" },
                    { Rarity.DUPR, "Duel Terminal Ultra Parallèle Rare" },
                    { Rarity.DScPR, "Duel Terminal Secret Parallèle Rare" },
                    { Rarity.GUR, "Or rares" },
                    { Rarity.STR, "Lumière des étoiles rare" }
                }
            },
            {
                Language.it, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "Normale" },
                    { Rarity.C, "Comune" },
                    { Rarity.NR, "Normale Raro" },
                    { Rarity.SP, "Stampa corta" },
                    { Rarity.SSP, "Stampa super corta" },
                    { Rarity.R, "Rara" },
                    { Rarity.SR, "Super raro" },
                    { Rarity.HFR, "Foglio olografico raro" },
                    { Rarity.UR, "Estremamente raro" },
                    { Rarity.UtR, "Ultimo raro" },
                    { Rarity.ScR, "Segreto raro" },
                    { Rarity.UScR, "Ultra Segreto Raro" },
                    { Rarity.ScUR, "Segreto ultra raro" },
                    { Rarity.PScR, "Prismatico Segreto Raro" },
                    { Rarity.HGR, "Olografica rara" },
                    { Rarity.GR, "Fantasma raro" },
                    { Rarity.PR, "Parallelo Raro" },
                    { Rarity.NPR, "Normale Parallelo Raro" },
                    { Rarity.PC, "Comune parallelo" },
                    { Rarity.SPR, "Super Parallelo Raro" },
                    { Rarity.UPR, "Ultra Parallelo Raro" },
                    { Rarity.DNPR, "Duel Terminal Normale Parallelo Raro" },
                    { Rarity.DPC, "Duel Terminal Parallelo Comune" },
                    { Rarity.DRPR, "Duel Terminal Raro Parallelo Raro" },
                    { Rarity.DSPR, "Duel Terminal Super Parallel Raro" },
                    { Rarity.DUPR, "Duel Terminal Ultra Parallelo Raro" },
                    { Rarity.DScPR, "Duel Terminal Segreto Parallelo Raro" },
                    { Rarity.GUR, "Oro raro" },
                    { Rarity.STR, "Starlight raro" }
                }
            },
            {
                Language.ja, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "普通" },
                    { Rarity.C, "一般" },
                    { Rarity.NR, "ノーマルレア" },
                    { Rarity.SP, "ショートプリント" },
                    { Rarity.SSP, "スーパーショートプリント" },
                    { Rarity.R, "レア" },
                    { Rarity.SR, "スーパーレア" },
                    { Rarity.HFR, "ホロフォイルレア" },
                    { Rarity.UR, "ウルトラ・レア" },
                    { Rarity.UtR, "アルティメットレア" },
                    { Rarity.ScR, "シークレットレア" },
                    { Rarity.UScR, "ウルトラシークレットレア" },
                    { Rarity.ScUR, "シークレットウルトラレア" },
                    { Rarity.PScR, "プリズマティックシークレットレア" },
                    { Rarity.HGR, "ホログラフィックレア" },
                    { Rarity.GR, "ゴーストレア" },
                    { Rarity.PR, "パラレルレア" },
                    { Rarity.NPR, "ノーマルパラレルレア" },
                    { Rarity.PC, "パラレルコモン" },
                    { Rarity.SPR, "スーパーパラレルレア" },
                    { Rarity.UPR, "ウルトラパラレルレア" },
                    { Rarity.DNPR, "デュエルターミナル ノーマルパラレルレア" },
                    { Rarity.DPC, "デュエルターミナル パラレルコモン" },
                    { Rarity.DRPR, "デュエルターミナルレア パラレルレア" },
                    { Rarity.DSPR, "デュエルターミナル スーパーパラレルレア" },
                    { Rarity.DUPR, "デュエルターミナル ウルトラパラレルレア" },
                    { Rarity.DScPR, "デュエルターミナル シークレット パラレルレア" },
                    { Rarity.GUR, "ゴールドレア" },
                    { Rarity.STR, "スターライトレア" }
                }
            },
            {
                Language.ko, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "정상" },
                    { Rarity.C, "흔한" },
                    { Rarity.NR, "노멀 레어" },
                    { Rarity.SP, "짧은 인쇄" },
                    { Rarity.SSP, "슈퍼 쇼트 프린트" },
                    { Rarity.R, "희귀한" },
                    { Rarity.SR, "슈퍼 레어" },
                    { Rarity.HFR, "홀로포일 희귀" },
                    { Rarity.UR, "울트라 레어" },
                    { Rarity.UtR, "얼티밋 레어" },
                    { Rarity.ScR, "시크릿 레어" },
                    { Rarity.UScR, "울트라 시크릿 레어" },
                    { Rarity.ScUR, "비밀 울트라 레어" },
                    { Rarity.PScR, "분광의 비밀 희귀" },
                    { Rarity.HGR, "홀로그램 희귀" },
                    { Rarity.GR, "고스트 레어" },
                    { Rarity.PR, "병렬 희귀" },
                    { Rarity.NPR, "노멀 패러렐 레어" },
                    { Rarity.PC, "병렬 공통" },
                    { Rarity.SPR, "슈퍼 병렬 희귀" },
                    { Rarity.UPR, "울트라 패러렐 레어" },
                    { Rarity.DNPR, "듀얼 터미널 노멀 패러럴 레어" },
                    { Rarity.DPC, "듀얼 터미널 병렬 공통" },
                    { Rarity.DRPR, "듀얼 터미널 레어 패러럴 레어" },
                    { Rarity.DSPR, "듀얼 터미널 슈퍼 패러럴 레어" },
                    { Rarity.DUPR, "듀얼 터미널 울트라 패러럴 레어" },
                    { Rarity.DScPR, "듀얼 터미널 시크릿 패러럴 레어" },
                    { Rarity.GUR, "골드 레어" },
                    { Rarity.STR, "별빛 희귀" }
                }
            },
            {
                Language.pt, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "Normal" },
                    { Rarity.C, "Comum" },
                    { Rarity.NR, "Normal Raro" },
                    { Rarity.SP, "Impressão curta" },
                    { Rarity.SSP, "Impressão super curta" },
                    { Rarity.R, "Crua" },
                    { Rarity.SR, "Super raro" },
                    { Rarity.HFR, "Holofólio Raro" },
                    { Rarity.UR, "Ultra raro" },
                    { Rarity.UtR, "Raro Supremo" },
                    { Rarity.ScR, "Segredo Raro" },
                    { Rarity.UScR, "Ultra Secreto Raro" },
                    { Rarity.ScUR, "Segredo Ultra Raro" },
                    { Rarity.PScR, "Segredo Prismático Raro" },
                    { Rarity.HGR, "Raro holográfico" },
                    { Rarity.GR, "Fantasma Raro" },
                    { Rarity.PR, "Paralelo Raro" },
                    { Rarity.NPR, "Normal Paralelo Raro" },
                    { Rarity.PC, "Paralelo Comum" },
                    { Rarity.SPR, "Super Paralelo Raro" },
                    { Rarity.UPR, "Ultra Paralelo Raro" },
                    { Rarity.DNPR, "Duelo Terminal Normal Paralelo Raro" },
                    { Rarity.DPC, "Duel Terminal Paralelo Comum" },
                    { Rarity.DRPR, "Duel Terminal Raro Paralelo Raro" },
                    { Rarity.DSPR, "Duel Terminal Super Paralelo Raro" },
                    { Rarity.DUPR, "Duel Terminal Ultra Paralelo Raro" },
                    { Rarity.DScPR, "Duel Terminal Secret Paralelo Raro" },
                    { Rarity.GUR, "Ouro Raro" },
                    { Rarity.STR, "Starlight Raro" }
                }
            },
            {
                Language.zh, new Dictionary<Rarity, string>
                {
                    { Rarity.N, "普通的" },
                    { Rarity.C, "常见的" },
                    { Rarity.NR, "普通 稀有" },
                    { Rarity.SP, "短版印刷" },
                    { Rarity.SSP, "超短版印刷" },
                    { Rarity.R, "稀有的" },
                    { Rarity.SR, "超级稀有" },
                    { Rarity.HFR, "罕见的全息图" },
                    { Rarity.UR, "超稀有" },
                    { Rarity.UtR, "终极稀有" },
                    { Rarity.ScR, "秘密稀有" },
                    { Rarity.UScR, "超秘密稀有" },
                    { Rarity.ScUR, "秘密超稀有" },
                    { Rarity.PScR, "棱镜秘密 稀有" },
                    { Rarity.HGR, "全息稀有" },
                    { Rarity.GR, "幽灵稀有" },
                    { Rarity.PR, "平行稀有" },
                    { Rarity.NPR, "普通平行稀有" },
                    { Rarity.PC, "并行公共" },
                    { Rarity.SPR, "超平行稀有" },
                    { Rarity.UPR, "超平行稀有" },
                    { Rarity.DNPR, "决斗终端 普通 平行 稀有" },
                    { Rarity.DPC, "决斗终端并行通用" },
                    { Rarity.DRPR, "决斗终端 稀有 平行 稀有" },
                    { Rarity.DSPR, "决斗终端超级平行稀有" },
                    { Rarity.DUPR, "决斗终端超平行稀有" },
                    { Rarity.DScPR, "决斗终端秘密平行稀有" },
                    { Rarity.GUR, "黄金稀有" },
                    { Rarity.STR, "星光稀有" }
                }
            }
        }
    );
    #endregion
}