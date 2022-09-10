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
using YuGiOh_DeckBuilder.YuGiOh.Logging;
using YuGiOh_DeckBuilder.YuGiOh.Utility;
using Attribute = YuGiOh_DeckBuilder.YuGiOh.Enums.Attribute;

namespace YuGiOh_DeckBuilder.YuGiOh;

/// <summary>
/// https://yugioh.fandom.com/wiki/
/// </summary>
internal sealed partial class YuGiOhFandom
{
    //TODO: Delete files in folder if not update
    
    #region Methods
    /// <summary>
    /// Serializes the <see cref="MainWindow.Packs"/> to .json files
    /// </summary>
    private async Task SerializePacksAsync()
    {
        var currentPack = 0;
        var packCount = this.PackData.Count;

        await Parallel.ForEachAsync(this.PackData, async (packData, token) => // TODO: Use cancellation token
        {
            foreach (var card in packData.Cards)
            {
                if (this.CardData.FirstOrDefault(cardData => cardData.Endpoint == card.Endpoint) is { Passcode: { } } cardData)
                {
                    card.Passcode = cardData.Passcode.Value;
                }
                else
                {
                    if (card.Passcode < 1)
                    {
                        Log.Error(Error.PackCardPasscodeError, packData.Endpoint, card.Endpoint);
                    }
                }
            }
            
            var pack = new Pack(packData.Name ?? string.Empty, packData.ReleaseDate ?? default, packData.Endpoint, packData.Cards.ToList());
            
            await Json.SerializeAsync(this.packsFolder, packData.Endpoint, pack);

            Output.Print("Serializing pack:", ++currentPack, packCount, packData.Endpoint, MainWindow.TestOutputHelper);
        });
    }
    
    /// <summary>
    /// Build every <see cref="ACard"/> from <see cref="CardData"/>
    /// </summary>
    private async Task SerializeCardsAsync()
    {
        var currentCard = 0;
        var cardCount = this.CardData.Count;
        
        await Parallel.ForEachAsync(this.CardData, async (cardData, token) => // TODO: Use cancellation token
        {
            if (cardData.CardType! == typeof(Monster))
            {
                await BuildMonsterCard(cardData);
            }
            else if (cardData.CardType! == typeof(Spell))
            {
                await BuildSpellCard(cardData);
            }
            else if (cardData.CardType! == typeof(Trap))
            {
                await BuildTrapCard(cardData);
            }
            
            Output.Print("Serializing Card:", ++currentCard, cardCount, cardData.Endpoint, MainWindow.TestOutputHelper);
        });
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
        var imagePasscodes = Structure.GetFilePaths(Folder.Images, Extension.jpg).Select(Path.GetFileNameWithoutExtension).ToArray();
        
        await Parallel.ForEachAsync(this.CardData, async (card, token) => // TODO: Use cancellation token
        {
            // TODO: Only check this un update, must be overridden when doing a normal download
            if (!card.ImageEndpoint.IsNullEmptyOrWhitespace() && imagePasscodes.All(imagePasscode => imagePasscode != card.Passcode.ToString()))
            {
                var image = await WebClient.DownloadImage(ImageBaseUri + card.ImageEndpoint, ImageCodecInfo, EncoderParameters);

                if (image != null)
                {
                    await using var fileStream = Structure.OpenStream(this.imagesFolder, card.Passcode.ToString()!, Extension.jpg, FileMode.Create);
                    
                    image.Object.Save(fileStream, ImageCodecInfo, EncoderParameters);
                }
            }
            
            Output.Print("Downloading Image:", ++currentCard, cardCount, card.ImageEndpoint ?? string.Empty, MainWindow.TestOutputHelper);
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
            Endpoint: data.Endpoint,
            ImageEndpoint: data.ImageEndpoint ?? string.Empty,
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
            Endpoint: data.Endpoint,
            ImageEndpoint: data.ImageEndpoint ?? string.Empty,
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
            Endpoint: data.Endpoint,
            ImageEndpoint: data.ImageEndpoint ?? string.Empty,
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