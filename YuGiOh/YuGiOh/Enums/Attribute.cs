namespace YuGiOh_DeckBuilder.YuGiOh.Enums;

/// <summary>
/// All possible attributes <br/>
/// https://yugioh.fandom.com/wiki/Attribute
/// </summary>
internal enum Attribute
{
    /// <summary>
    /// https://yugioh.fandom.com/wiki/DARK
    /// </summary>
    DARK = 1,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/DIVINE
    /// </summary>
    DIVINE = 2,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/EARTH
    /// </summary>
    EARTH = 3,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/FIRE
    /// </summary>
    FIRE = 4,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/LIGHT
    /// </summary>
    LIGHT = 5,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/WATER
    /// </summary>
    WATER = 6,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/WIND
    /// </summary>
    WIND = 7,
    /// <summary>
    /// Default value, when an <see cref="Attribute"/> couldn't be found
    /// </summary>
    MISSING = -1,
}