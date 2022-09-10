using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;
using YuGiOh_DeckBuilder.YuGiOh.Logging;
using YuGiOh_DeckBuilder.YuGiOh.Utility;
using WebClient = YuGiOh_DeckBuilder.Web.WebClient;

namespace YuGiOh_DeckBuilder.YuGiOh;

/// <summary>
/// https://yugioh.fandom.com/wiki/Booster_Pack
/// </summary>
internal sealed partial class YuGiOhFandom
{
    #region Constants
    /// <summary>
    /// Endpoint for the card list <br/>
    /// https://yugioh.fandom.com/wiki/Booster_Pack
    /// </summary>
    private const string boosterPacksEndpoint = "Booster_Pack";
    #endregion
    
    #region Members
    /// <summary>
    /// <see cref="Regex"/> to find a booster pack
    /// </summary>
    private readonly Regex boosterPackRegex = new("<li><i><a href=\"/wiki/.+\" title=\".+\">.+</a></i></li>");
    /// <summary>
    /// <see cref="Regex"/> to get the endpoint of a pack
    /// </summary>
    private readonly Regex endpointRegex = new("wiki/.*\"");
    /// <summary>
    /// <see cref="Regex"/> to stop reading the table of booster packs
    /// </summary>
    private readonly Regex packStopConditionRegex = new("<ul><li><a href=\"/wiki/Template:");
    /// <summary>
    /// <see cref="Regex"/> to get the name of a pack
    /// </summary>
    private readonly Regex packNameRegex = new("<h2 class=\"pi-item pi-item-spacing pi-title pi-secondary-background\">.*</h2>");
    /// <summary>
    /// <see cref="Regex"/> to get the release date of a pack
    /// </summary>
    private readonly Regex releaseDateRegex = new("<div class=\"pi-data-value pi-font\">\\w+\\s\\d+,\\s\\d+</div>");
    /// <summary>
    /// <see cref="Regex"/> to get the start of the table that holds the cards in a pack
    /// </summary>
    private readonly Regex cardTableRegex = new("<table(.*?)sortable(.*?)>");
    /// <summary>
    /// Same as <see cref="cardTableRegex"/>, but for tables which are in a single line
    /// </summary>
    private readonly Regex singleLineCardTableRegex = new("<table(.*?)sortable(.*?)>(.*?)</table>");
    /// <summary>
    /// <see cref="Regex"/> to get the headers columns of a card list table
    /// </summary>
    private readonly Regex cardHeaderRegex = new("<th(.*?)>[a-zA-Z\\d\\s]+</th>");
    /// <summary>
    /// <see cref="Regex"/> to get a card in a card list table
    /// </summary>
    private readonly Regex cardRowRegex = new("<tr><td>(.*?)</td></tr>");
    /// <summary>
    /// <see cref="Regex"/> to get each column of a card row
    /// </summary>
    private readonly Regex cardColumnRegex = new("<td(.*?)>(.*?)</td>");
    /// <summary>
    /// <see cref="Regex"/> to get the endpoint of a card
    /// </summary>
    private readonly Regex cardEndpointRegex = new("<a href=\"/wiki/(.*?)\"");
    /// <summary>
    /// <see cref="Regex"/> to get the rarity of a card
    /// </summary>
    private readonly Regex cardRarityRegex = new("<a href=\"/wiki/(.*?)\">(.*?)</a>");

    /// <summary>
    /// Contains endpoints for packs that are missing on: https://yugioh.fandom.com/wiki/Booster_Pack
    /// </summary>
    private readonly ReadOnlyCollection<string> missingPackEndpoints = new
    (
        new List<string>
        {
            "Ancient_Guardians",
            "Battle_of_Chaos",
            "Battle_Pack_2:_War_of_the_Giants",
            "Battle_Pack_3:_Monster_League",
            "Battle_Pack:_Epic_Dawn",
            "Battles_of_Legend:_Armageddon",
            "Battles_of_Legend:_Hero%27s_Revenge",
            "Battles_of_Legend:_Light%27s_Revenge",
            "Battles_of_Legend:_Relentless_Revenge",
            "Brothers_of_Legend",
            "Crossed_Souls",
            "Dark_Beginning_1",
            "Dark_Beginning_2",
            "Dark_Legends",
            "Dark_Revelation_Volume_1",
            "Dark_Revelation_Volume_2",
            "Dark_Revelation_Volume_3",
            "Dark_Revelation_Volume_4",
            "Dark_Saviors",
            "Destiny_Soldiers",
            "Dimension_of_Chaos",
            "Dragons_of_Legend_2",
            "Dragons_of_Legend:_The_Complete_Series",
            "Dragons_of_Legend:_Unleashed",
            "Dragons_of_Legend",
            "Duel_Overload",
            "Duelist_Alliance",
            "Eternity_Code",
            "Exclusive_Pack",
            "Fists_of_the_Gadgets",
            "Fusion_Enforcers",
            "Galactic_Overlord",
            "Genesis_Impact",
            "Ghosts_From_the_Past_(set)",
            "Ghosts_From_the_Past:_The_2nd_Haunting",
            "Hidden_Arsenal_2",
            "Hidden_Arsenal_3",
            "Hidden_Arsenal_4:_Trishula%27s_Triumph",
            "Hidden_Arsenal_5:_Steelswarm_Invasion ",
            "Hidden_Arsenal_6:_Omega_Xyz",
            "Hidden_Arsenal_7:_Knight_of_Stars",
            "Hidden_Arsenal:_Chapter_1",
            "Hidden_Arsenal",
            "Hidden_Summoners",
            "High-Speed_Riders",
            "Invasion:_Vengeance",
            "Judgment_of_the_Light",
            "King%27s_Court",
            "Legendary_Duelists:_Ancient_Millennium",
            "Legendary_Duelists:_Duels_from_the_Deep",
            "Legendary_Duelists:_Immortal_Destiny",
            "Legendary_Duelists:_Magical_Hero",
            "Legendary_Duelists:_Rage_of_Ra",
            "Legendary_Duelists:_Season_1",
            "Legendary_Duelists:_Season_2",
            "Legendary_Duelists:_Season_3",
            "Legendary_Duelists:_Sisters_of_the_Rose",
            "Legendary_Duelists:_Synchro_Storm",
            "Legendary_Duelists:_White_Dragon_Abyss",
            "Legendary_Duelists",
            "Millennium_Pack",
            "Movie_Pack",
            "Mystic_Fighters",
            "Number_Hunters",
            "Pendulum_Evolution",
            "Premium_Pack_(TCG)",
            "Premium_Pack_2_(TCG)",
            "Ra_Yellow_Mega_Pack",
            "Retro_Pack_2",
            "Retro_Pack",
            "Secret_Slayers",
            "Secrets_of_Eternity",
            "Speed_Duel:_Arena_of_Lost_Souls",
            "Speed_Duel:_Scars_of_Battle",
            "Speed_Duel:_Trials_of_the_Kingdom",
            "Spirit_Warriors",
            "Star_Pack_2013",
            "Star_Pack_2014",
            "Star_Pack_ARC-V",
            "Star_Pack_Battle_Royal",
            "Star_Pack_VRAINS",
            "Tactical_Masters",
            "The_Grand_Creators",
            "The_Infinity_Chasers",
            "The_New_Challengers",
            "The_Secret_Forces",
            "Toon_Chaos",
            "War_of_the_Giants:_Round_2",
            "Wing_Raiders",
            "World_Superstars",
            "Yu-Gi-Oh!_3D_Bonds_Beyond_Time_Movie_Pack",
            "Yu-Gi-Oh!_The_Dark_Side_of_Dimensions_Movie_Pack_Secret_Edition",
            "Yu-Gi-Oh!_The_Dark_Side_of_Dimensions_Movie_Pack"
        }
    );
    #endregion
    
    #region Methods
    /// <summary>
    /// Gets all available packs from: <br/>
    /// https://yugioh.fandom.com/wiki/Booster_Pack
    /// </summary>
    /// <param name="packEndpoint">Only downloads the pack with this endpoint</param>
    /// <param name="cardEndpoint">Only downloads the card with this endpoint</param>
    internal async Task GetAllPacksAsync(string packEndpoint = "", string cardEndpoint = "")
    {
        if (packEndpoint.IsNullEmptyOrWhitespace() && cardEndpoint.IsNullEmptyOrWhitespace())
        {
            await this.GetBoosterPackEndpointsAsync(); 
        }

        if (cardEndpoint.IsNullEmptyOrWhitespace())
        {
            await this.GetBoosterPacksDataAsync();
        }
    }

    /// <summary>
    /// Asynchronously gets all booster packs from: <br/>
    /// https://yugioh.fandom.com/wiki/Booster_Pack
    /// </summary>
    private async Task GetBoosterPackEndpointsAsync()
    {
        var stopCondition = false;
        
        await WebClient.ReadWebsiteAsStreamAsync(BaseUri + boosterPacksEndpoint, this.GetBoosterPackEndpoint, line =>
        {
            return stopCondition = this.packStopConditionRegex.IsMatch(line);
        });

        foreach (var missingPackEndpoint in missingPackEndpoints)
        {
            if (this.PackData.All(pack => pack.Endpoint != missingPackEndpoint))
            {
                this.PackData.Add(new PackData(missingPackEndpoint));
            }
        }
        
        this.PackData.AddRange(await this.GetMissingPacks());
        
        if (!stopCondition)
        {
            Log.Error(Error.PackStopConditionError, boosterPacksEndpoint, stopCondition.ToString());
        }
    }
    
    /// <summary>
    /// Gets the endpoint of a booster pack and adds it to <see cref="MainWindow.Packs"/>
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    private void GetBoosterPackEndpoint(string line)
    {
        var boosterPackMatch = this.boosterPackRegex.Match(line);

        if (boosterPackMatch.Success)
        {
            var endpointMatch = this.endpointRegex.Match(boosterPackMatch.Value);
            
            var startIndex = endpointMatch.Value.IndexOf('/') + 1;
            var length = endpointMatch.Value.IndexOf('"') - startIndex;
            
            var endpoint = endpointMatch.Value.Substring(startIndex, length);
            
            this.PackData.Add(new PackData(endpoint));
        }
    }

    /// <summary>
    /// Gets all packs from <see cref="FileName.MissingPacks"/>
    /// </summary>
    /// <returns>A <see cref="List{T}"/> of <see cref="PackData"/> with the pack endpoints from <see cref="FileName.MissingPacks"/></returns>
    private async Task<List<PackData>> GetMissingPacks()
    {
        var regex = new Regex($@"^{BaseUri}[^/\s]+$");
        var filePath = Structure.BuildPath(FileName.MissingPacks);
        var lines = await File.ReadAllLinesAsync(filePath);
        
        return lines.Where(line => regex.IsMatch(line)).Select(line => new PackData(line.Replace(BaseUri, string.Empty))).ToList();
    }
    
    /// <summary>
    /// Asynchronously gets the data of all booster packs in <see cref="MainWindow.Packs"/> 
    /// </summary>
    private async Task GetBoosterPacksDataAsync()
    {
        var currentPack = 0;
        var packCount = this.PackData.Count;

        await Parallel.ForEachAsync(this.PackData, async (packData, token) => // TODO: Use cancellation token
        {
            var response = await WebClient.ReadWebsiteAsStreamAsync(BaseUri + packData.Endpoint, line =>
            {
                this.GetBoosterPackName(line, packData);
                this.GetReleaseDate(line, packData);
                this.GetCardTable(line, packData);
                
            }, _ => packData.StopSearch);

            
            if (CheckForErrors(response, packData))
            {
                goto SKIP_PACK; // TODO: Check if pack needs to be skipped when an error occured
            }

            SKIP_PACK:;
            
            Output.Print("Getting pack data:", ++currentPack, packCount, packData.Endpoint, MainWindow.TestOutputHelper);
        });

        await Parallel.ForEachAsync(this.PackData.AsParallel().SelectMany(packData => packData.Cards).GroupBy(card => card.Endpoint), (grouping, token) => // TODO: Use cancellation token
        {
            var rarities = new List<Rarity>();
            
            foreach (var card in grouping)
            {
                rarities.AddRange(card.Rarities);
            }

            rarities = rarities.Distinct().ToList();
            
            foreach (var card in grouping)
            {
                card.Rarities = new List<Rarity>(rarities);
            }
            
            return ValueTask.CompletedTask;
        });
        
        Output.PrintSkip(this.PackData.Count, this.PackData.Count, Log.SkippedPacks, MainWindow.TestOutputHelper);
    }
    
    /// <summary>
    /// Gets the name of a booster pack
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="packData">Contains all data of a pack</param>
    private void GetBoosterPackName(string line, PackData packData)
    {
        if (packData.Name != null) return;
        
        var packNameMatch = this.packNameRegex.Match(line);

        if (packNameMatch.Success)
        {
            var startIndex = packNameMatch.Value.IndexOf('>') + 1;
            var length = packNameMatch.Value.LastIndexOf('<') - startIndex;

            packData.Name = packNameMatch.Value.Substring(startIndex, length);
        }
    }

    /// <summary>
    /// Gets the earliest release date of a booster pack
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="packData">Contains all data of a pack</param>
    private void GetReleaseDate(string line, PackData packData)
    {
        if (packData.ReleaseDate != null) return;
        
        var releaseDateMatch = this.releaseDateRegex.Match(line);

        if (releaseDateMatch.Success)
        {
            var startIndex = releaseDateMatch.Value.IndexOf('>') + 1;
            var length = releaseDateMatch.Value.LastIndexOf('<') - startIndex;
            var date = releaseDateMatch.Value.Substring(startIndex, length);

            var parsedDate = DateTime.SpecifyKind(DateTime.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal), DateTimeKind.Utc);

            if (packData.ReleaseDate == null)
            {
                packData.ReleaseDate = parsedDate;
            }
            else
            {
                if (parsedDate.Date < packData.ReleaseDate.Value.Date)
                {
                    packData.ReleaseDate = parsedDate;
                }
            }
        }
    }

    /// <summary>
    /// Gets the table list of cards
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="packData">Contains all data of a pack</param>
    private void GetCardTable(string line, PackData packData)
    {
        if (this.cardTableRegex.IsMatch(line))
        {
            packData.CardTableFound = true;
        }
        
        if (packData.CardTableFound)
        {
            var singleLineMatch = this.singleLineCardTableRegex.Match(line);
            
            // For tables that are in a single line
            if (singleLineMatch.Success)
            {
                packData.StopSearch = true;
                
                this.GetCard(packData.Endpoint, singleLineMatch.Value, packData);
            }
            // For tables with multiple lines
            else
            {
                packData.StringBuilder.Item.Append(line);  
                
                if (line.Contains("</table>"))
                {
                    packData.StopSearch = true;
                    
                    this.GetCard(packData.Endpoint, packData.StringBuilder.Item.ToString(), packData);
                    
                    packData.StringBuilder.Return();
                }
            }
        }
    }

    /// <summary>
    /// Gets a card in a pack
    /// </summary>
    /// <param name="currentEndpoint">Endpoint of the booster pack that is currently being read</param>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="packData">Contains all data of a pack</param>
    private void GetCard(string currentEndpoint, string line, PackData packData)
    {
        var headerMatches = this.cardHeaderRegex.Matches(line);
        
        if (headerMatches.Any())
        {
            var nameColumnNumber = GetColumnIndex(currentEndpoint, headerMatches, "NAME");
            var rarityColumnNumber = GetColumnIndex(currentEndpoint, headerMatches, "RARITY");

            var cardRowMatches = this.cardRowRegex.Matches(line);
            
            foreach (var cardRowMatch in cardRowMatches.Cast<Match>())
            {
                var cardColumnMatches = this.cardColumnRegex.Matches(cardRowMatch.Value);
                
                if (cardColumnMatches.Any())
                {
                    var nameColumn = cardEndpointRegex.Match(cardColumnMatches[nameColumnNumber].Value).Value;
                    var rarityColumn = cardRarityRegex.Matches(cardColumnMatches[rarityColumnNumber].Value);
                    
                    var cardEndpoint = string.Empty;
                    var rarities = new List<Rarity>();
                    
                    try
                    {
                        var endPointStart = nameColumn.NthIndexOf('/', 2) + 1;
                        var endPointLength = nameColumn.LastIndexOf('"') - endPointStart;
                        
                        cardEndpoint = nameColumn.Substring(endPointStart, endPointLength);
                    }
                    catch { /* Ignored */ }

                    try
                    {
                        foreach (var rarity in rarityColumn.Cast<Match>())
                        {
                            var rarityStart = rarity.Value.IndexOf('>') + 1;
                            var rarityLenght = rarity.Value.LastIndexOf('<') - rarityStart;
                            var rarityString = rarity.Value.Substring(rarityStart, rarityLenght);

                            foreach (var kvp in Translator.Rarities[Language.en])
                            {
                                if (string.Equals(kvp.Value, rarityString))
                                {
                                    rarities.Add(kvp.Key);
                                    break;
                                }
                            }
                        }
                    }
                    catch { /* Ignored */ }

                    if (cardEndpoint != string.Empty)
                    {
                        if (rarities.Any())
                        {
                            packData.Cards.Add(new Card(cardEndpoint, rarities));  
                        }
                        else
                        {
                            packData.Cards.Add(new Card(cardEndpoint, new List<Rarity>()));  
                            Log.Error(Error.CardRarityError, currentEndpoint, cardColumnMatches[rarityColumnNumber].Value);
                        }
                    }
                    else
                    {
                        Log.Error(Error.CardEndpointError, currentEndpoint, cardColumnMatches[nameColumnNumber].Value);
                    }
                }
                else
                {
                    Log.Error(Error.CardColumnMatchError, currentEndpoint, cardRowMatch.Value);
                }
            }
        }
        else
        {
            Log.Error(Error.CardHeaderMatchError, currentEndpoint, line);
        }
    }

    /// <summary>
    /// Gets the column index of the given searchString in a table header
    /// </summary>
    /// <param name="currentEndpoint">Endpoint of the booster pack that is currently being read</param>
    /// <param name="matchCollection"><see cref="MatchCollection"/> that contains all found table header columns</param>
    /// <param name="searchString">The <see cref="string"/> to search for in the table headers</param>
    /// <returns>The column index, where the searchString was found, or -1 if it couldn't be found</returns>
    private static int GetColumnIndex(string currentEndpoint, MatchCollection matchCollection, string searchString)
    {
        for (var i = 0; i < matchCollection.Count; i++)
        {
            if (matchCollection[i].Value.ToUpper().Contains(searchString))
            {
                return i;
            }
        }

        Log.Error(Error.CardColumnNumberError, currentEndpoint, matchCollection.Aggregate(string.Empty, (current, match) => $"{current} {match.Value}"));
        
        return -1;
    }

    /// <summary>
    /// Checks if error occured during the data search
    /// </summary>
    /// <param name="response">Response from the http request</param>
    /// <param name="packData">Contains all data of a pack</param>
    /// <returns>True == skip this pack</returns>
    private static bool CheckForErrors(HttpStatusCode response, PackData packData)
    {
        if (response == HttpStatusCode.BadRequest)
        {
            Log.SkipPack(Skip.BadRequest, packData.Endpoint); // TODO: Print/Log skipped packs
            return true;
        }
        
        if (packData.Name == null)
        {
            Log.Error(Error.PackNameError, packData.Endpoint, "null");
        }
        
        if (packData.ReleaseDate == null)
        {
            Log.Error(Error.PackReleaseDateError, packData.Endpoint, "null");
        }

        if (!packData.StopSearch)
        {
            Log.Error(Error.CardTableMatchError, packData.Endpoint, packData.StopSearch.ToString());
        }

        return false;
    }
    #endregion
}