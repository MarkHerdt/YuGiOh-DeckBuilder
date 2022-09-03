using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
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
    /// /Data/Packs/
    /// </summary>
    private readonly Folder packsFolder = Folder.Packs;
    /// <summary>
    /// /Data/Cards/
    /// </summary>
    private readonly Folder cardsFolder = Folder.Cards;
    /// <summary>
    /// /Data/Images/
    /// </summary>
    private readonly Folder imagesFolder = Folder.Images;
    /// <summary>
    /// /Data/Localization/
    /// </summary>
    private readonly Folder localizationFolder = Folder.Localization;
    #endregion
    
    #region Properties
    /// <summary>
    /// Contains all data needed to create a <see cref="Pack"/>
    /// </summary>
    internal List<PackData> PackData { get; private set;  } = new();
    /// <summary>
    /// Contains all data needed to create a <see cref="ACard"/>
    /// </summary>
    private ConcurrentBag<CardData> CardData { get; } = new();
    /// <summary>
    /// List of all found passcodes <be/>
    /// <i>No duplicates</i>
    /// </summary>
    private ConcurrentBag<int> PassCodes { get; } = new();
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
    /// Downloads all necessary data from https://yugioh.fandom.com/wiki/
    /// </summary>
    /// <param name="packEndpoint">Only downloads the pack with this endpoint</param>
    /// <param name="cardEndpoint">Only downloads the card with this endpoint</param>
    internal async Task DownloadData(string packEndpoint = "", string cardEndpoint = "")
    {
        Log.SkippedPacks.Clear();
        Log.SkippedCards.Clear();
        
        await this.GetAllPacksAsync(packEndpoint, cardEndpoint);
        await this.GetAllCardsAsync();
        
        await this.SerializePacksAsync();
        await this.SerializeCardsAsync();
        
        await this.DownloadImagesAsync();
        
        this.Dispose();
    }

    /// <summary>
    /// Updates every missing <see cref="Pack"/>/<see cref="Card"/> and card image
    /// </summary>
    internal async Task UpdateData()
    {
        Log.SkippedPacks.Clear();
        Log.SkippedCards.Clear();
        
        await this.GetAllPacksAsync();
        await this.UpdatePacksAsync();
        await this.UpdateCardsAsync();
        await this.GetAllCardsAsync();
        
        await this.SerializePacksAsync();
        await this.SerializeCardsAsync();
        
        await this.DownloadImagesAsync();
        
        this.Dispose();
    }
    
    /// <summary>
    /// Disposes of objects that are not needed anymore
    /// </summary>
    private void Dispose()
    {
        this.PackData.Clear();
        this.CardData.Clear();
        this.PassCodes.Clear();
    }
    #endregion
}