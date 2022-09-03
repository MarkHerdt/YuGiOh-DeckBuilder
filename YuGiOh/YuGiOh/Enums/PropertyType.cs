namespace YuGiOh_DeckBuilder.YuGiOh.Enums;

/// <summary>
/// All possible spell/trap card types <br/>
/// https://yugioh.fandom.com/wiki/Spell_Card?so=search <br/>
/// https://yugioh.fandom.com/wiki/Trap_Card?so=search
/// </summary>
internal enum PropertyType
{
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Normal_Spell_Card <br/>
    /// https://yugioh.fandom.com/wiki/Normal_Trap_Card
    /// </summary>
    Normal = 0,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Continuous_Spell_Card <br/>
    /// https://yugioh.fandom.com/wiki/Continuous_Trap_Card
    /// </summary>
    Continuous = 1,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Counter_Trap_Card
    /// </summary>
    Counter = 2,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Equip_Spell_Card
    /// </summary>
    Equip = 3,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Field_Spell_Card
    /// </summary>
    Field = 4,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Quick-Play_Spell_Card
    /// </summary>
    QuickPlay = 5,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Ritual_Spell_Card
    /// </summary>
    Ritual = 6,
    /// <summary>
    /// Default value, when a <see cref="PropertyType"/> couldn't be found
    /// </summary>
    MISSING = -1,
}