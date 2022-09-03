using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.Utility.Json;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.Web;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using YuGiOh_DeckBuilder.YuGiOh.Utility;
using Attribute = YuGiOh_DeckBuilder.YuGiOh.Enums.Attribute;

namespace YuGiOh_DeckBuilder.YuGiOh;

/// <summary>
/// https://yugioh.fandom.com/wiki/
/// </summary>
internal sealed partial class YuGiOhFandom
{
    #region Methods
    /// <summary>
    /// Serializes the <see cref="MainWindow.Packs"/> to .json files
    /// </summary>
    private async Task SerializePacksAsync()
    {
        var packs = new ConcurrentBag<Pack>();
        var currentPack = 0;
        var packCount = this.PackData.Count;

        await Parallel.ForEachAsync(this.PackData, async (packData, token) => // TODO: Use cancellation token
        {
            var pack = new Pack(packData.PackName ?? "MISSING", packData.ReleaseDate ?? default, packData.PackEndpoint, packData.Cards.ToList());
            
            await Json.SerializeAsync(this.packsFolder, packData.PackEndpoint, pack);
            
            packs.Add(pack);
            
            Output.Print("Serializing pack:", ++currentPack, packCount, packData.PackEndpoint, MainWindow.TestOutputHelper);
        });

        MainWindow.Packs = new ReadOnlyCollection<Pack>(packs.ToList());
    }
    
    /// <summary>
    /// Build every <see cref="ACard"/> from <see cref="CardData"/>
    /// </summary>
    private async Task SerializeCardsAsync()
    {
        var currentCard = 0;
        var cardCount = this.CardData.Count;
        
        await Parallel.ForEachAsync(this.CardData, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, async (data, token) => // TODO: Use cancellation token
        {
            if (data.CardType! == typeof(Monster))
            {
                await BuildMonsterCard(data);
            }
            else if (data.CardType! == typeof(Spell))
            {
                await BuildSpellCard(data);
            }
            else if (data.CardType! == typeof(Trap))
            {
                await BuildTrapCard(data);
            }
            
            Output.Print("Serializing Card:", ++currentCard, cardCount, data.CardNames[Language.en], MainWindow.TestOutputHelper);
        });

        MainWindow.SetCards(this.CardData.Select(cardData => cardData.Card).ToList()!);
    }
    
    /// <summary>
    /// Downloads an <see cref="Image"/> for every card in <see cref="CardData"/> in parallel
    /// </summary>
    private async Task DownloadImagesAsync()
    {
        // TODO: Try to download images again that couldn't be downloaded the first time
        // Http Request Error | https://static.wikia.nocookie.net/yugioh/images/d/d0/TestTiger-RYMP-EN-C-1E.png | Response status code does not indicate success: 503 (Backend fetch failed).
        
        var currentCard = 0;
        var cardCount = this.CardData.Count;
        
        await Parallel.ForEachAsync(this.CardData, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, async (card, token) => // TODO: Use cancellation token
        {
            if (!card.CardImageEndpoint.IsNullEmptyOrWhitespace())
            {
                var image = await WebClient.DownloadImage(ImageBaseUri + card.CardImageEndpoint, ImageCodecInfo, EncoderParameters);

                if (image != null)
                {
                    var filePath = Structure.BuildPath(this.imagesFolder, card.Passcode.ToString()!, Extension.jpg);

                    await using var fileStream = File.Open(filePath, FileMode.OpenOrCreate);
                    
                    image.Object.Save(fileStream, ImageCodecInfo, EncoderParameters);
                }
            }
            
            Output.Print("Downloading Image:", ++currentCard, cardCount, card.CardImageEndpoint ?? "MISSING", MainWindow.TestOutputHelper);
        });
    }
    
    /// <summary>
    /// Builds a <see cref="Monster"/> card
    /// </summary>
    /// <param name="data"><see cref="Decks.Cards.CardData"/> to create the <see cref="Monster"/> from</param>
    private async Task BuildMonsterCard(CardData data)
    {
        var localized = await this.SerializeLocalizations(data);
        
        data.Card = new Monster
        (
            Localized: localized,
            Endpoint: data.CardEndpoint,
            ImageEndpoint: data.CardImageEndpoint ?? "MISSING",
            Passcode: data.Passcode!.Value,
            Rarities: data.Rarities,
            Statuses: data.Statuses,
            Level: data.MonsterLevel ?? -1,
            Attribute: data.Attribute ?? Attribute.MISSING,
            LinkArrows: data.LinkArrows,
            PendulumScale: data.PendulumScale ?? -1,
            MonsterTypes: data.MonsterTypes,
            Attack: data.Attack ?? int.MaxValue,
            Defense: data.Defense ?? int.MaxValue
        );
            
        await Json.SerializeAsync(this.cardsFolder, data.Passcode.ToString()!, data.Card);
    }

    /// <summary>
    /// Builds a <see cref="Spell"/> card
    /// </summary>
    /// <param name="data"><see cref="Decks.Cards.CardData"/> to create the <see cref="Spell"/> from</param>
    private async Task BuildSpellCard(CardData data)
    {
        var localized = await this.SerializeLocalizations(data);
        
        data.Card = new Spell
        (
            Localized: localized,
            Endpoint: data.CardEndpoint,
            ImageEndpoint: data.CardImageEndpoint ?? "MISSING",
            Passcode: data.Passcode!.Value,
            Rarities: data.Rarities,
            Statuses: data.Statuses,
            PropertyType: data.PropertyType ?? PropertyType.MISSING
        );
        
        await Json.SerializeAsync(this.cardsFolder, data.Passcode.ToString()!, data.Card);
    }

    /// <summary>
    /// Builds a <see cref="Trap"/> card
    /// </summary>
    /// <param name="data"><see cref="Decks.Cards.CardData"/> to create the <see cref="Trap"/> from</param>
    private async Task BuildTrapCard(CardData data)
    {
        var localized = await this.SerializeLocalizations(data);

        data.Card = new Trap
        (
            Localized: localized,
            Endpoint: data.CardEndpoint,
            ImageEndpoint: data.CardImageEndpoint ?? "MISSING",
            Passcode: data.Passcode!.Value,
            Rarities: data.Rarities,
            Statuses: data.Statuses,
            PropertyType: data.PropertyType ?? PropertyType.MISSING
        );
        
        await Json.SerializeAsync(this.cardsFolder, data.Passcode.ToString()!, data.Card);
    }

    /// <summary>
    /// Gets the localized names and descriptions from the given <see cref="CardData"/> and serializes them
    /// </summary>
    /// <param name="data"><see cref="CardData"/> to get the localizations from</param>
    /// <returns>The localizations from the given <see cref="CardData"/></returns>
    private async Task<Localized> SerializeLocalizations(CardData data)
    {
        var localized = new Localized();
        
        foreach (var (language, name) in data.CardNames)
        {
            localized.Name[language] = name;
        }

        foreach (var (language, description) in data.Descriptions)
        {
            localized.Description[language] = description;
        }

        await Json.SerializeAsync(this.localizationFolder, data.Passcode.ToString()!, localized);

        return localized;
    }
    #endregion
}