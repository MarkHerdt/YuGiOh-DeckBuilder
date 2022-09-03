using System.Collections.Generic;
using Newtonsoft.Json;
using YuGiOh_DeckBuilder.YuGiOh.Enums;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;

/// <summary>
/// Represents one card in a <see cref="Pack"/>
/// </summary>
/// <param name="EndPoint">Endpoint for this card</param>
/// <param name="Rarities">Rarities for this card</param>
internal sealed record Card(string EndPoint, IEnumerable<Rarity> Rarities)
{
    #region Properties
    /// <summary>
    /// Endpoint for this card
    /// </summary>
    public string EndPoint { get; } = EndPoint;
    /// <summary>
    /// 8-digit unique id
    /// </summary>
    public int Passcode { get; set; } = -1;
    /// <summary>
    /// Rarities for this card
    /// </summary>
    [JsonIgnore]
    internal IEnumerable<Rarity> Rarities { get; } = Rarities;
    /// <summary>
    /// Index of this card in <see cref="MainWindow.CardImages"/> 
    /// </summary>
    [JsonIgnore]
    internal int Index { get; set; }
    #endregion

    #region Methods
    public override string ToString()
    {
        return $"    \"{nameof(this.EndPoint)}\": \"{this.EndPoint}\",\n" +
               $"    \"{nameof(this.Passcode)}\": {this.Passcode}";
    }
    #endregion
}