using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.Utility.Json.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using Attribute = YuGiOh_DeckBuilder.YuGiOh.Enums.Attribute;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;

/// <summary>
/// Base class for all cards
/// </summary>
/// <param name="Localized">Contains localizations for a <see cref="ACard"/></param>
/// <param name="Endpoint">Endpoint for the card <see cref="ACard"/></param>
/// <param name="ImageEndpoint">Endpoint for the image of this <see cref="ACard"/></param>
/// <param name="Passcode">8-digit unique id</param>
/// <param name="Rarities">Every <see cref="Rarity"/> of this <see cref="ACard"/></param>
/// <param name="Statuses">Every <see cref="Status"/> of this <see cref="ACard"/></param>
[JsonConverter(typeof(CardConverter))]
internal abstract record ACard(Localized Localized, string Endpoint, string ImageEndpoint, int Passcode, IEnumerable<Rarity> Rarities, IEnumerable<Status> Statuses)
{
    #region Properties
    /// <summary>
    /// Contains localizations for a <see cref="ACard"/>
    /// </summary>
    [JsonIgnore]
    internal Localized Localized { get; set; } = Localized;
    /// <summary>
    /// The <see cref="Enums.CardType"/> of this <see cref="ACard"/>
    /// </summary>
    [JsonProperty(Order = 0)]
    public abstract CardType CardType { get; }
    /// <summary>
    /// Endpoint for the card
    /// </summary>
    [JsonProperty(Order = 1)]
    public string Endpoint { get; } = Endpoint;
    /// <summary>
    /// Uri the the image of this <see cref="ACard"/>
    /// </summary>
    [JsonProperty(Order = 2)]
    public string ImageEndpoint { get; } = ImageEndpoint;
    /// <summary>
    /// 8-digit unique id
    /// </summary>
    [JsonProperty(Order = 3)]
    public int Passcode { get; } = Passcode;
    /// <summary>
    /// Every <see cref="Rarity"/> of this <see cref="ACard"/>
    /// </summary>
    [JsonProperty(Order = 11)]
    public IEnumerable<Rarity> Rarities { get; } = Rarities;
    /// <summary>
    /// Every <see cref="Status"/> of this <see cref="ACard"/>
    /// </summary>
    [JsonProperty(Order = 12)]
    public IEnumerable<Status> Statuses { get; } = Statuses;
    #endregion

    #region Methods
    internal virtual void Init() { }
    
    /// <summary>
    /// Gets the localized <see cref="Cards.Localized.Name"/> depending on the currently set <see cref="MainWindow.Language"/> <br/>
    /// <i>If the name for the currently set <see cref="MainWindow.Language"/> is <see cref="string"/>.<see cref="string.Empty"/>, the <see cref="Language.en"/> name will be returned</i>
    /// </summary>
    /// <returns>The localized <see cref="Cards.Localized.Name"/> depending on the currently set <see cref="MainWindow.Language"/></returns>
    internal string GetName()
    {
        var name = this.Localized.Name[MainWindow.Language];

        if (name.IsNullEmptyOrWhitespace())
        {
            name = this.Localized.Name[Language.en];
        }

        return name;
    }

    /// <summary>
    /// Gets the localized <see cref="Cards.Localized.Description"/> depending on the currently set <see cref="MainWindow.Language"/> <br/>
    /// <i>If the description for the currently set <see cref="MainWindow.Language"/> is <see cref="string"/>.<see cref="string.Empty"/>, the <see cref="Language.en"/> description will be returned</i>
    /// </summary>
    /// <returns>The localized <see cref="Cards.Localized.Description"/> depending on the currently set <see cref="MainWindow.Language"/></returns>
    internal string GetDescription()
    {
        var description = this.Localized.Description[MainWindow.Language];

        if (description.IsNullEmptyOrWhitespace())
        {
            description = this.Localized.Description[Language.en];
        }

        return description;
    }
    
    /// <summary>
    /// Gets the <see cref="Monster.monsterCardType"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.monsterCardType"/> of a <see cref="Monster"/> card</returns>
    internal virtual MonsterType GetMonsterType() => MonsterType.MISSING;
    
    /// <summary>
    /// Gets the <see cref="Monster.ability"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.ability"/> of a <see cref="Monster"/> card</returns>
    internal virtual MonsterType GetAbilityType() => MonsterType.MISSING;
    
    /// <summary>
    /// Gets the <see cref="Monster.type"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.type"/> of a <see cref="Monster"/> card</returns>
    internal new virtual MonsterType GetType() => MonsterType.MISSING;
    
    /// <summary>
    /// Indicates whether this <see cref="ACard"/> belongs in the extra deck or not
    /// </summary>
    /// <returns>True, this <see cref="ACard"/> belongs in the extra deck, False, it belongs in the normal deck</returns>
    internal virtual bool IsExtraDeckCard() => false;
    
    /// <summary>
    /// Gets the <see cref="Spell.PropertyType"/> of a <see cref="Spell"/>/<see cref="Trap"/> card
    /// </summary>
    /// <returns>The <see cref="Spell.PropertyType"/> of a <see cref="Spell"/>/<see cref="Trap"/> card</returns>
    internal virtual PropertyType GetPropertyType() => PropertyType.MISSING;

    /// <summary>
    /// Gets the <see cref="Monster.Level"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.Level"/> of a <see cref="Monster"/> card</returns>
    internal virtual int GetLevel() => -1;
    
    /// <summary>
    /// Gets the <see cref="Monster.Attribute"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.Attribute"/> of a <see cref="Monster"/> card</returns>
    internal virtual Attribute GetAttribute() => Attribute.MISSING;

    /// <summary>
    /// Gets the <see cref="Monster.PendulumScale"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.PendulumScale"/> of a <see cref="Monster"/> card</returns>
    internal virtual int GetPendulumScale() => -1;

    /// <summary>
    /// Gets the <see cref="Monster.LinkArrows"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.LinkArrows"/> of a <see cref="Monster"/> card</returns>
    internal virtual IEnumerable<LinkArrow> GetLinkArrows() => Array.Empty<LinkArrow>();

    /// <summary>
    /// Gets the <see cref="Monster.Attack"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.Attack"/> of a <see cref="Monster"/> card</returns>
    internal virtual int GetAttack() => -1;

    /// <summary>
    /// Gets the <see cref="Monster.Defense"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <returns>The <see cref="Monster.Defense"/> of a <see cref="Monster"/> card</returns>
    internal virtual int GetDefense() => -1;
    #endregion
}