using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;

namespace YuGiOh_DeckBuilder.Utility.Json.Cards;

/// <summary>
/// <see cref="DefaultContractResolver"/> for <see cref="ACard"/>
/// </summary>
internal class CardContractResolver : DefaultContractResolver
{
    #region Methods
    protected override JsonConverter? ResolveContractConverter(Type objectType)
    {
        // Prevents stack overflow
        if (typeof(ACard).IsAssignableFrom(objectType) && !objectType.IsAbstract)
        {
            return null;
        }
        return base.ResolveContractConverter(objectType);
    }
    #endregion
}