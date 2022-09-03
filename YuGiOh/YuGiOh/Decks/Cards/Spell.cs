using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using YuGiOh_DeckBuilder.YuGiOh.Enums;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;

/// <summary>
/// Represents a spell card
/// </summary>
/// <param name="Localized">Contains localizations for a <see cref="ACard"/></param>
/// <param name="Endpoint">Endpoint for the card <see cref="ACard"/></param>
/// <param name="ImageEndpoint">Endpoint for the image of this <see cref="ACard"/></param>
/// <param name="Passcode">8-digit unique id</param>
/// <param name="Rarities">Every <see cref="Rarity"/> of this <see cref="ACard"/></param>
/// <param name="Statuses">Every <see cref="Status"/> of this <see cref="ACard"/></param>
/// <param name="PropertyType"><see cref="Enums.PropertyType"/> of this <see cref="Spell"/> card</param>
internal record Spell
(
    Localized Localized, 
    string Endpoint,
    string ImageEndpoint,
    int Passcode,
    IEnumerable<Rarity> Rarities,
    IEnumerable<Status> Statuses,
    PropertyType PropertyType

) : ACard(Localized, Endpoint, ImageEndpoint, Passcode, Rarities, Statuses)
{
    #region Properties
    /// <summary>
    /// The <see cref="Enums.CardType"/> of this <see cref="ACard"/>
    /// </summary>
    [JsonProperty(Order = 0)]
    public override CardType CardType { get; } = CardType.Spell;
    /// <summary>
    /// <see cref="Enums.PropertyType"/> of this <see cref="Spell"/> card
    /// </summary>
    [JsonProperty(Order = 4)]
    public PropertyType PropertyType { get; } = PropertyType;
    #endregion
    
    #region Methods
    internal override PropertyType GetPropertyType() => this.PropertyType;

    public override string ToString()
    {
        return   "{\n" +
                $"  \"{nameof(this.CardType)}\": \"{this.CardType.ToString()}\"\n" +
                $"  \"{nameof(base.Endpoint)}\": \"{YuGiOhFandom.BaseUri + base.Endpoint}\"\n" +
                $"  \"{nameof(base.ImageEndpoint)}\": \"{YuGiOhFandom.ImageBaseUri + base.ImageEndpoint}\"\n" +
                $"  \"{nameof(base.Passcode)}\": {(base.Passcode.ToString().Length <= 8 ? base.Passcode.ToString("00000000") : base.Passcode.ToString("000000000"))}\n" +
                $"  \"{nameof(this.PropertyType)}\": \"{this.PropertyType.ToString()}\"\n" +
                $"  \"{nameof(base.Rarities)}\": [ {string.Join(", ", base.Rarities.Select(rarity => $"\"{rarity.ToString()}\""))} ]\n" +
                $"  \"{nameof(base.Statuses)}\": [ {string.Join(", ", base.Statuses.Select(status => $"\"{status.ToString()}\""))} ]\n" +
                "}";
    }
    #endregion
}