// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
namespace YuGiOh_DeckBuilder.YuGiOh.Enums;

/// <summary>
/// All possible rarities <br/>
/// https://yugioh.fandom.com/wiki/Rarity
/// </summary>
internal enum Rarity
{
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Normal
    /// </summary>
    N,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Common
    /// </summary>
    C,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Normal_Rare
    /// </summary>
    NR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Short_Print
    /// </summary>
    SP,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Short_Print#Trivia
    /// </summary>
    SSP,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Holofoil_Rare
    /// </summary>
    HFR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Rare
    /// </summary>
    R,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Super_Rare
    /// </summary>
    SR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Ultra_Rare
    /// </summary>
    UR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Pharaoh%27s_Rare
    /// </summary>
    URPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Ultimate_Rare
    /// </summary>
    UtR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Ghost_Rare
    /// </summary>
    GR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Holographic_Rare
    /// </summary>
    HGR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Platinum_Rare
    /// </summary>
    PIR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Starlight_Rare
    /// </summary>
    StR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Secret_Rare
    /// </summary>
    ScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Prismatic_Secret_Rare
    /// </summary>
    PScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Extra_Secret_Rare
    /// </summary>
    EScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Platinum_Secret_Rare
    /// </summary>
    PlScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Ultra_Secret_Rare
    /// </summary>
    UScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/20th_Secret_Rare
    /// </summary>
    _20ScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Secret_Ultra_Rare
    /// </summary>
    ScUR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/10000_Secret_Rare
    /// </summary>
    _10000ScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Parallel_Rare
    /// </summary>
    PR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Normal_Parallel_Rare
    /// </summary>
    NPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Normal_Parallel_Rare
    /// </summary>
    PC,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Super_Parallel_Rare
    /// </summary>
    SPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Ultra_Parallel_Rare
    /// </summary>
    UPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Secret_Parallel_Rare
    /// </summary>
    ScPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Extra_Secret_Parallel_Rare
    /// </summary>
    EScPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Starfoil_Rare
    /// </summary>
    SFR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Mosaic_Rare
    /// </summary>
    MSR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Shatterfoil_Rare
    /// </summary>
    SHR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Collector%27s_Rare
    /// </summary>
    CR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Holographic_Parallel_Rare
    /// </summary>
    HGPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Kaiba_Corporation_Common
    /// </summary>
    KCC,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Kaiba_Corporation_Rare
    /// </summary>
    KCR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Kaiba_Corporation_Ultra_Rare
    /// </summary>
    KCUR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Gold_Rare
    /// </summary>
    GUR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Gold_Secret_Rare
    /// </summary>
    GScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Ghost/Gold_Rare
    /// </summary>
    GGR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Premium_Gold_Rare
    /// </summary>
    PGR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Millennium_Rare
    /// </summary>
    MLR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Millennium_Super_Rare
    /// </summary>
    MLSR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Millennium_Ultra_Rare
    /// </summary>
    MLUR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Millennium_Secret_Rare
    /// </summary>
    MLScR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Millennium_Gold_Rare
    /// </summary>
    MLGR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Duel_Terminal_Normal_Parallel_Rare
    /// </summary>
    DNPR,
    /// <summary>
    /// Duel Terminal Parallel Common
    /// </summary>
    DPC,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Duel_Terminal_Normal_Rare_Parallel_Rare
    /// </summary>
    DNRPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Duel_Terminal_Rare_Parallel_Rare
    /// </summary>
    DRPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Duel_Terminal_Super_Parallel_Rare
    /// </summary>
    DSPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Duel_Terminal_Ultra_Parallel_Rare
    /// </summary>
    DUPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Duel_Terminal_Secret_Parallel_Rare
    /// </summary>
    DScPR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Rush_Rare
    /// </summary>
    RR,
    /// <summary>
    /// Gold Rush Rare
    /// </summary>
    GRR,
    /// <summary>
    /// Over Rush Rare
    /// </summary>
    ORR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Standard_Edition
    /// </summary>
    SE,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/1st_Edition
    /// </summary>
    _1E,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Unlimited_Edition
    /// </summary>
    UE,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Limited_Edition
    /// </summary>
    LE,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Duel_Terminal_Edition
    /// </summary>
    DT,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Fixed_Rarity
    /// </summary>
    FR,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Variant_card
    /// </summary>
    Vc,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Original_Print
    /// </summary>
    OgP,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Replica
    /// </summary>
    RP,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Reprint
    /// </summary>
    Rp,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Out_of_Print
    /// </summary>
    OoP,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Giant_Card
    /// </summary>
    GC,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Official_Proxy
    /// </summary>
    OP,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Case_Topper
    /// </summary>
    CT,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Case_Topper <br/>
    /// <i>Oversized Promo</i>
    /// </summary>
    OSP,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/BAM_Legend
    /// </summary>
    BAM,
}