using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Enums;
using YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;
using YuGiOh_DeckBuilder.YuGiOh.Logging;
using YuGiOh_DeckBuilder.YuGiOh.Utility;
using Attribute = YuGiOh_DeckBuilder.YuGiOh.Enums.Attribute;
using WebClient = YuGiOh_DeckBuilder.Web.WebClient;

namespace YuGiOh_DeckBuilder.YuGiOh;

/// <summary>
/// https://yugioh.fandom.com/wiki/
/// </summary>
internal sealed partial class YuGiOhFandom
{
    #region Members
    /// <summary>
    /// <see cref="Regex"/> to get the card image
    /// </summary>
    private readonly Regex cardImageRegex = new("<td class=\"cardtable-cardimage\" rowspan=\"50\"><a href=\"https://static[.]wikia[.]nocookie[.]net/yugioh/images/(.*?)[.](png|jpg)");
    /// <summary>
    /// <see cref="Regex"/> to get the english name of the card
    /// </summary>
    private readonly Regex englishCardNameRegex = new("<th class=\"cardtable-header\"(.*?)>(.*?)<([>]?)");
    /// <summary>
    /// <see cref="Regex"/> to get the name of the card for all translations except english
    /// </summary>
    private readonly Regex cardNameRegex = new("<span lang=\"\\w+\">.+</span>");
    /// <summary>
    /// <see cref="Regex"/> to get the card type
    /// </summary>
    private readonly Regex cardTypeRegex = new("<a href=\"/wiki/(Monster|Spell|Trap)_Card\" title=\"(Monster|Spell|Trap) Card\">(Monster|Spell|Trap)</a>");
    /// <summary>
    /// <see cref="Regex"/> to get the attribute of a <see cref="Monster"/> card
    /// </summary>
    private readonly Regex attributeRegex = new("<tr class=\"cardtablerow\"><th class=\"cardtablerowheader\" scope=\"row\"><a href=\"/wiki/Attribute\" title=\"Attribute\">Attribute</a></th><td class=\"cardtablerowdata\">");
    /// <summary>
    /// <see cref="Regex"/> to the the type/s of a <see cref="Monster"/> card
    /// </summary>
    private readonly Regex monsterTypesRegex = new("<a href=\"/wiki/Type\" title=\"Type\">Type[s]?</a>");
    /// <summary>
    /// <see cref="Regex"/> to split all types in one line
    /// </summary>
    private readonly Regex typeSplitRegex = new("(<a href=\"/wiki/(.*?)\" title=\"(.*?)\">(.*?)</a>(\\s[/]\\s)?)");
    /// <summary>
    /// <see cref="Regex"/> to get the level/rank of a <see cref="Monster"/> card
    /// </summary>
    private readonly Regex monsterLevelRegex = new("<a href=\"/wiki/(Level|Rank)_\\d+_Monster_Cards\" title=\"(Level|Rank) \\d+ Monster Cards\">\\d+</a>");
    /// <summary>
    /// <see cref="Regex"/> to get the pendulum scale of a <see cref="Monster"/> card
    /// </summary>
    private readonly Regex pendulumScaleRegex = new("<a href=\"/wiki/Pendulum_Scale_\\d+_Monster_Cards\" title=\"Pendulum Scale \\d+ Monster Cards\">\\d+</a>");
    /// <summary>
    /// <see cref="Regex"/> to get the link arrows of a <see cref="Monster"/> card
    /// </summary>
    private readonly Regex linkArrowRegex = new("<a href=\"/wiki/[a-zA-z-]+_Link_Arrow_Monster_Cards\" title=\"[a-zA-z-]+ Link Arrow Monster Cards\">[a-zA-z-]+</a>");
    /// <summary>
    /// <see cref="Regex"/> to get the ATK/DEF/Link-Rating of a <see cref="Monster"/> card
    /// </summary>
    private readonly Regex monsterCardStatsRegex = new("<a href=\"/wiki/(\\d+|%3F)_ATK_Monster_Cards\" title=\"(\\d+|[?]) ATK Monster Cards\">(\\d+|[?])</a> / <a href=\"/wiki/((\\d+|%3F)_DEF|Link_\\d+)_Monster_Cards\" title=\"((\\d+|[?]) DEF|Link \\d+) Monster Cards\">(\\d+|[?])</a></td></tr>");
    /// <summary>
    /// <see cref="Regex"/> to get the link rating of a <see cref="Monster"/> card
    /// </summary>
    private readonly Regex linkRatingRegex = new("<a href=\"/wiki/Link_\\d+_Monster_Cards\" title=\"Link \\d+ Monster Cards\">\\d+</a>");
    /// <summary>
    /// <see cref="Regex"/> to get the property type for <see cref="Spell"/>/<see cref="Trap"/> cards
    /// </summary>
    private readonly Regex propertyTypeRegex = new("<a href=\"/wiki/[a-zA-Z-]+_(Spell|Trap)_Card\" title=\"[a-zA-Z-]+ (Spell|Trap) Card\">[a-zA-Z-]+</a>");
    /// <summary>
    /// <see cref="Regex"/> that indicates, the row containing the passcode, has been reached
    /// </summary>
    private readonly Regex passcodeReached = new("<tr class=\"cardtablerow\"><th class=\"cardtablerowheader\" scope=\"row\"><a href=\"/wiki/Passcode\" title=\"Passcode\">Passcode</a></th><td class=\"cardtablerowdata\">");
    /// <summary>
    /// <see cref="Regex"/> to get the passcode of a card
    /// </summary>
    private readonly Regex passcodeRegex = new(">\\d+</(a|span)></td></tr>");
    /// <summary>
    /// <see cref="Regex"/> to get the statuses of a card
    /// </summary>
    private readonly Regex cardStatusRegex = new("<td class=\"cardtablerowdata\"(.*?)><a href=\"/wiki/\\w+\" title=\"\\w+\">\\w+</a>");
    /// <summary>
    /// <see cref="Regex"/> to get the language of a description
    /// </summary>
    private readonly Regex descriptionLanguageRegex = new("<tbody><tr><th class=\"navbox-title\"><div style=\"font-size: 110%;\"><div style=\"float: left; width: 6em;\">&#160;</div>\\w+</div></th></tr>");
    /// <summary>
    /// <see cref="Regex"/> that indicates, that the next line will contain a card description
    /// </summary>
    private readonly Regex cardDescriptionRegex = new("<tr><td class=\"navbox-list\" style=\"text-align: left; padding: 0px; ;\">");
    /// <summary>
    /// <see cref="Regex"/> to match everything in between the "less-than" and "greater-than" signs
    /// </summary>
    private readonly Regex htmlBracketsRegex = new("<.*?>");
    /// <summary>
    /// <see cref="Regex"/> to stop reading the card info table
    /// </summary>
    private readonly Regex cardStopConditionRegex = new("\\w+</a></i> sets</b><br />|^NewPP limit report$");
    #endregion
    
    #region Methods
    /// <summary>
    /// Gets all cards from <see cref="PackData"/>
    /// </summary>
    private async Task GetAllCardsAsync()
    {
        var cards = this.PackData.AsParallel().SelectMany(packData => packData.Cards).Where(card => !card.Skip).DistinctBy(card => card.Endpoint).ToArray();
        
        var currentCard = 0;
        var cardCount = cards.Length;
        var customPasscode = this.GetHighestPasscode();

        await Parallel.ForEachAsync(cards, async (card, token) => // TODO: Use cancellation token
        {
            var cardData = new CardData(card.Endpoint, card.Rarities);

            var response = await WebClient.ReadWebsiteAsStreamAsync(BaseUri + card.Endpoint, line =>
            {
                this.GetImage(line, cardData);
                this.GetName(line, cardData);
                this.GetCardType(line, cardData);
                this.GetAttribute(line, cardData);
                this.GetMonsterTypes(line, cardData);
                this.GetMonsterLevel(line, cardData);
                this.GetPendulumScale(line, cardData);
                this.GetLinkArrows(line, cardData);
                this.GetMonsterStats(line, cardData);
                this.GetPropertyType(line, cardData);
                this.GetPassCode(line, cardData);
                this.GetStatuses(line, cardData);
                this.GetDescriptions(line, cardData);

            }, line =>
            {
                return cardData.StopSearch = cardData.AllDescriptionsFound || this.cardStopConditionRegex.IsMatch(line);
            });

            cardData.Passcode ??= this.DeserializedCards.FirstOrDefault(deserializedCard => deserializedCard.Endpoint == cardData.Endpoint)?.Passcode ?? Interlocked.Increment(ref customPasscode);
            
            if (CheckForErrors(response, cardData))
            {
                goto SKIP_CARD;
            }

            this.CardData.Add(cardData);
                
            SKIP_CARD:;
                
            Output.Print("Getting card data:", ++currentCard, cardCount, card.Endpoint, MainWindow.TestOutputHelper);
        });
        
        Output.PrintSkip(cardCount, this.CardData.Count, Log.SkippedCards, MainWindow.TestOutputHelper);
    }

    /// <summary>
    /// Gets the highest passcode number from all serialized cards
    /// </summary>
    /// <returns>The highest passcode number</returns>
    private int GetHighestPasscode()
    {
        const int customPasscode = 900000000; // Original passcodes are 8-digit numbers, this purposely starts with 9-digits, to not interfere with any of the original ones

        var passcode = this.DeserializedCards.MaxBy(card => card.Passcode)?.Passcode ?? 0;

        if (passcode < customPasscode)
        {
            return customPasscode;
        }

        return passcode + 1;
    }
    
    /// <summary>
    /// Gets the image of a card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetImage(string line, CardData cardData)
    {
        if (cardData.ImageEndpoint != null) return;
        
        var cardImageMatch = this.cardImageRegex.Match(line);

        if (cardImageMatch.Success)
        {
            var startIndex = cardImageMatch.Value.IndexOf("https", StringComparison.Ordinal);
            var cardImageUri = cardImageMatch.Value[startIndex..];

            cardData.ImageEndpoint = cardImageUri.Replace(ImageBaseUri, string.Empty);
        }
    }
    
    /// <summary>
    /// Gets the name for every <see cref="Enums.Language"/> of the card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetName(string line, CardData cardData)
    {
        if (cardData.CardNames.Count == languageCount) return;
        
        Match? cardNameMatch = null;

        if (this.englishCardNameRegex.IsMatch(line))
        {
            cardNameMatch = this.englishCardNameRegex.Match(line);
        }
        else if (this.cardNameRegex.IsMatch(line))
        {
            cardNameMatch = this.cardNameRegex.Match(line);
        }

        if (cardNameMatch != null)
        {
            var startIndex = cardNameMatch.Value.IndexOf('>') + 1;
            var length = cardNameMatch.Value.LastIndexOf('<') - startIndex;
            var cardName = cardNameMatch.Value.Substring(startIndex, length);
            
            foreach (var language in Enum.GetValues<Language>())
            {
                if (cardNameMatch.Value.Contains($"lang=\"{language}\""))
                {
                    cardData.CardNames.TryAdd(language, cardName);
                    
                    return;
                }
            }

            cardData.CardNames.TryAdd(Language.en, cardName);
        }
    }

    /// <summary>
    /// Gets the type of the card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetCardType(string line, CardData cardData)
    {
        if (cardData.CardType != null) return;
        
        var cardTypeMatch = this.cardTypeRegex.Match(line);

        if (cardTypeMatch.Success)
        {
            var startIndex = cardTypeMatch.Value.IndexOf('>') + 1;
            var length = cardTypeMatch.Value.LastIndexOf('<') - startIndex;
            var cardTypeString = cardTypeMatch.Value.Substring(startIndex, length);

            if (string.Equals(cardTypeString, nameof(Monster)))
            {
                cardData.CardType = typeof(Monster);
            }
            else if (string.Equals(cardTypeString, nameof(Spell)))
            {
                cardData.CardType = typeof(Spell);
            }
            else if (string.Equals(cardTypeString, nameof(Trap)))
            {
                cardData.CardType = typeof(Trap);
            }
            else
            {
                Log.Error(Error.TypeError, cardData.Endpoint, cardTypeMatch.Value);
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="Attribute"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetAttribute(string line, CardData cardData)
    {
        if (cardData.CardType != typeof(Monster) || cardData.Attribute != null) return;
        
        if (this.attributeRegex.IsMatch(line))
        {
            cardData.AttributeFound = true;
        }
        else
        {
            if (cardData.AttributeFound)
            {
                cardData.AttributeFound = false;
                
                var startIndex = line.IndexOf('>') + 1;
                var length = line.IndexOf("</", StringComparison.Ordinal) - startIndex;
                var attributeString = line.Substring(startIndex, length);

                foreach (var attributeType in Enum.GetValues<Attribute>())
                {
                    if (string.Equals(attributeType.ToString(), attributeString))
                    {
                        cardData.Attribute = attributeType;
                    
                        return;
                    }
                }
            
                Log.Error(Error.AttributeError, cardData.Endpoint, line);  
            }
        }
    }

    /// <summary>
    /// Gets every <see cref="MonsterType"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetMonsterTypes(string line, CardData cardData)
    {
        if (cardData.CardType != typeof(Monster) || cardData.MonsterTypes.Any()) return;
        
        if (this.monsterTypesRegex.IsMatch(line))
        {
            cardData.MonsterTypeFound = true;
        }
        else
        {
            if (cardData.MonsterTypeFound)
            {
                cardData.MonsterTypeFound = false;
                
                var typeMatches = this.typeSplitRegex.Matches(line);

                foreach (var typeMatch in typeMatches.Cast<Match>())
                {
                    var startIndex = typeMatch.Value.IndexOf('>') + 1;
                    var length = typeMatch.Value.LastIndexOf('<') - startIndex;
                    var monsterTypeString = typeMatch.Value.Substring(startIndex, length);
                    
                    foreach (var kvp in Translator.MonsterTypes[Language.en])
                    {
                        if (string.Equals(kvp.Value, monsterTypeString))
                        {
                            cardData.MonsterTypes.Add(kvp.Key);
                            
                            goto SKIP_ERROR;
                        }
                    }

                    Log.Error(Error.MonsterTypeError, cardData.Endpoint, typeMatch.Value);
                    
                    SKIP_ERROR:;
                }
            }
        }
    }
    
    /// <summary>
    /// Gets the level/rank of a <see cref="Monster"/> card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetMonsterLevel(string line, CardData cardData)
    {
        if (cardData.CardType != typeof(Monster) || cardData.MonsterLevel != null) return;
        
        var monsterLevelMatch = this.monsterLevelRegex.Match(line);

        if (monsterLevelMatch.Success)
        {
            var startIndex = monsterLevelMatch.Value.IndexOf('>') + 1;
            var length = monsterLevelMatch.Value.LastIndexOf('<') - startIndex;
            var monsterLevelString = monsterLevelMatch.Value.Substring(startIndex, length);

            if (int.TryParse(monsterLevelString, out var number))
            {
                cardData.MonsterLevel = number;
            }
            else
            {
                Log.Error(Error.MonsterLevelError, cardData.Endpoint, monsterLevelMatch.Value);
            }
        }
    }

    /// <summary>
    /// Gets the pendulum scale of a <see cref="Monster"/> card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetPendulumScale(string line, CardData cardData)
    {
        if (cardData.CardType != typeof(Monster) || cardData.PendulumScale != null) return;
        
        var pendulumScaleMatch = this.pendulumScaleRegex.Match(line);

        if (pendulumScaleMatch.Success)
        {
            var startIndex = pendulumScaleMatch.Value.IndexOf('>') + 1;
            var length = pendulumScaleMatch.Value.LastIndexOf('<') - startIndex;
            var pendulumScaleString = pendulumScaleMatch.Value.Substring(startIndex, length);

            if (int.TryParse(pendulumScaleString, out var number))
            {
                cardData.PendulumScale = number;
            }
            else
            {
                Log.Error(Error.PendulumScaleError, cardData.Endpoint, pendulumScaleMatch.Value);
            }
        }
    }

    /// <summary>
    /// Gets every <see cref="LinkArrow"/> of a <see cref="Monster"/> card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetLinkArrows(string line, CardData cardData)
    {
        if (cardData.CardType != typeof(Monster) || cardData.LinkArrows.Any()) return;
        
        var linkArrowMatches = this.linkArrowRegex.Matches(line);

        if (linkArrowMatches.Any())
        {
            foreach (var linkArrowMatch in linkArrowMatches.Cast<Match>())
            {
                var startIndex = linkArrowMatch.Value.IndexOf('>') + 1;
                var length = linkArrowMatch.Value.LastIndexOf('<') - startIndex;
                var linkArrowString = linkArrowMatch.Value.Substring(startIndex, length);
                                    
                foreach (var kvp in Translator.LinkArrows[Language.en])
                {
                    if (string.Equals(kvp.Value, linkArrowString))
                    {
                        cardData.LinkArrows.Add(kvp.Key);
                            
                        goto SKIP_ERROR;
                    }
                }

                Log.Error(Error.LinkArrowError, cardData.Endpoint, linkArrowMatch.Value);
                    
                SKIP_ERROR:;
            }
        }
    }

    /// <summary>
    /// Gets the stats of a <see cref="Monster"/> card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetMonsterStats(string line, CardData cardData)
    {
        if (cardData.CardType != typeof(Monster) || cardData.Attack != null || cardData.Defense != null) return;
        
        var monsterCardStatsMatch = this.monsterCardStatsRegex.Match(line);

        if (monsterCardStatsMatch.Success)
        {
            var isLinkMonster = this.linkRatingRegex.IsMatch(monsterCardStatsMatch.Value);
            var stats = monsterCardStatsMatch.Value.Split(" / ");
            
            for (var i = 0; i < stats.Length; i++)
            {
                var startIndex = stats[i].IndexOf('>') + 1;
                var length = stats[i].NthIndexOf('<', 2) - startIndex;
                var statValueString = stats[i].Substring(startIndex, length);
                var statValue = 0;

                if (string.Equals(statValueString, "?"))
                {
                    statValue = -1;
                }
                else if (int.TryParse(statValueString, out var number))
                {
                    statValue = number;
                }
                else
                {
                    Log.Error(Error.MonsterStatsError, cardData.Endpoint, monsterCardStatsMatch.Value);
                }
                
                if (i == 0)
                {
                    cardData.Attack = statValue;
                }
                else
                {
                    if (isLinkMonster)
                    {
                        cardData.MonsterLevel = statValue;
                        cardData.Defense = 0;
                    }
                    else
                    {
                        cardData.Defense = statValue;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="PropertyType"/> of a <see cref="Spell"/>/<see cref="Trap"/> card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetPropertyType(string line, CardData cardData)
    {
        if (cardData.CardType != typeof(Spell) && cardData.CardType != typeof(Trap) || cardData.PropertyType != null) return;
        
        var propertyTypeMatch = this.propertyTypeRegex.Match(line);

        if (propertyTypeMatch.Success)
        {
            var startIndex = propertyTypeMatch.Value.IndexOf('>') + 1;
            var length = propertyTypeMatch.Value.LastIndexOf('<') - startIndex;
            var propertyTypeString = propertyTypeMatch.Value.Substring(startIndex, length);
            
            foreach (var kvp in Translator.PropertyTypes[Language.en])
            {
                if (string.Equals(kvp.Value, propertyTypeString))
                {
                    cardData.PropertyType = kvp.Key;
                            
                    goto SKIP_ERROR;
                }
            }
            
            Log.Error(Error.PropertyTypeError, cardData.Endpoint, propertyTypeMatch.Value);
                    
            SKIP_ERROR:;
        }
    }

    /// <summary>
    /// Gets the Passcode of a card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetPassCode(string line, CardData cardData)
    {
        if (cardData.Passcode != null) return;

        if (this.passcodeReached.IsMatch(line))
        {
            cardData.PasscodeReached = true;
        }

        if (cardData.PasscodeReached)
        {
            var passCodeMatch = this.passcodeRegex.Match(line);

            if (passCodeMatch.Success)
            {
                var startIndex = passCodeMatch.Value.IndexOf('>') + 1;
                var length = passCodeMatch.Value.IndexOf('<') - startIndex;
                var passCodeString = passCodeMatch.Value.Substring(startIndex, length).Replace(" ", string.Empty);

                if (int.TryParse(passCodeString, out var number))
                {
                    cardData.Passcode = number;
                }
                else
                {
                    Log.Error(Error.PasscodeError, cardData.Endpoint, passCodeMatch.Value);
                }
            }
        }
    }
    
    /// <summary>
    /// Gets all statuses of a card
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetStatuses(string line, CardData cardData)
    {
        if (cardData.DescriptionFound) return;
        
        var cardStatusMatches = this.cardStatusRegex.Matches(line);
        
        if (cardStatusMatches.Any())
        {
            foreach (var cardStatusMatch in cardStatusMatches.Cast<Match>())
            {
                var startIndex = cardStatusMatch.Value.NthIndexOfLast('>', 2) + 1;
                var length = cardStatusMatch.Value.LastIndexOf('<') - startIndex;
                var statusString = cardStatusMatch.Value.Substring(startIndex, length);
                
                foreach (var kvp in Translator.Statuses[Language.en])
                {
                    if (string.Equals(kvp.Value, statusString))
                    {
                        if (!cardData.Statuses.Contains(kvp.Key))
                        {
                            cardData.Statuses.Add(kvp.Key);   
                        }

                        goto SKIP_ERROR;
                    }
                }
            
                Log.Error(Error.StatusError, cardData.Endpoint, cardStatusMatch.Value);
                    
                SKIP_ERROR:;
            }
        }
    }

    /// <summary>
    /// Gets the card description for every <see cref="Enums.Language"/>
    /// </summary>
    /// <param name="line">Line that is currently being read</param>
    /// <param name="cardData">Contains all data of a card</param>
    private void GetDescriptions(string line, CardData cardData)
    {
        if (cardData.AllDescriptionsFound) return;
        
        if (line.Contains("<b>Card descriptions</b>"))
        {
            cardData.DescriptionFound = true;
        }
        else
        {
            if (cardData.DescriptionFound)
            {
                if (this.descriptionLanguageRegex.IsMatch(line))
                {
                    const string div = "</div>";
                    var startIndex = line.NthOccurenceOfLast(div, 2) + div.Length;
                    var length = line.NthOccurenceOfLast(div, 1) - startIndex;
                    var languageString = line.Substring(startIndex, length);

                    foreach (var kvp in Translator.Languages)
                    {
                        if (string.Equals(kvp.Value, languageString))
                        {
                            cardData.Descriptions.TryAdd(kvp.Key, string.Empty);
                            break;
                        }
                    }
                }
                else if (this.cardDescriptionRegex.IsMatch(line))
                {
                    cardData.NextIsDescription = true;
                }
                else if (cardData.NextIsDescription)
                {
                    cardData.NextIsDescription = false;

                    var language = cardData.Descriptions.Last().Key;
                    cardData.Descriptions[language] = this.htmlBracketsRegex.Replace(line, string.Empty);
                    
                    if (cardData.Descriptions.Count == languageCount)
                    {
                        cardData.AllDescriptionsFound = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks if error occured during the data search
    /// </summary>
    /// <param name="response">Response from the http request</param>
    /// <param name="cardData">Contains all data of a card</param>
    /// <returns>True == skip this card</returns>
    private static bool CheckForErrors(HttpStatusCode response, CardData cardData)
    {
        if (response == HttpStatusCode.BadRequest)
        {
            Log.SkipCard(Skip.BadRequest, cardData.Endpoint); // TODO: Print/Log skipped cards
            return true;
        }
        
        if (!cardData.CardNames.Any())
        {
            Log.Error(Error.NameError, cardData.Endpoint, cardData.CardNames.Count.ToString());
        }
        
        if (cardData.ImageEndpoint.IsNullEmptyOrWhitespace())
        {
            Log.Error(Error.ImageError, cardData.Endpoint, cardData.ImageEndpoint ?? "null");
        }
        
        if (!cardData.Descriptions.Any())
        {
            Log.Error(Error.DescriptionError, cardData.Endpoint, cardData.Descriptions.Count.ToString());
        }
        
        if (!cardData.StopSearch)
        {
            Log.Error(Error.StopConditionError, cardData.Endpoint, cardData.StopSearch.ToString());
        }

        return false;
    }
    #endregion
}