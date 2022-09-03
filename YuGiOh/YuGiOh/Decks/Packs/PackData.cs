using System;
using System.Collections.Generic;
using System.Text;
using YuGiOh_DeckBuilder.Utility.ObjectPools;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;

/// <summary>
/// Contains all data for a pack to be created
/// </summary>
/// <param name="PackEndpoint">Endpoint of this pack</param>
internal sealed record PackData(string PackEndpoint)
{
    #region Properties
    /// <summary>
    /// Endpoint of this pack
    /// </summary>
    internal string PackEndpoint { get; } = PackEndpoint;
    /// <summary>
    /// <see cref="PoolItem{T}"/> containing a <see cref="System.Text.StringBuilder"/>
    /// </summary>
    internal PoolItem<StringBuilder> StringBuilder { get; } = MainWindow.StringBuilderPool.GetObject();

    /// <summary>
    /// Name of this booster pack
    /// </summary>
    internal string? PackName { get; set; }
    /// <summary>
    /// Date when this booster pack was released
    /// </summary>
    internal DateTime? ReleaseDate { get; set; }
    
    /// <summary>
    /// Indicates, that the table, contain all card in this booster pack, has been found
    /// </summary>
    internal bool CardTableFound { get; set; }
    /// <summary>
    /// Indicates, that the search for this booster pack should be stopped
    /// </summary>
    internal bool StopSearch { get; set; }
    
    /// <summary>
    /// Endpoints and rarities of all cards in this <see cref="Pack"/>
    /// </summary>
    internal List<Card> Cards { get; } = new();
    #endregion
};