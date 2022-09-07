using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YuGiOh_DeckBuilder.Utility.Json;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;

namespace YuGiOh_DeckBuilder.YuGiOh;

internal sealed partial class YuGiOhFandom
{
    #region Methods
    /// <summary>
    /// Removes every <see cref="Pack"/> from <see cref="PackData"/>, that is already serialized at <see cref="Folder"/>.<see cref="Folder.Packs"/>
    /// </summary>
    private async Task UpdatePacksAsync()
    {
        var packs = new List<Pack>();
        var newPackData = new List<PackData>();

        foreach (var packPath in Directory.GetFiles(Structure.BuildPath(Folder.Packs)))
        {
            if (await Json.DeserializeAsync<Pack>(packPath) is { } pack )
            {
                packs.Add(pack);
            }
        }

        foreach (var packData in this.PackData)
        {
            if (packs.All(pack => pack.EndPoint != packData.PackEndpoint))
            {
                newPackData.Add(packData);
            }
        }

        this.PackData = newPackData;
    }

    /// <summary>
    /// Removes every <see cref="Card"/> from <see cref="PackData"/>.<see cref="Decks.Packs.PackData.Cards"/>, that is already serialized at <see cref="Folder"/>.<see cref="Folder.Cards"/>
    /// </summary>
    private async Task UpdateCardsAsync()
    {
        await Task.Run(async () =>
        {
            var cards = new List<ACard>();

            foreach (var cardPath in Directory.GetFiles(Structure.BuildPath(Folder.Cards)))
            {
                cards.Add((await Json.DeserializeAsync<ACard>(cardPath))!);
            }
        
            foreach (var packData in this.PackData)
            {
                for (var i = 0; i < packData.Cards.Count; i++)
                {
                    if (cards.AsParallel().Any(card => string.Equals(card.Endpoint, packData.Cards[i].EndPoint)))
                    {
                        packData.Cards.RemoveAt(i);
                        i--;
                    }
                }
            }
        });
    }
    #endregion
}