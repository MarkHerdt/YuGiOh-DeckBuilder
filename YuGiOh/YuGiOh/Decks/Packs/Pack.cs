using System;
using System.Collections.Generic;
using System.Linq;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;

/// <summary>
/// Represents one pack on: <br/>
/// https://yugioh.fandom.com/wiki/Booster_Pack
/// </summary>
/// <param name="Name">The name of the pack</param>
/// <param name="ReleaseDate">The date, this pack was released</param>
/// <param name="Endpoint">Endpoint for this pack on https://yugioh.fandom.com/wiki/</param>
/// <param name="Cards">Endpoints and rarities of all cards in this <see cref="Pack"/></param>
internal sealed record Pack(string Name, DateTime ReleaseDate, string Endpoint, ICollection<Card> Cards)
{
    #region Properties
    /// <summary>
    /// The name of the pack
    /// </summary>
    public string Name { get; } = Name;
    /// <summary>
    /// The date, this pack was released
    /// </summary>
    public DateTime ReleaseDate { get; } = ReleaseDate;
    /// <summary>
    /// Endpoint for this pack on https://yugioh.fandom.com/wiki/
    /// </summary>
    public string Endpoint { get; } = Endpoint;
    /// <summary>
    /// Endpoints and rarities of all cards in this <see cref="Pack"/>
    /// </summary>
    // ReSharper disable once ReturnTypeCanBeEnumerable.Global
    public ICollection<Card> Cards { get; } = Cards;
    #endregion

    #region Methods
    public override string ToString()
    {
        return  "{\n" +
               $"  \"{nameof(this.Name)}\": \"{this.Name}\"\n" +
               $"  \"{nameof(this.ReleaseDate)}\": \"{this.ReleaseDate}\"\n" +
               $"  \"{nameof(this.Endpoint)}\": \"{this.Endpoint}\"\n" +
               $"  \"{nameof(this.Cards)}\": [ " + "{\n" +
               $"  {string.Join("\n  }, {\n", this.Cards.Select(card => card.ToString()))}\n" +
                "    } ]\n" +
                "}";
    }
    #endregion
}