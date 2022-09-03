using System.Text.RegularExpressions;
using YuGiOh_DeckBuilder;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.Utility.Json;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.Web;
using YuGiOh_DeckBuilder.YuGiOh;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;
using YuGiOh_DeckBuilder.YuGiOh.Enums;

namespace YuGiOh_DeckBuilder_Tests.YuGiOh.Tests;

public sealed partial class YuGiOhTests
{
    #region Methods
    /// <summary>
    /// Adds a single deserialized pack/card-endpoint from /Data/Packs/ to <see cref="YuGiOhFandom.Packs"/>
    /// </summary>
    /// <param name="packEndpoint">Endpoint of the pack (Leave empty, to only get a single card)</param>
    /// <param name="cardEndpoint">Endpoint of a card (Leave empty, to get a full pack)</param>
    private void SetupDownload(string packEndpoint = "", string cardEndpoint = "")
    {
        this.yuGiOhFandom.PackData.Clear();
        
        DeleteFolderContents(Folder.Packs_TEST);
        DeleteFolderContents(Folder.Cards_TEST);
        DeleteFolderContents(Folder.Localization_TEST);
        DeleteFolderContents(Folder.Images_TEST);
        
        PackData pack;
        
        // For a single existing pack
        if (!packEndpoint.IsNullEmptyOrWhitespace())
        {
            pack = new PackData(packEndpoint)
            {
                PackName = Regex.Replace(packEndpoint, @"[^a-zA-Z0-9]", " "),
                ReleaseDate = DateTime.Now
            };
        }
        // For a single card
        else
        {
            const string endPoint = "Test_Pack_-_Single_Card";

            pack = new PackData(endPoint)
            {
                PackName = Regex.Replace(endPoint, @"[_]", " "),
                ReleaseDate = DateTime.Now,
                Cards = { new Card(cardEndpoint, new List<Rarity>()) }
            };
        }
        
        this.yuGiOhFandom.PackData.Add(pack);
    }
    
    /// <summary>
    /// Prints the serialized pack/card to the console
    /// </summary>
    /// <param name="packEndpoint">Only downloads the pack with this endpoint</param>
    /// <param name="cardEndpoint">Only downloads the card with this endpoint</param>
    private async Task PrintDownloadedData(string packEndpoint = "", string cardEndpoint = "")
    {
        this.console.WriteLine("---------------------------------------------------------");

        try
        {
            if (!packEndpoint.IsNullEmptyOrWhitespace())
            {
                var pack = await Json.DeserializeAsync<Pack>(Folder.Packs_TEST, packEndpoint);
            
                this.console.WriteLine(pack!.ToString());
            }
            else
            {
                if (MainWindow.Packs?.SelectMany(pack => pack.Cards).FirstOrDefault(card => string.Equals(card.EndPoint, cardEndpoint))?.Passcode is { } passcode)
                {
                    var card = (await Json.DeserializeAsync<ACard>(Folder.Cards_TEST, passcode.ToString()))!;
                
                    console.WriteLine(card.Endpoint);
                
                    this.console.WriteLine(card.ToString());
                }
            }
        }
        catch (Exception exception)
        {
            this.console.WriteLine(exception.Message);
        }
    }

    /// <summary>
    /// Downloads the image from the given endpoint
    /// </summary>
    /// <param name="imageEndpoint">An endpoint of <see cref="YuGiOhFandom.ImageBaseUri"/></param>
    private async Task SetupImageDownload(string imageEndpoint)
    {
        DeleteFolderContents(Folder.Images_TEST);
        
        var image = await WebClient.DownloadImage(YuGiOhFandom.ImageBaseUri + imageEndpoint, YuGiOhFandom.ImageCodecInfo, YuGiOhFandom.EncoderParameters);

        if (image != null)
        {
            var filePath = Structure.BuildPath(Folder.Images_TEST, Path.GetFileNameWithoutExtension(imageEndpoint), Extension.jpg);

            await using var fileStream = File.Open(filePath, FileMode.OpenOrCreate);
                    
            image.Object.Save(fileStream, YuGiOhFandom.ImageCodecInfo, YuGiOhFandom.EncoderParameters);
        }
    }

    /// <summary>
    /// Serializes the given <see cref="object"/> in a .json format and saves it at: <br/>
    /// <i>/YuGiOh.Tests/Data/Serialize/SerializationTest.json</i>
    /// </summary>
    /// <param name="object">The <see cref="object"/> to serialize</param>
    /// <typeparam name="T">Can be any <see cref="Type"/></typeparam>
    private async Task Serialize<T>(T @object)
    {
        DeleteFolderContents(Folder.Serialize_TEST);
        
        const string fileName = "SerializationTest";
        
        await Json.SerializeAsync(Folder.Serialize_TEST, fileName, @object!);
        var json = await File.ReadAllTextAsync(Structure.BuildPath(Folder.Serialize_TEST, fileName, Extension.json));
        
        this.console.WriteLine("[SERIALIZED]");
        this.console.WriteLine(json);
        this.console.WriteLine("---------------------------------------------------------------------------------------");

        @object = (await Json.DeserializeAsync<T>(Folder.Serialize_TEST, fileName))!;
        
        this.console.WriteLine("[DESERIALIZED]");
        this.console.WriteLine(@object.ToString());
    }

    /// <summary>
    /// Deletes all files in the given <see cref="Folder"/>
    /// </summary>
    /// <param name="folder">The <see cref="Folder"/> to delete all files in</param>
    private static void DeleteFolderContents(Folder folder)
    {
        var folderPath = Structure.BuildPath(folder);
        
        foreach (var filePath in Directory.GetFiles(folderPath))
        {
            File.Delete(filePath);
        }
    }
    #endregion
}