using System;
using System.Collections.Generic;
using System.Drawing;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using Attribute = YuGiOh_DeckBuilder.YuGiOh.Enums.Attribute;

namespace YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;

/// <summary>
/// Contains all data for a card to be created
/// </summary>
/// <param name="Endpoint">Endpoint for the card</param>
/// <param name="Rarities">Rarities for this card</param>
internal sealed record CardData(string Endpoint, IEnumerable<Rarity> Rarities)
{
    #region Properties
    /// <summary>
    /// Contains the finished card
    /// </summary>
    internal ACard? Card { get; set; }
    
    /// <summary>
    /// Endpoint for the card
    /// </summary>
    internal string Endpoint { get; } = Endpoint;

    /// <summary>
    /// Rarities for this card
    /// </summary>
    internal IEnumerable<Rarity> Rarities { get; } = Rarities;

    /// <summary>
    /// <see cref="Monster"/>/<see cref="Spell"/>/<see cref="Trap"/>
    /// </summary>
    internal Type? CardType { get; set; }

    /// <summary>
    /// <b>Key: </b> <see cref="Language"/> <br/>
    /// <b>Value: </b> Translated name for that <see cref="Language"/>
    /// </summary>
    internal Dictionary<Language, string> CardNames { get; } = new();
    
    /// <summary>
    /// <see cref="Enums.Attribute"/> of a <see cref="Monster"/> card
    /// </summary>
    internal Attribute? Attribute { get; set; }
    /// <summary>
    /// Indicates, if the row that hold the <see cref="Enums.Attribute"/> has been found
    /// </summary>
    internal bool AttributeFound { get; set; }

    /// <summary>
    /// <see cref="Enums.PropertyType"/> of a <see cref="Spell"/>/<see cref="Trap"/> card
    /// </summary>
    internal PropertyType? PropertyType { get; set; }
    
    /// <summary>
    /// Level/Rank (Stars) of a <see cref="Monster"/> card
    /// </summary>
    internal int? MonsterLevel { get; set; }
    
    /// <summary>
    /// Uri for the <see cref="Image"/> of a card
    /// </summary>
    internal string? ImageEndpoint { get; set; }
    
    /// <summary>
    /// Every <see cref="Enums.LinkArrow"/> of a link <see cref="Monster"/>
    /// </summary>
    internal List<LinkArrow> LinkArrows { get; } = new();
    
    /// <summary>
    /// Pendulum scale of a pendulum <see cref="Monster"/>
    /// </summary>
    internal int? PendulumScale { get; set; }
    
    /// <summary>
    /// Every <see cref="Enums.MonsterType"/> of a <see cref="Monster"/>
    /// </summary>
    internal List<MonsterType> MonsterTypes { get; } = new();
    /// <summary>
    /// Indicates, if the row that hold the monster types has been found
    /// </summary>
    internal bool MonsterTypeFound { get; set; }

    /// <summary>
    /// Indicates, if the description table has been found
    /// </summary>
    internal bool DescriptionFound { get; set; }
    /// <summary>
    /// Indicates that the next line will hold the description text
    /// </summary>
    internal bool NextIsDescription { get; set; }
    /// <summary>
    /// Indicates that all descriptions for the available <see cref="Language"/>s have been found
    /// </summary>
    internal bool AllDescriptionsFound { get; set; }
    /// <summary>
    /// <b>Key: </b> <see cref="Enums.Language"/> <br/>
    /// <b>Value: </b> Translated description for that <see cref="Enums.Language"/>
    /// </summary>
    internal Dictionary<Language, string> Descriptions { get; } = new();
    
    /// <summary>
    /// Attack value of a <see cref="Monster"/> card
    /// </summary>
    internal int? Attack { get; set; }
    /// <summary>
    /// Defense value of a <see cref="Monster"/> card
    /// </summary>
    internal int? Defense { get; set; }

    /// <summary>
    /// Indicates, the row containing the passcode, has been reached
    /// </summary>
    internal bool PasscodeReached { get; set; }
    /// <summary>
    /// Passcode of a card
    /// </summary>
    internal int? Passcode { get; set; }

    /// <summary>
    /// Every <see cref="Enums.Status"/> of a card
    /// </summary>
    internal List<Status> Statuses { get; } = new();

    /// <summary>
    /// Indicates, that the search for this card should be stopped
    /// </summary>
    internal bool StopSearch { get; set; }
    #endregion
}