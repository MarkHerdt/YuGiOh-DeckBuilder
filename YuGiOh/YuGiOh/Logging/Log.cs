using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.Utility.Project;

namespace YuGiOh_DeckBuilder.YuGiOh.Logging;

/// <summary>
/// Stores logging information
/// </summary>
internal static class Log
{
    #region Members
    /// <summary>
    /// Error logs
    /// </summary>
    internal static readonly ConcurrentBag<string> Errors = new();
    /// <summary>
    /// Known errors that shouldn't raise an alarm
    /// </summary>
    private static readonly List<string> knownErrors = new()
    {
        "Card Endpoint Error | Dawn_of_Majesty_%2B1_Bonus_Pack | <td>\"<span class=\"new\" title=\"Live☆Twin Trouble-Sun (page does not exist)\" data-uncrawlable-url=\"L3dpa2kvTGl2ZSVFMiU5OCU4NlR3aW5fVHJvdWJsZS1TdW4/YWN0aW9uPWVkaXQmcmVkbGluaz0x\">Live☆Twin Trouble-Sun</span>\"</td>",
        "Card Endpoint Error | Photon_Hypernova | <td></td>",
        "Card Endpoint Error | Speed_Duel:_Trials_of_the_Kingdom | <td>\"<span class=\"new\" title=\"Dino Destruction (Skill Card) (page does not exist)\" data-uncrawlable-url=\"L3dpa2kvRGlub19EZXN0cnVjdGlvbl8oU2tpbGxfQ2FyZCk/YWN0aW9uPWVkaXQmcmVkbGluaz0x\">Dino Destruction</span>\"</td>",
        "Card Endpoint Error | Speed_Duel:_Trials_of_the_Kingdom | <td>\"<span class=\"new\" title=\"Switcheroo (Skill Card) (page does not exist)\" data-uncrawlable-url=\"L3dpa2kvU3dpdGNoZXJvb18oU2tpbGxfQ2FyZCk/YWN0aW9uPWVkaXQmcmVkbGluaz0x\">Switcheroo</span>\"</td>",
        "Card Rarity Error | King%27s_Court | <td></td>",
        "Card Rarity Error | Photon_Hypernova | <td></td>",
        "Card Table Match Error | Photon_Hypernova_%2B1_Bonus_Pack | False",
        "Card Table Match Error | Cyberstorm_Access | False",
        "Card Table Match Error | Cyberstorm_Access_%2B1_Bonus_Pack | False",
        "Description Error |  | 0",
        "Http Request Error | https://static.wikia.nocookie.net/yugioh/images/MISSING | Response status code does not indicate success: 404 (Not Found).",
        "Image Error |  | null",
        "Image Error | Alien_Stealthbuster | null",
        "Image Error | Double_Disrupter_Dragon | null",
        "Image Error | Geminize_Lord_Golknight | null",
        "Image Error | Giant_Starfall | null",
        "Image Error | Magical_Cylinders | null",
        "Image Error | Magikey_Locking | null",
        "Image Error | Meowseclick | null",
        "Image Error | Mimicking_Man-Eater_Bug | null",
        "Image Error | Outstanding_Dog_Mary | null",
        "Image Error | Stained_Glass_of_Light_and_Dark | null",
        "Image Error | Undaunted_Bumpkin_Beast | null",
        "Name Error |  | 0"
    };

    /// <summary>
    /// Packs that have been skipped during the download <br/>
    /// <b>Key: </b> Reason, why the pack was skipped <br/>
    /// <b>Value: </b> Endpoint of the packs that were skipped
    /// </summary>
    internal static ConcurrentDictionary<Skip, ICollection<string>> SkippedPacks { get; } = new();
    /// <summary>
    /// Cards that have been skipped during the download
    /// <b>Key: </b> Reason, why the card was skipped <br/>
    /// <b>Value: </b> Endpoint of the cards that were skipped
    /// </summary>
    internal static ConcurrentDictionary<Skip, ICollection<string>> SkippedCards { get; } = new();
    #endregion

    #region Constrcutor
    static Log()
    {
        using var fileStream = File.OpenRead(Structure.BuildPath(Folder.Logging, "KnownErrors", Extension.csv));
        using var streamReader = new StreamReader(fileStream);
        
        while (streamReader.ReadLineAsync().Result is { } currentLine)
        {
            knownErrors.Add(currentLine);
        }
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// Add the given <see cref="Logging.Error"/> to <see cref="Errors"/>, when it's not already in <see cref="knownErrors"/>
    /// </summary>
    /// <param name="error"><see cref="Logging.Error"/> that was thrown</param>
    /// <param name="endPoint">Endpoint on which the error occured</param>
    /// <param name="trigger">The <see cref="string"/> that lead to the <see cref="Logging.Error"/></param>
    internal static void Error(Error error, string endPoint, string trigger)
    {
        var stringBuilder = MainWindow.StringBuilderPool.GetObject();

        stringBuilder.Item.Append(error.ToString().CamelCaseSpacing());
        stringBuilder.Item.Append(" | ");
        stringBuilder.Item.Append(endPoint);
        stringBuilder.Item.Append(" | ");
        stringBuilder.Item.Append(trigger);

        if (knownErrors.Any(knownError => knownError.Contains(stringBuilder.Item.ToString())))
        {
            return;
        }
        
        Errors.Add(stringBuilder.Item.ToString());
        
        stringBuilder.Return();
    }

    /// <summary>
    /// Adds a log entry for a skipped card to <see cref="SkippedPacks"/>
    /// </summary>
    /// <param name="reason">The reason, why this endpoint was skipped</param>
    /// <param name="endpoint">The endpoint that was skipped</param>
    internal static void SkipPack(Skip reason, string endpoint)
    {
        Skip(reason, endpoint, SkippedPacks);
    }

    /// <summary>
    /// Adds a log entry for a skipped card to <see cref="SkippedCards"/>
    /// </summary>
    /// <param name="reason">The reason, why this endpoint was skipped</param>
    /// <param name="endpoint">The endpoint that was skipped</param>
    internal static void SkipCard(Skip reason, string endpoint)
    {
        Skip(reason, endpoint, SkippedCards);
    }
    
    /// <summary>
    /// Adds a log entry for the given endpoint with the given reason to the given <see cref="ConcurrentDictionary{TKey,TValue}"/>
    /// </summary>
    /// <param name="reason">The reason, why this endpoint was skipped</param>
    /// <param name="endpoint">The endpoint that was skipped</param>
    /// <param name="skipped">The <see cref="ConcurrentDictionary{TKey,TValue}"/> to add the log to</param>
    private static void Skip(Skip reason, string endpoint, ConcurrentDictionary<Skip, ICollection<string>> skipped)
    {
        if (skipped.ContainsKey(reason))
        {
            skipped[reason].Add(endpoint);
        }
        else
        {
            skipped.TryAdd(reason, new List<string> { endpoint });   
        }
    }
    #endregion
}