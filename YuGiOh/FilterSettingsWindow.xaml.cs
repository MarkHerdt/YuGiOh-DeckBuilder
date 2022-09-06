using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using YuGiOh_DeckBuilder.YuGiOh;

namespace YuGiOh_DeckBuilder;

/// <summary>
/// <see cref="Window"/> to ste all <see cref="FilterSettings"/> for the search
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class FilterSettingsWindow : Window
{
    #region Constants
    string test = DateTime.Now.ToString("yyyy MMMM dd");
    /// <summary>
    /// <see cref="string"/> formatting for every <see cref="DateTime"/> in <see cref="FilterSettingsWindow"/>
    /// </summary>
    internal const string DateTimeFormat = "yyyy MMMM dd";
    #endregion
    
    #region Members
    /// <summary>
    /// <see cref="FilterSettings"/>
    /// </summary>
    private readonly FilterSettings filterSettings;
    /// <summary>
    /// Is set after the <see cref="Window.OnActivated"/> has been called
    /// </summary>
    private bool isActivated;
    #endregion

    #region Properties
    /// <summary>
    /// Release dates of all <see cref="FilterSettings.Packs"/> in <see cref="filterSettings"/>
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public List<DateTime> ReleaseDates { get; } = new();
    /// <summary>
    /// Content for <see cref="ComboBox_DateStart"/> and <see cref="ComboBox_DateEnd"/> <br/>
    /// <i>Same order as <see cref="ReleaseDates"/></i>
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public List<string> ReleaseDateFormats { get; } = new();
    #endregion
    
    #region Events
    /// <summary>
    /// Is fired when the size of <see cref="ListView_Packs"/> changes <br/>
    /// <i>Parameter: New width of <see cref="ListView_Packs"/></i>
    /// </summary>
    internal static event Action<double>? OnSizeChanged;
    #endregion
    
    #region Constrcutor
    /// <param name="filterSettings"><see cref="FilterSettings"/></param>
    internal FilterSettingsWindow(FilterSettings filterSettings)
    {
        InitializeComponent();
        
        this.filterSettings = filterSettings;
        this.DataContext = filterSettings;
        
        this.ComboBox_DateStart.DataContext = this;
        this.ComboBox_DateEnd.DataContext = this;

        foreach (var pack in this.filterSettings.Packs) // TODO: Initialize once when packs are loaded
        {
            this.ReleaseDates.Add(pack.ReleaseDate);
            this.ReleaseDateFormats.Add(pack.ReleaseDate.ToString(DateTimeFormat));
        }
        
        this.ComboBox_DateStart.SelectedIndex = this.ReleaseDates.IndexOf(this.filterSettings.DateStart);
        this.ComboBox_DateEnd.SelectedIndex = this.ReleaseDates.IndexOf(this.filterSettings.DateEnd);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Clears all selections in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_Clear_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        this.filterSettings.Packs.ForEach(pack => pack.IsChecked = false);
        this.filterSettings.DateStart = default;
        this.filterSettings.DateEnd = default;
        ClearAll(this.filterSettings.CardTypes);
        ClearAll(this.filterSettings.MonsterCardTypes);
        ClearAll(this.filterSettings.PropertyTypes);
        ClearAll(this.filterSettings.Levels);
        ClearAll(this.filterSettings.Attributes);
        ClearAll(this.filterSettings.Abilities);
        ClearAll(this.filterSettings.Types);
        ClearAll(this.filterSettings.PendulumScales);
        ClearAll(this.filterSettings.LinkArrows);
        ClearAll(this.filterSettings.Rarities);
        ClearAll(this.filterSettings.Statuses);
        
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Packs));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.CardTypes));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.MonsterCardTypes));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.PropertyTypes));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Levels));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Attributes));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Abilities));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Types));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.PendulumScales));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.LinkArrows));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Rarities));
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Statuses));
    }

    /// <summary>
    /// Sets every <see cref="bool"/> in the given <see cref="IDictionary{TKey,TValue}"/> to false
    /// </summary>
    /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to set the <see cref="IDictionary{TKey,TValue}.Values"/> of</param>
    /// <typeparam name="T">Can be of any <see cref="Type"/></typeparam>
    private static void ClearAll<T>(IDictionary<T, bool> dictionary)
    {
        foreach (var (key, _) in dictionary)
        {
            dictionary[key] = false;
        }
    }
    
    /// <summary>
    /// Sets all <see cref="IDictionary{TKey,TValue}.Values"/> of the given <see cref="IDictionary{TKey,TValue}"/>
    /// </summary>
    /// <param name="dictionary">The <see cref="IDictionary{TKey,TValue}"/> to adjust the <see cref="bool"/> value in</param>
    /// <typeparam name="T">Can be of any <see cref="Type"/></typeparam>
    private static void SetAll<T>(IDictionary<T, bool> dictionary)
    {
        if (dictionary.Values.Any(value => value))
        {
            foreach (var (key, _) in dictionary)
            {
                dictionary[key] = false;
            }
        }
        else
        {
            foreach (var (key, _) in dictionary)
            {
                dictionary[key] = true;
            }
        }
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.CardTypes"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_CardType_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.CardTypes);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.CardTypes));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.MonsterCardTypes"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_MonsterType_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.MonsterCardTypes);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.MonsterCardTypes));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.PropertyTypes"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_SpellTrap_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.PropertyTypes);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.PropertyTypes));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.Levels"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_LevelRank_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.Levels);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Levels));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.Attributes"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_Attribute_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.Attributes);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Attributes));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.Abilities"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_Ability_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.Abilities);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Abilities));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.Types"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_Type_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.Types);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Types));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.PendulumScales"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_PendulumScale_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.PendulumScales);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.PendulumScales));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.LinkArrows"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_LinkArrow_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.LinkArrows);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.LinkArrows));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.Rarities"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_Rarity_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.Rarities);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Rarities));
    }
    
    /// <summary>
    /// Selects/Deselects all <see cref="FilterSettings.Statuses"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private void Button_Status_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        SetAll(this.filterSettings.Statuses);
        this.filterSettings.OnPropertyChanged(nameof(this.filterSettings.Statuses));
    }

    protected override void OnActivated(EventArgs eventArgs)
    {
        this.isActivated = true;
        base.OnActivated(eventArgs);
    }
    
    /// <summary>
    /// Is fired when the selection in <see cref="ComboBox_DateStart"/> changes
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="selectionChangedEventArgs"><see cref="SelectionChangedEventArgs"/></param>
    private void ComboBox_DateStart_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        if (!this.isActivated) return;
        
        var currentStartDate = this.ReleaseDates[this.ReleaseDateFormats.IndexOf((string)(sender as ComboBox)!.SelectedItem)];
        
        foreach (var packToggleButton in this.filterSettings.Packs)
        {
            if (packToggleButton.ReleaseDate >= this.filterSettings.DateStart && packToggleButton.ReleaseDate <= this.filterSettings.DateEnd)
            {
                packToggleButton.IsChecked = false;
            }
            if (packToggleButton.ReleaseDate >= currentStartDate && packToggleButton.ReleaseDate <= this.filterSettings.DateEnd)
            {
                packToggleButton.IsChecked = true;
            }
        }

        this.filterSettings.DateStart = currentStartDate;
    }
    
    /// <summary>
    /// Is fired when the selection in <see cref="ComboBox_DateEnd"/> changes
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="selectionChangedEventArgs"><see cref="SelectionChangedEventArgs"/></param>
    private void ComboBox_DateEnd_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
        if (!this.isActivated) return;
        
        var currentEndDate = this.ReleaseDates[this.ReleaseDateFormats.IndexOf((string)(sender as ComboBox)!.SelectedItem)];

        foreach (var packToggleButton in this.filterSettings.Packs)
        {
            if (packToggleButton.ReleaseDate >= this.filterSettings.DateStart && packToggleButton.ReleaseDate <= this.filterSettings.DateEnd)
            {
                packToggleButton.IsChecked = false;
            }
            if (packToggleButton.ReleaseDate >= this.filterSettings.DateStart && packToggleButton.ReleaseDate <= currentEndDate)
            {
                packToggleButton.IsChecked = true;
            }
        }
        
        this.filterSettings.DateEnd = currentEndDate;
    }
    
    /// <summary>
    /// Is fired, when the size of <see cref="ListView_Packs"/> changes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="sizeChangedEventArgs"></param>
    private void ListView_SizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
        OnSizeChanged?.Invoke(sizeChangedEventArgs.NewSize.Width);
    }
    #endregion
}