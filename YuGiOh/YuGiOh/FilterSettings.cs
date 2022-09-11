using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using YuGiOh_DeckBuilder.WPF;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using Attribute = YuGiOh_DeckBuilder.YuGiOh.Enums.Attribute;

// ReSharper disable UnusedMember.Global

namespace YuGiOh_DeckBuilder.YuGiOh;

/// <summary>
/// Contains all filter, that should be applied on the next search
/// </summary>
internal record FilterSettings : INotifyPropertyChanged
{
    #region Properties
    /// <summary>
    /// Contains all <see cref="MainWindow.Packs"/>
    /// </summary>
    public List<PackToggleButton> Packs { get; } = new();
    /// <summary>
    /// <see cref="DateTime"/> for <see cref="FilterSettingsWindow.ComboBox_DateStart"/>
    /// </summary>
    public DateTime DateStart { get; set; }
    /// <summary>
    /// <see cref="DateTime"/> for <see cref="FilterSettingsWindow.ComboBox_DateEnd"/>
    /// </summary>
    public DateTime DateEnd { get; set; }
    
    /// <summary>
    /// Determines what <see cref="CardType"/> to search for
    /// </summary>
    public Dictionary<CardType, bool> CardTypes { get; } = Enum.GetValues<CardType>().ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines if the search should include/exclude <see cref="MonsterType.Normal"/>/<see cref="MonsterType.Effect"/> <see cref="Monster"/>
    /// </summary>
    public Dictionary<MonsterType, bool> NormalOrEffectMonster { get; } = Monster.NormalOrEffectMonster.ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines what <see cref="MonsterType"/> to search for
    /// </summary>
    public Dictionary<MonsterType, bool> MonsterCardTypes { get; } = Monster.MonsterCardTypes.ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines what <see cref="PropertyType"/> to search for
    /// </summary>
    public Dictionary<PropertyType, bool> PropertyTypes { get; } = Enum.GetValues<PropertyType>().SkipWhile(propertyType => propertyType == PropertyType.MISSING).ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines what <see cref="Monster.Level"/> to search for
    /// </summary>
    public Dictionary<int, bool> Levels { get; } = new()
    {
        { 0,  false },
        { 1,  false },
        { 2,  false },
        { 3,  false },
        { 4,  false },
        { 5,  false },
        { 6,  false },
        { 7,  false },
        { 8,  false },
        { 9,  false },
        { 10, false },
        { 11, false },
        { 12, false },
        { 13, false }
    };
    /// <summary>
    /// Determines what <see cref="Attribute"/> to search for
    /// </summary>
    public Dictionary<Attribute, bool> Attributes { get; } = Enum.GetValues<Attribute>().SkipWhile(propertyType => propertyType == Attribute.MISSING).ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines what <see cref="MonsterType"/> to search for
    /// </summary>
    public Dictionary<MonsterType, bool> Abilities { get; } = Monster.Abilities.ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines what <see cref="MonsterType"/> to search for
    /// </summary>
    public Dictionary<MonsterType, bool> Types { get; } = Monster.Types.ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines what <see cref="Monster.PendulumScale"/> to search for
    /// </summary>
    public Dictionary<int, bool> PendulumScales { get; } = new()
    {
        { 0,  false },
        { 1,  false },
        { 2,  false },
        { 3,  false },
        { 4,  false },
        { 5,  false },
        { 6,  false },
        { 7,  false },
        { 8,  false },
        { 9,  false },
        { 10, false },
        { 11, false },
        { 12, false },
        { 13, false }
    };
    /// <summary>
    /// Determines what <see cref="LinkArrow"/> to search for
    /// </summary>
    public Dictionary<LinkArrow, bool> LinkArrows { get; } = Enum.GetValues<LinkArrow>().ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines what <see cref="Rarity"/> to search for
    /// </summary>
    public Dictionary<Rarity, bool> Rarities { get; } = Enum.GetValues<Rarity>().ToDictionary(key => key, _ => false);
    /// <summary>
    /// Determines what <see cref="Status"/> to search for
    /// </summary>
    public Dictionary<Status, bool> Statuses { get; } = Enum.GetValues<Status>().ToDictionary(key => key, _ => false);
    #endregion

    #region Events
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion
    
    #region Methods
    /// <summary>
    /// <see cref="PropertyChanged"/>
    /// </summary>
    /// <param name="propertyName">The name of the property that changed</param>
    [NotifyPropertyChangedInvocator]
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}