using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.Utility.Json;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.WPF;
using YuGiOh_DeckBuilder.WPF.Utility;
using YuGiOh_DeckBuilder.YuGiOh;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using Attribute = YuGiOh_DeckBuilder.YuGiOh.Enums.Attribute;

namespace YuGiOh_DeckBuilder;

public partial class MainWindow
{
    #region Methods
    /// <summary>
    /// Custom initialization
    /// </summary>
    private void Init()
    {
        Instance = this;

        this.settingsWindow = new FilterSettingsWindow(filterSettings);
        
        this.SearchCommand = new Command(this.Search);
        
        this.TextBox_Search.DataContext = this;
        this.ComboBox_Sort.DataContext = this;
        this.ListView_Cards.DataContext = this;
        this.ListView_Deck.DataContext = this;
        this.ListView_ExtraDeck.DataContext = this;
        this.Image_Selected.DataContext = this;

        this.Image_Selected.Source = CardImage.CardBack;
    }
    
    /// <summary>
    /// Loads all <see cref="Folder.Packs"/>
    /// </summary>
    private async Task LoadPacks()
    {
        Console.WriteLine("Loading Packs");
        
        var concurrentBag = new ConcurrentBag<Pack>();
        var directoryPath = Structure.BuildPath(Folder.Packs);
        var filesPaths = Directory.GetFiles(directoryPath);

        await Parallel.ForEachAsync(filesPaths, async (filePath, token) => // TODO: Use cancellation token
        {
            var pack = await Json.DeserializeAsync<Pack>(filePath);

            if (pack != null)
            {
                concurrentBag.Add(pack);
            }
        });

        Packs = new ReadOnlyCollection<Pack>(concurrentBag.ToList());
    }
    
    /// <summary>
    /// Loads all <see cref="Folder.Cards"/>
    /// </summary>
    private async Task LoadCards()
    {
        Console.WriteLine("Loading Cards");
        
        var concurrentBag = new ConcurrentBag<ACard>();
        var directoryPath = Structure.BuildPath(Folder.Cards);
        var filesPaths = Directory.GetFiles(directoryPath);

        await Parallel.ForEachAsync(filesPaths, async (filePath, token) => // TODO: Use cancellation token
        {
            var card = await Json.DeserializeAsync<ACard>(filePath);

            if (card != null)
            {
                if (await Json.DeserializeAsync<Localized>(Folder.Localization, card.Passcode.ToString()) is { } localized)
                {
                    card.Localized = localized;
                }
                
                card.Init();
                concurrentBag.Add(card);
            }
        });
        
        SetCards(concurrentBag.ToList());
    }

    /// <summary>
    /// Populates <see cref="Cards"/> with the given cards
    /// </summary>
    /// <param name="cards">The cards to populate <see cref="Cards"/> with</param>
    internal static void SetCards(List<ACard> cards)
    {
        var card = new Monster // Will be displayed, when there's an error with the actual card
        (
            Localized: Localized.Error, 
            Endpoint: nameof(Localized.Error),
            ImageEndpoint: nameof(Localized.Error),
            Passcode: 0,
            Rarities: Array.Empty<Rarity>(),
            Statuses: Array.Empty<Status>(),
            Level: -1,
            Attribute: Attribute.MISSING,
            LinkArrows: Array.Empty<LinkArrow>(),
            PendulumScale: -1,
            MonsterTypes: Array.Empty<MonsterType>(),
            Attack: int.MaxValue,
            Defense: int.MaxValue
        );
        
        cards.Insert(0, card); // Error card must always be the first card (index 0)
        
        Cards = new ReadOnlyCollection<ACard>(cards);
    }
    
    /// <summary>
    /// Sets the necessary index references for all <see cref="Cards"/>
    /// </summary>
    private void IndexCards()
    {
        var backgroundWorker = new BackgroundWorker();
            
        backgroundWorker.DoWork += IndexCards;
        backgroundWorker.RunWorkerAsync();
    }
    
    /// <summary>
    /// <b>Use <see cref="IndexCards()"/></b>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="doWorkEventArgs"><see cref="DoWorkEventArgs"/></param>
    private void IndexCards(object? sender, DoWorkEventArgs doWorkEventArgs)
    {
        var cardImages = new List<CardImage>();
        var packCards = Packs!.SelectMany(pack => pack.Cards).ToList();
        
        for (var i = 0; i < Cards!.Count; i++) // Cards!.Count
        {
            cardImages.Add(this.CreateCardImages(i));
            
            this.SetIndexInPacks(packCards, i);
            
            Console.WriteLine(i);
        }

        CardImages = new ReadOnlyCollection<CardImage>(cardImages);
        
        Console.WriteLine("\nFinished Loading\n");
    }

    /// <summary>
    /// Creates a <see cref="CardImage"/> for every <see cref="ACard"/> in <see cref="Cards"/> and adds it to <see cref="CardImages"/>
    /// </summary>
    /// <param name="index">Index of a <see cref="ACard"/> in <see cref="Cards"/>, the <see cref="CardImage"/> will be displaying</param>
    /// <returns>The created <see cref="CardImage"/></returns>
    private CardImage CreateCardImages(int index)
    {
        return this.Dispatcher.Invoke(() => new CardImage(index));
    }

    /// <summary>
    /// Sets the <see cref="Card.Index"/> of a <see cref="Card"/> in every <see cref="Pack"/>
    /// </summary>
    /// <param name="packCards">Every <see cref="Card"/> of every <see cref="Pack"/></param>
    /// <param name="index">The <see cref="Card.Index"/>, to set the <see cref="Card"/> to</param>
    private void SetIndexInPacks(List<Card> packCards, int index)
    {
        foreach (var card in packCards)
        {
            if (card.Passcode == Cards![index].Passcode)
            {
                card.Index = index;
            }
        }
    }
    
    /// <summary>
    /// Searches all cards according to <see cref="filterSettings"/>
    /// </summary>
    /// <param name="searchString">A <see cref="string"/> to search for in the <see cref="Localized.Name"/> and/or <see cref="Localized.Description"/> of a <see cref="ACard"/></param>
    private void Search(string? searchString)
    {
        // TODO: Remove, once cards are loaded on startup
        if (CardImages == null)
        {
            return;
        }
        
        var search = this.SearchPacks();
        search = this.SearchCardTypes(search);
        search = this.SearchMonsterTypes(search);
        search = this.SearchPropertyTypes(search);
        search = this.SearchLevels(search);
        search = this.SearchAttributes(search);
        search = this.SearchAbilities(search);
        search = this.SearchTypes(search);
        search = this.SearchPendulumScales(search);
        search = this.SearchLinkArrows(search);
        search = this.SearchRarities(search);
        search = this.SearchStatuses(search);
        search = this.SearchString(search, searchString);
        
        this.CardsListView = this.SortCards(search, (Sorting)this.ComboBox_Sort.SelectedValue, this.currentSortingOrder);
    }

    /// <summary>
    /// Searches every <see cref="Pack"/> according to the set <see cref="FilterSettings"/>
    /// </summary>
    /// <returns>Every <see cref="CardImage"/> from the selected packs</returns>
    private List<CardImage> SearchPacks()
    {
        // ReSharper disable once LocalVariableHidesMember
        var packs = filterSettings.Packs.Where(pack => pack.IsChecked!.Value).ToList();
        
        if (packs.Any())
        {
            var cards = Packs!.AsParallel().Where((_, index) => packs.Any(pack => pack.Index == index)).SelectMany(pack => pack.Cards.Select(card =>
            {
                return card.Index != 0 ? CardImages![card.Index] : new CardImage(card.Index); // TODO Use image object pool for error cards (index 0)

            })).Distinct().ToList(); // TODO: Maybe there's a different way to prevent duplicate cards (is contained in multiple selected packs), then "Distinct()"

            return new List<CardImage>(cards);;
        }
        
        return new List<CardImage>(CardImages!.Skip(1)); // Skips the first card (error card)
    }

    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="ACard.CardType"/> matches the selected <see cref="FilterSettings.CardTypes"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="ACard.CardType"/> matches the selected <see cref="FilterSettings.CardTypes"/> in <see cref="filterSettings"/></returns>
    private List<CardImage> SearchCardTypes(List<CardImage> cardImages)
    {
        var cardTypes = filterSettings.CardTypes.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        if (cardTypes.Any())
        {
            return cardImages.AsParallel().Where(cardImage => cardTypes.Contains(cardImage.CardData.CardType)).ToList();
        }

        return cardImages;
    }

    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="Monster.MonsterTypes"/> match the selected <see cref="FilterSettings.MonsterCardTypes"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.MonsterTypes"/> match the selected <see cref="FilterSettings.MonsterCardTypes"/> in <see cref="filterSettings"/></returns>
    private List<CardImage> SearchMonsterTypes(List<CardImage> cardImages)
    {
        var monsterTypes = filterSettings.MonsterCardTypes.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();

        if (monsterTypes.Any())
        {
            return cardImages.AsParallel().Where(cardImage => monsterTypes.Any(monsterType => monsterType == cardImage.CardData.GetMonsterType())).ToList();
        }

        return cardImages;
    }

    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="Spell.PropertyType"/> matches the selected <see cref="FilterSettings.PropertyTypes"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Spell.PropertyType"/> matches the selected <see cref="FilterSettings.PropertyTypes"/> in <see cref="filterSettings"/></returns>
    private List<CardImage> SearchPropertyTypes(List<CardImage> cardImages)
    {
        var propertyTypes = filterSettings.PropertyTypes.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        if (propertyTypes.Any())
        {
            return cardImages.AsParallel().Where(cardImage => propertyTypes.Contains(cardImage.CardData.GetPropertyType())).ToList();
        }

        return cardImages;
    }

    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="Monster.Level"/> matches the selected <see cref="FilterSettings.Levels"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.Level"/> matches the selected <see cref="FilterSettings.Levels"/> in <see cref="filterSettings"/></returns>
    /// <returns></returns>
    private List<CardImage> SearchLevels(List<CardImage> cardImages)
    {
        var levels = filterSettings.Levels.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        if (levels.Any())
        {
            return cardImages.AsParallel().Where(cardImage => levels.Contains(cardImage.CardData.GetLevel())).ToList();
        }

        return cardImages;
    }

    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="Monster.Attribute"/> matches the selected <see cref="FilterSettings.Attributes"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.Attribute"/> matches the selected <see cref="FilterSettings.Attributes"/> in <see cref="filterSettings"/></returns>
    /// <returns></returns>
    private List<CardImage> SearchAttributes(List<CardImage> cardImages)
    {
        var attributes = filterSettings.Attributes.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        if (attributes.Any())
        {
            return cardImages.AsParallel().Where(cardImage => attributes.Contains(cardImage.CardData.GetAttribute())).ToList();
        }

        return cardImages;
    }
    
    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="Monster.MonsterTypes"/> match the selected <see cref="FilterSettings.Abilities"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.MonsterTypes"/> match the selected <see cref="FilterSettings.Abilities"/> in <see cref="filterSettings"/></returns>
    private List<CardImage> SearchAbilities(List<CardImage> cardImages)
    {
        var abilities = filterSettings.Abilities.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();

        if (abilities.Any())
        {
            return cardImages.AsParallel().Where(cardImage => abilities.Any(ability => ability == cardImage.CardData.GetAbilityType())).ToList();
        }

        return cardImages;
    }
    
    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="Monster.MonsterTypes"/> match the selected <see cref="FilterSettings.Types"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.MonsterTypes"/> match the selected <see cref="FilterSettings.Types"/> in <see cref="filterSettings"/></returns>
    private List<CardImage> SearchTypes(List<CardImage> cardImages)
    {
        var types = filterSettings.Types.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();

        if (types.Any())
        {
            return cardImages.AsParallel().Where(cardImage => types.Any(type => type == cardImage.CardData.GetType())).ToList();
        }

        return cardImages;
    }
    
    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="Monster.PendulumScale"/> matches the selected <see cref="FilterSettings.PendulumScales"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.PendulumScale"/> matches the selected <see cref="FilterSettings.PendulumScales"/> in <see cref="filterSettings"/></returns>
    /// <returns></returns>
    private List<CardImage> SearchPendulumScales(List<CardImage> cardImages)
    {
        var levels = filterSettings.PendulumScales.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        if (levels.Any())
        {
            return cardImages.AsParallel().Where(cardImage => levels.Contains(cardImage.CardData.GetPendulumScale())).ToList();
        }

        return cardImages;
    }
    
    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="Monster.LinkArrows"/> matches the selected <see cref="FilterSettings.LinkArrows"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.LinkArrows"/> matches the selected <see cref="FilterSettings.LinkArrows"/> in <see cref="filterSettings"/></returns>
    /// <returns></returns>
    private List<CardImage> SearchLinkArrows(List<CardImage> cardImages)
    {
        var linkArrows = filterSettings.LinkArrows.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        if (linkArrows.Any())
        {
            return cardImages.AsParallel().Where(cardImage => linkArrows.Any(cardImage.CardData.GetLinkArrows().Contains)).ToList();
        }

        return cardImages;
    }
    
    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="ACard.Rarities"/> matches the selected <see cref="FilterSettings.Rarities"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.Rarities"/> matches the selected <see cref="FilterSettings.Rarities"/> in <see cref="filterSettings"/></returns>
    /// <returns></returns>
    private List<CardImage> SearchRarities(List<CardImage> cardImages)
    {
        var rarities = filterSettings.Rarities.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        if (rarities.Any())
        {
            return cardImages.AsParallel().Where(cardImage => rarities.Any(cardImage.CardData.Rarities.Contains)).ToList();
        }

        return cardImages;
    }
    
    /// <summary>
    /// Searches any <see cref="CardImage"/> whose <see cref="ACard.Statuses"/> matches the selected <see cref="FilterSettings.Statuses"/> in <see cref="filterSettings"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <returns>Every card in the given cardImages, whose <see cref="Monster.Statuses"/> matches the selected <see cref="FilterSettings.Statuses"/> in <see cref="filterSettings"/></returns>
    /// <returns></returns>
    private List<CardImage> SearchStatuses(List<CardImage> cardImages)
    {
        var statuses = filterSettings.Statuses.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
        
        if (statuses.Any())
        {
            return cardImages.AsParallel().Where(cardImage => statuses.Any(cardImage.CardData.Statuses.Contains)).ToList();
        }

        return cardImages;
    }
    
    /// <summary>
    /// Searches any <see cref="CardImage"/> that contains the given searchString in its <see cref="Localized.Name"/> ot <see cref="Localized.Description"/>
    /// </summary>
    /// <param name="cardImages">The cards to search in</param>
    /// <param name="searchString">The string to search for</param>
    /// <returns>Every card in the given cardImages, that contain the given searchString</returns>
    private List<CardImage> SearchString(List<CardImage> cardImages, string? searchString)
    {
        if (!searchString.IsNullEmptyOrWhitespace())
        {
            return cardImages.AsParallel().Where(cardImage =>
            {
                if (cardImage.CardData.GetName().Contains(searchString!, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (cardImage.CardData.GetDescription().Contains(searchString!, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            
            }).ToList();    
        }
        
        return cardImages;
    }

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="cardImages"></param>
    /// <param name="sorting"></param>
    /// <param name="sortingOrder"></param>
    /// <returns></returns>
    private List<CardImage> SortCards(List<CardImage> cardImages, Sorting sorting, bool sortingOrder)
    {
        if (sortingOrder)
        {
            switch (sorting)
            {
                case YuGiOh.Enums.Sorting.Level:
                    return cardImages.OrderBy(card => card.CardData.GetLevel()).ToList();
                
                case YuGiOh.Enums.Sorting.Attack:
                    return cardImages.OrderBy(card => card.CardData.GetAttack()).ToList();
                
                case YuGiOh.Enums.Sorting.Defense:
                    return cardImages.OrderBy(card => card.CardData.GetDefense()).ToList();
            }
        }
        else
        {
            switch (sorting)
            {
                case YuGiOh.Enums.Sorting.Level:
                    return cardImages.OrderByDescending(card => card.CardData.GetLevel()).ToList();
                
                case YuGiOh.Enums.Sorting.Attack:
                    return cardImages.OrderByDescending(card => card.CardData.GetAttack()).ToList();
                
                case YuGiOh.Enums.Sorting.Defense:
                    return cardImages.OrderByDescending(card => card.CardData.GetDefense()).ToList();
            }
        }

        return cardImages;
    }

    /// <summary>
    /// Displays the data for the given <see cref="CardImage"/> as the currently selected card 
    /// </summary>
    /// <param name="cardImage"><see cref="CardImage"/> to display the data of</param>
    internal void SelectCard(CardImage cardImage)
    {
        this.Image_Selected.Source = cardImage.Source;
        this.TextBox_Name.Text = Cards![cardImage.Index].GetName();
        this.TextBox_Passcode.Text = Cards![cardImage.Index].Passcode.ToString();
        this.TextBox_Description.Text = Cards![cardImage.Index].GetDescription();
    }
    
    /// <summary>
    /// Adds the given <see cref="CardImage"/> to <see cref="DeckListView"/>
    /// </summary>
    /// <param name="cardImage">The <see cref="CardImage"/> to add to <see cref="DeckListView"/></param>
    internal void AddCardToDeck(CardImage cardImage)
    {
        // TODO: Sort cards by currently set sorting
        
        if (cardImage.CardData.IsExtraDeckCard())
        {
            AddOrInsert(this.ExtraDeckListView, cardImage);
            this.OnPropertyChanged(nameof(this.ExtraDeckListView));
        }
        else
        {
            AddOrInsert(this.DeckListView, cardImage);
            this.OnPropertyChanged(nameof(this.DeckListView));
        }
    }

    /// <summary>
    /// Adds or inserts the given <see cref="CardImage"/> into the given <see cref="List{T}"/>
    /// </summary>
    /// <param name="listView">The <see cref="List{T}"/> to add the <see cref="CardImage"/> to</param>
    /// <param name="cardImage">The <see cref="CardImage"/> to add to the <see cref="List{T}"/></param>
    private static void AddOrInsert(List<CardImage> listView, CardImage cardImage)
    {
        var deckIndex = listView.FindIndex(deckCardImage => deckCardImage.Index == cardImage.Index);
        
        if (deckIndex != -1)
        {
            listView.Insert(deckIndex, cardImage);
        }
        else
        {
            listView.Add(cardImage);
        }
    }
    
    /// <summary>
    /// Removes the given <see cref="CardImage"/> from <see cref="DeckListView"/>
    /// </summary>
    /// <param name="cardImage">The <see cref="CardImage"/> to remove from <see cref="DeckListView"/></param>
    internal void RemoveCardFromDeck(CardImage cardImage)
    {
        if (cardImage.CardData.IsExtraDeckCard())
        {
            Remove(this.ExtraDeckListView, cardImage);
            this.OnPropertyChanged(nameof(this.ExtraDeckListView));
        }
        else
        {
            Remove(this.DeckListView, cardImage);
            this.OnPropertyChanged(nameof(this.DeckListView));
        }
    }

    /// <summary>
    /// Removes the given <see cref="CardImage"/> from the given <see cref="List{T}"/>
    /// </summary>
    /// <param name="listView">The <see cref="List{T}"/> to remove the <see cref="CardImage"/> from</param>
    /// <param name="cardImage">The <see cref="CardImage"/> to remove from the <see cref="List{T}"/></param>
    private static void Remove(List<CardImage> listView, CardImage cardImage)
    {
        var deckIndex = listView.FindIndex(deckCardImage => deckCardImage.Index == cardImage.Index);
        
        if (deckIndex != -1)
        {
            listView.RemoveAt(deckIndex);
        }
    }
    #endregion
}