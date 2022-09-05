using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit.Abstractions;
using YuGiOh_DeckBuilder.Utility.ObjectPools;
using YuGiOh_DeckBuilder.WPF;
using YuGiOh_DeckBuilder.YuGiOh;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;
using YuGiOh_DeckBuilder.YuGiOh.Enums;

namespace YuGiOh_DeckBuilder;

/// <summary>
/// Contains general data for <see cref="MainWindow"/>
/// </summary>
public partial class MainWindow : INotifyPropertyChanged
{
    #region Statics
    /// <summary>
    /// <see cref="ITestOutputHelper"/>
    /// </summary>
    internal static ITestOutputHelper? TestOutputHelper { get; set; }
    /// <summary>
    /// Singleton of <see cref="MainWindow"/>
    /// </summary>
#pragma warning disable CS8618
    internal static MainWindow Instance { get; private set; }
#pragma warning restore CS8618

    /// <summary>
    /// The language to get the cards in
    /// </summary>
    internal new static Language Language { get; private set; } = Language.en;
    /// <summary>
    /// Object pool for <see cref="StringBuilder"/>
    /// </summary>
    internal static StringBuilderPool StringBuilderPool { get; } = new();
    
    /// <summary>
    /// Contains every <see cref="Pack"/>
    /// </summary>
    private static ReadOnlyCollection<Pack>? packs;
    /// <summary>
    /// <see cref="packs"/>
    /// </summary>
    internal static ReadOnlyCollection<Pack>? Packs
    {
        get => packs;
        set
        {
            packs = new ReadOnlyCollection<Pack>(value!.Where(pack => pack.ReleaseDate != default).OrderBy(pack => pack.ReleaseDate).ToList());

            if (TestOutputHelper == null)
            {
                for (var i = 0; i < packs.Count; i++)
                {
                    filterSettings.Packs.Add(new PackToggleButton(i));
                }   
            }
        }
    }
    /// <summary>
    /// Contains every <see cref="ACard"/>
    /// </summary>
    internal static ReadOnlyCollection<ACard>? Cards { get; private set; }
    /// <summary>
    /// Contains 
    /// </summary>
    private static ReadOnlyCollection<CardImage>? CardImages { get; set; }
    #endregion

    #region Members
    /// <summary>
    /// <see cref="FilterSettingsWindow"/>
    /// </summary>
    private FilterSettingsWindow? settingsWindow;
    /// <summary>
    /// <see cref="FilterSettings"/>
    /// </summary>
    private static readonly FilterSettings filterSettings = new();

    /// <summary>
    /// The currently set sorting order for <see cref="CardsListView"/>
    /// </summary>
    private bool currentSortingOrder;
    /// <summary>
    /// Mapping for the sorting order
    /// </summary>
    private readonly Dictionary<bool, SortingOrder> sortingOrder = new()
    {
        { true, SortingOrder.Ascending },
        { false, SortingOrder.Descending }
    };

    /// <summary>
    /// Contains every <see cref="CardImage"/> that is currently shown in the <see cref="ListView_Cards"/>
    /// </summary>
    private List<CardImage> cardsListView = new();
    #endregion
    
    #region Properties
    /// <summary>
    /// Sorting for <see cref="CardsListView"/>
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once CollectionNeverQueried.Global
    public List<Sorting> Sorting { get; } = Enum.GetValues<Sorting>().ToList();

    /// <summary>
    /// <see cref="cardsListView"/>
    /// </summary>
    public List<CardImage> CardsListView
    {
        get => this.cardsListView;
        private set
        {
            this.cardsListView = value;
            this.TextBlock_CardCount.Text = this.cardsListView.Count.ToString();
            this.OnPropertyChanged(nameof(this.CardsListView));
        }
    }
    /// <summary>
    /// Contains every <see cref="CardImage"/> that is currently shown in <see cref="ListView_Deck"/>
    /// </summary>
    public List<CardImage> DeckListView { get; } = new();
    /// <summary>
    /// Contains every <see cref="CardImage"/> that is currently shown in <see cref="ListView_ExtraDeck"/>
    /// </summary>
    public List<CardImage> ExtraDeckListView { get; } = new();
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
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}