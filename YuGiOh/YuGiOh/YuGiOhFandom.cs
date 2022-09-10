using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YuGiOh_DeckBuilder.Utility.Json;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using YuGiOh_DeckBuilder.YuGiOh.Logging;

namespace YuGiOh_DeckBuilder.YuGiOh;

/// <summary>
/// https://yugioh.fandom.com/wiki/
/// </summary>
internal sealed partial class YuGiOhFandom
{
    #region Constants
    /// <summary>
    /// Base uri for https://www.db.yugioh-card.com/yugiohdb/
    /// </summary>
    internal const string BaseUri = "https://yugioh.fandom.com/wiki/";
    /// <summary>
    /// Base uri for card images
    /// </summary>
    internal const string ImageBaseUri = "https://static.wikia.nocookie.net/yugioh/images/";
    #endregion

    #region Statics
    /// <summary>
    /// Maximum number of different <see cref="Language"/>s
    /// </summary>
    private static readonly int languageCount = Enum.GetValues<Language>().Length;
    /// <summary>
    /// The quality of an <see cref="Image"/> <br/>
    /// <i>1 - 100</i>
    /// </summary>
    private static long imageQuality = 100L; // 1 - 100
    /// <summary>
    /// The quality of an <see cref="Image"/> <br/>
    /// <i>1 - 100</i>
    /// </summary>
    internal static long ImageQuality
    {
        get => imageQuality;
        set => EncoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, imageQuality = value);
    }
    /// <summary>
    /// <see cref="System.Drawing.Imaging.ImageCodecInfo"/> <br/>
    /// <i>Will be .jpg</i>
    /// </summary>
    internal static readonly ImageCodecInfo ImageCodecInfo = ImageCodecInfo.GetImageEncoders().First(codecInfo => codecInfo.MimeType == "image/jpeg");
    /// <summary>
    /// <see cref="System.Drawing.Imaging.EncoderParameters"/>
    /// </summary>
    internal static readonly EncoderParameters EncoderParameters = new(1)
    {
        Param = new[] { new EncoderParameter(Encoder.Quality, imageQuality) }
    };
    #endregion

    #region Members
    /// <summary>
    /// <see cref="Folder.Packs"/>/<see cref="Folder.Packs_TEST"/>
    /// </summary>
    private readonly Folder packsFolder = Folder.Packs;
    /// <summary>
    /// <see cref="Folder.Cards"/>/<see cref="Folder.Cards_TEST"/>
    /// </summary>
    private readonly Folder cardsFolder = Folder.Cards;
    /// <summary>
    /// <see cref="Folder.Images"/>/<see cref="Folder.Images_TEST"/>
    /// </summary>
    private readonly Folder imagesFolder = Folder.Images;
    /// <summary>
    /// <see cref="Folder.Localization"/>/<see cref="Folder.Localization_TEST"/>
    /// </summary>
    private readonly Folder localizationFolder = Folder.Localization;
    #endregion
    
    #region Properties
    /// <summary>
    /// Every <see cref="Pack"/> in <see cref="Folder.Packs"/>
    /// </summary>
    internal List<Pack> DeserializedPacks { get; private set; } = new();
    /// <summary>
    /// Every <see cref="ACard"/> in <see cref="Folder.Cards"/>
    /// </summary>
    internal List<ACard> DeserializedCards { get; private set; } = new();
    /// <summary>
    /// Contains all data needed to create a <see cref="Pack"/>
    /// </summary>
    internal List<PackData> PackData { get; private set;  } = new();
    /// <summary>
    /// Contains all data needed to create a <see cref="ACard"/>
    /// </summary>
    private ConcurrentBag<CardData> CardData { get; } = new();
    #endregion

    #region Constructor
    internal YuGiOhFandom()
    {
        if (MainWindow.TestOutputHelper != null)
        {
            this.packsFolder = Folder.Packs_TEST;
            this.cardsFolder = Folder.Cards_TEST;
            this.imagesFolder = Folder.Images_TEST;
            this.localizationFolder = Folder.Localization_TEST;
        }
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// Deserializes every <see cref="Pack"/>/<see cref="ACard"/> in <see cref="Folder.Packs"/>/<see cref="Folder.Cards"/> and adds them to <see cref="DeserializedPacks"/>/<see cref="DeserializedCards"/>
    /// </summary>
    internal async Task DeserializePackCardsAsync()
    {
        Console.WriteLine("Deserializing");
        
        await Task.Run(async () =>
        {
            var packPaths = Structure.GetFilePaths(Folder.Packs, Extension.json);
            var packs = await Json.DeserializeAsync<Pack>(packPaths);
            this.DeserializedPacks = new List<Pack>(packs);
            
            var cardPaths = Structure.GetFilePaths(Folder.Cards, Extension.json);
            var cards = await Json.DeserializeAsync<ACard>(cardPaths);
            this.DeserializedCards = new List<ACard>(cards);
        });
    }
    
    /// <summary>
    /// Downloads all necessary data from https://yugioh.fandom.com/wiki/
    /// </summary>
    /// <param name="packEndpoint">Only downloads the pack with this endpoint</param>
    /// <param name="cardEndpoint">Only downloads the card with this endpoint</param>
    internal async Task DownloadData(string packEndpoint = "", string cardEndpoint = "")
    {
        Log.SkippedPacks.Clear();
        Log.SkippedCards.Clear();

        await this.DeserializePackCardsAsync();
        
        await this.GetAllPacksAsync(packEndpoint, cardEndpoint);
        await this.GetAllCardsAsync();
        
        await this.SerializePacksAsync();
        await this.SerializeCardsAsync();
        
        await this.DownloadImagesAsync();

        await this.CleanUp();
        
        this.Dispose();
    }

    /// <summary>
    /// Updates every missing <see cref="Pack"/>/<see cref="Card"/> and card image
    /// </summary>
    internal async Task UpdateData()
    {
        Log.SkippedPacks.Clear();
        Log.SkippedCards.Clear();
        
        await this.DeserializePackCardsAsync();
        
        await this.GetAllPacksAsync();
        await this.UpdateAsync();
        await this.GetAllCardsAsync();
        
        await this.SerializePacksAsync();
        await this.SerializeCardsAsync();
        
        await this.DownloadImagesAsync();
        
        this.Dispose();
    }

    /// <summary>
    /// Deletes all files in <see cref="Folder.Packs"/>/<see cref="Folder.Cards"/>/<see cref="Folder.Localization"/>/<see cref="Folder.Images"/>, that are not in <see cref="PackData"/>/<see cref="CardData"/>
    /// </summary>
    private async Task CleanUp()
    {
        var packFilePaths = Structure.GetFilePaths(Folder.Packs, Extension.json);
        var cardFilePaths = Structure.GetFilePaths(Folder.Cards, Extension.json);
        var localizationFilePaths = Structure.GetFilePaths(Folder.Localization, Extension.json);
        var imageFilePaths = Structure.GetFilePaths(Folder.Images, Extension.jpg);

        Console.WriteLine("Cleaning up Packs");
        
        await Parallel.ForEachAsync(packFilePaths, (packFilePath, token) =>
        {
            var endpoint = Path.GetFileNameWithoutExtension(packFilePath);
                
            if (this.PackData.All(packData => Structure.ReplaceForbiddenFileNameCharacters(packData.Endpoint) != endpoint))
            {
                File.Delete(packFilePath);
            }
            
            return ValueTask.CompletedTask;
        });
        
        Console.WriteLine("Cleaning up Cards");
        
        await Parallel.ForEachAsync(cardFilePaths, (cardFilePath, token) =>
        {
            var passcode = Path.GetFileNameWithoutExtension(cardFilePath);
                
            if (this.CardData.All(cardData => cardData.Passcode!.Value.ToString() != passcode))
            {
                File.Delete(cardFilePath);
            }
            
            return ValueTask.CompletedTask;
        });
        
        Console.WriteLine("Cleaning up Localizations");
        
        await Parallel.ForEachAsync(localizationFilePaths, (localizationFilePath, token) =>
        {
            var passcode = Path.GetFileNameWithoutExtension(localizationFilePath);
                
            if (this.CardData.All(cardData => cardData.Passcode!.Value.ToString() != passcode))
            {
                File.Delete(localizationFilePath);
            }
            
            return ValueTask.CompletedTask;
        });
        
        Console.WriteLine("Cleaning up Images");
        
        await Parallel.ForEachAsync(imageFilePaths.Where(imageFilePath => imageFilePath != Structure.BuildPath(FileName.CardBack)), (imageFilePath, token) =>
        {
            var passcode = Path.GetFileNameWithoutExtension(imageFilePath);
                
            if (this.CardData.All(cardData => cardData.Passcode!.Value.ToString() != passcode))
            {
                File.Delete(imageFilePath);
            }
            
            return ValueTask.CompletedTask;
        });
    }
    
    /// <summary>
    /// Disposes of objects that are not needed anymore
    /// </summary>
    private void Dispose()
    {
        this.DeserializedPacks.Clear();
        this.DeserializedCards.Clear();
        this.PackData.Clear();
        this.CardData.Clear();
    }
    #endregion
}