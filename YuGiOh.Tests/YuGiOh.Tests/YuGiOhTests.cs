using Xunit.Abstractions;
using YuGiOh_DeckBuilder;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.Web;
using YuGiOh_DeckBuilder.YuGiOh;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Logging;

namespace YuGiOh_DeckBuilder_Tests.YuGiOh.Tests;

/// <summary>
/// Contains unit tests for the <see cref="YuGiOh"/> project
/// </summary>
public sealed partial class YuGiOhTests
{
    #region Members
    /// <summary>
    /// <see cref="ITestOutputHelper"/>
    /// </summary>
    private readonly ITestOutputHelper console;
    /// <summary>
    /// <see cref="YuGiOhFandom"/>
    /// </summary>
    private readonly YuGiOhFandom yuGiOhFandom;
    #endregion
    
    #region Constructor
    public YuGiOhTests(ITestOutputHelper console)
    {
        MainWindow.TestOutputHelper = console;
        this.console = console;
        this.yuGiOhFandom = new YuGiOhFandom();
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// Downloads the content of a website
    /// </summary>
    [Fact]
    public async Task DownloadWebsite()
    {
        await WebClient.DownloadWebsiteAsync("Underground_Arachnid", Folder.Websites_TEST);
        
        Log.Errors.ToList().ForEach(this.console.WriteLine);
        Assert.Empty(Log.Errors);
    }

    /// <summary>
    /// Downloads a single pack or card
    /// </summary>
    [Fact]
    public async Task DownloadSingle()
    {
        MainWindow.TestOutputHelper = this.console;
        
        const string packEndpoint = ""; // Lightning_Overdrive_%2B1_Bonus_Pack 
        const string cardEndpoint = "Ectoplasmic_Fortification!"; // Mazera_DeVille
        
        this.SetupDownload(packEndpoint, cardEndpoint);
        await this.yuGiOhFandom.DownloadData(packEndpoint, cardEndpoint);
        
        this.console.WriteLine("-------------------------[ERROR]-------------------------");
        Log.Errors.ToList().ForEach(this.console.WriteLine);
        
        await this.PrintDownloadedData(packEndpoint, cardEndpoint);
    }

    /// <summary>
    /// Downloads an image
    /// </summary>
    [Fact]
    public async Task DownloadImage()
    {
        const string imageEndpoint = "f/f2/PotofGreed-YGLD-EN-C-1E.png";

        await this.SetupImageDownload(imageEndpoint);
    }

    /// <summary>
    /// Test if the given object is correctly serialized/deserialized
    /// </summary>
    [Fact]
    public async void TestSerializing()
    {
        var testObject = this.testMonster;

        await this.Serialize(testObject);
    }
    
    [Fact]
#pragma warning disable CS1998
    public async Task Test()
#pragma warning restore CS1998
    {
        var uris = await File.ReadAllLinesAsync(Structure.BuildPath(Folder.Data_TEST, "Test1", Extension.txt));

        var list = new List<string>();
        
        foreach (var uri in uris)
        {
            if (uri.Contains("https://"))
            {
                list.Add($"\"{uri[(uri.LastIndexOf('/') + 1)..]}\",");
            }
        }
        
        list.OrderBy(list => list).ToList().ForEach(this.console.WriteLine);
    }
    #endregion
}