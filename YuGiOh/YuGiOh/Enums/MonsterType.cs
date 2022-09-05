using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums;

/// <summary>
/// All possible monster types <br/>
/// https://yugioh.fandom.com/wiki/Type
/// </summary>
internal enum MonsterType
{
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Normal_Monster
    /// </summary>
    Normal = 0,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Aqua
    /// </summary>
    Aqua = 1,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Beast
    /// </summary>
    Beast = 2,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Beast-Warrior
    /// </summary>
    BeastWarrior = 3,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Creator_God
    /// </summary>
    CreatorGod = 4,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Cyberse
    /// </summary>
    Cyberse = 5,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Dinosaur
    /// </summary>
    Dinosaur = 6,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Divine-Beast
    /// </summary>
    DivineBeast = 7,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Dragon
    /// </summary>
    Dragon = 8,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Effect_Monster?so=search
    /// </summary>
    Effect = 9,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Fairy
    /// </summary>
    Fairy = 10,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Fiend
    /// </summary>
    Fiend = 11,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Fish
    /// </summary>
    Fish = 12,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Flip_monster?so=search
    /// </summary>
    Flip = 13,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Fusion_Monster?so=search
    /// </summary>
    Fusion = 14,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Gemini_monster?so=search
    /// </summary>
    Gemini = 15,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Insect
    /// </summary>
    Insect = 16,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Link_Monster?so=search
    /// </summary>
    Link = 17,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Machine
    /// </summary>
    Machine = 18,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Pendulum_Monster?so=search
    /// </summary>
    Pendulum = 19,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Plant
    /// </summary>
    Plant = 20,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Psychic
    /// </summary>
    Psychic = 21,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Pyro
    /// </summary>
    Pyro = 22,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Reptile
    /// </summary>
    Reptile = 23,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Ritual_Monster?so=search
    /// </summary>
    Ritual = 24,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Rock
    /// </summary>
    Rock = 25,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Sea_Serpent
    /// </summary>
    SeaSerpent = 26,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Spellcaster
    /// </summary>
    Spellcaster = 27,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Spirit_monster?so=search
    /// </summary>
    Spirit = 28,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Synchro_Monster?so=search
    /// </summary>
    Synchro = 29,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Thunder
    /// </summary>
    Thunder = 30,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Toon_monster?so=search
    /// </summary>
    Toon = 31,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Tuner_monster?so=search
    /// </summary>
    Tuner = 32,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Union_monster?so=search
    /// </summary>
    Union = 33,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Warrior
    /// </summary>
    Warrior = 34,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Winged_Beast
    /// </summary>
    WingedBeast = 35,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Wyrm
    /// </summary>
    Wyrm = 36,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Xyz_Monster?so=search
    /// </summary>
    Xyz = 37,
    /// <summary>
    /// https://yugioh.fandom.com/wiki/Zombie
    /// </summary>
    Zombie = 38,
    /// <summary>
    /// Default value, when a <see cref="Monster"/> doesn't have a specific <see cref="MonsterType"/> 
    /// </summary>
    MISSING = -1
}