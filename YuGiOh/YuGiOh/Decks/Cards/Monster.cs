using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using YuGiOh_DeckBuilder.YuGiOh.Enums;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;

/// <summary>
/// Represents a monster card
/// </summary>
/// <param name="Localized">Contains localizations for a <see cref="ACard"/></param>
/// <param name="Endpoint">Endpoint for the card <see cref="ACard"/></param>
/// <param name="ImageEndpoint">Endpoint for the image of this <see cref="ACard"/></param>
/// <param name="Passcode">8-digit unique id</param>
/// <param name="Rarities">Every <see cref="Rarity"/> of this <see cref="ACard"/></param>
/// <param name="Statuses">Every <see cref="Status"/> of this <see cref="ACard"/></param>
/// <param name="Level">Level/Rank (Stars/Link rating) of this <see cref="Monster"/> card</param>
/// <param name="Attribute"><see cref="Enums.Attribute"/> of this <see cref="Monster"/> card</param>
/// <param name="LinkArrows">Every <see cref="LinkArrow"/> of this <see cref="Monster"/> card</param>
/// <param name="PendulumScale">Pendulum scale of this <see cref="Monster"/> card</param>
/// <param name="MonsterTypes">All <see cref="MonsterType"/>s of this <see cref="Monster"/></param>
/// <param name="Attack">Attack value of this <see cref="Monster"/> card</param>
/// <param name="Defense">Defense value of this <see cref="Monster"/> card</param>
internal sealed record Monster
(
    Localized Localized, 
    string Endpoint,
    string ImageEndpoint,
    int Passcode,
    IEnumerable<Rarity> Rarities,
    IEnumerable<Status> Statuses,
    int Level,
    Attribute Attribute,
    IEnumerable<LinkArrow> LinkArrows,
    int PendulumScale,
    IEnumerable<MonsterType> MonsterTypes,
    int Attack,
    int Defense
    
) : ACard(Localized, Endpoint, ImageEndpoint, Passcode, Rarities, Statuses)
{
    #region Properties
    /// <summary>
    /// The <see cref="Enums.CardType"/> of this <see cref="ACard"/>
    /// </summary>
    [JsonProperty(Order = 0)]
    public override CardType CardType { get; } = CardType.Monster;
    /// <summary>
    /// Level/Rank (Stars/Link rating) of this <see cref="Monster"/> card
    /// </summary>
    [JsonProperty(Order = 4)]
    public int Level { get; } = Level;
    /// <summary>
    /// <see cref="Enums.Attribute"/> of this <see cref="Monster"/> card
    /// </summary>
    [JsonProperty(Order = 5)]
    public Attribute Attribute { get; } = Attribute;
    /// <summary>
    /// Every <see cref="LinkArrow"/> of this <see cref="Monster"/> card
    /// </summary>
    [JsonProperty(Order = 6)]
    public IEnumerable<LinkArrow> LinkArrows { get; } = LinkArrows;
    /// <summary>
    /// Pendulum scale of this <see cref="Monster"/> card
    /// </summary>
    [JsonProperty(Order = 7)]
    public int PendulumScale { get; } = PendulumScale;
    /// <summary>
    /// All <see cref="MonsterType"/>s of this <see cref="Monster"/>
    /// </summary>
    [JsonProperty(Order = 8)]
    public IEnumerable<MonsterType> MonsterTypes { get; } = MonsterTypes;
    /// <summary>
    /// Attack value of this <see cref="Monster"/> card <br/>
    /// <i>Will be -1 if the value would be '?'</i>
    /// </summary>
    [JsonProperty(Order = 9)]
    public int Attack { get; } = Attack;
    /// <summary>
    /// Defense value of this <see cref="Monster"/> card <br/>
    /// <i>Will be -1 if the value would be '?'</i>
    /// </summary>
    [JsonProperty(Order = 10)]
    public int Defense { get; } = Defense;
    #endregion
    
    #region Methods
    internal override IEnumerable<MonsterType> GetMonsterTypes() => this.MonsterTypes; // TODO: Initialize a single property for the MonsterType in -> FilterSettings.MonsterTypes / FilterSettings.Abilities / FilterSettings.Types

    internal override int GetLevel() => this.Level;
    
    internal override Attribute GetAttribute() => this.Attribute;
    
    internal override int GetPendulumScale() => this.PendulumScale;

    internal override IEnumerable<LinkArrow> GetLinkArrows() => this.LinkArrows;

    internal override int GetAttack() => this.Attack;
    
    internal override int GetDefense() => this.Defense;

    public override string ToString()
    {
        return  "{\n" +
               $"  \"{nameof(this.CardType)}\": \"{this.CardType.ToString()}\"\n" +
               $"  \"{nameof(base.Endpoint)}\": \"{YuGiOhFandom.BaseUri + base.Endpoint}\"\n" +
               $"  \"{nameof(base.ImageEndpoint)}\": \"{YuGiOhFandom.ImageBaseUri + base.ImageEndpoint}\"\n" +
               $"  \"{nameof(base.Passcode)}\": {(base.Passcode.ToString().Length <= 8 ? base.Passcode.ToString("00000000") : base.Passcode.ToString("000000000"))}\n" +
               $"  \"{nameof(this.Level)}\": {this.Level}\n" +
               $"  \"{nameof(this.Attribute)}\": \"{this.Attribute}\"\n" +
               $"  \"{nameof(this.LinkArrows)}\": [ {string.Join(", ", this.LinkArrows.Select(linkArrow => $"\"{linkArrow.ToString()}\""))} ]\n" +
               $"  \"{nameof(this.PendulumScale)}\": {this.PendulumScale}\n" +
               $"  \"{nameof(this.MonsterTypes)}\": [ {string.Join(", ", this.MonsterTypes.Select(monsterType => $"\"{monsterType.ToString()}\""))} ]\n" +
               $"  \"{nameof(this.Attack)}\": {this.Attack}\n" +
               $"  \"{nameof(this.Defense)}\": {this.Defense}\n" +
               $"  \"{nameof(base.Rarities)}\": [ {string.Join(", ", base.Rarities.Select(rarity => $"\"{rarity.ToString()}\""))} ]\n" +
               $"  \"{nameof(base.Statuses)}\": [ {string.Join(", ", base.Statuses.Select(status => $"\"{status.ToString()}\""))} ]\n" +
               "}";
    }
    #endregion
}