using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;

namespace YuGiOh_DeckBuilder.YuGiOh;

internal sealed partial class YuGiOhFandom
{
    #region Methods
    /// <summary>
    /// Removes every <see cref="Pack"/> from <see cref="PackData"/>, that is already serialized at <see cref="Folder"/>.<see cref="Folder.Packs"/>
    /// </summary>
    private async Task UpdateAsync()
    {
        await Task.Run(() =>
        {
            var updatedPackData = new List<PackData>();

            foreach (var deserializedPack in this.DeserializedPacks)
            {
                if (deserializedPack.Cards.Any(card => card.Passcode < 1))
                {
                    updatedPackData.Add(new PackData(deserializedPack.Endpoint)
                    {
                        Cards = deserializedPack.Cards.ToList()
                    });
                }
            }
        
            foreach (var packData in this.PackData)
            {
                if (this.DeserializedPacks.All(deserializedPack => deserializedPack.Endpoint != packData.Endpoint))
                {
                    updatedPackData.Add(packData);
                }
            }

            this.PackData = updatedPackData;
        });
    }
    #endregion
}