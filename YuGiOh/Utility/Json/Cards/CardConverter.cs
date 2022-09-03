using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;
using YuGiOh_DeckBuilder.YuGiOh.Enums;

namespace YuGiOh_DeckBuilder.Utility.Json.Cards;

/// <summary>
/// <see cref="JsonConverter"/> for <see cref="ACard"/>
/// </summary>
internal class CardConverter : JsonConverter
{
    #region Statics
    /// <summary>
    /// <see cref="JsonSerializerSettings"/>
    /// </summary>
    private static readonly JsonSerializerSettings jsonSerializerSettings = new()
    {
        ContractResolver = new CardContractResolver()
    };
    #endregion
    
    #region Properties
    public override bool CanWrite => false;
    #endregion
    
    #region Methods
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException(); // Won't be called, because "CanWrite" returns false
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);
        var cardType = jObject[nameof(ACard.CardType)]!.Value<int>();

        return cardType switch
        {
            (int)CardType.Monster => JsonConvert.DeserializeObject<Monster>(jObject.ToString(), jsonSerializerSettings),
            (int)CardType.Spell => JsonConvert.DeserializeObject<Spell>(jObject.ToString(), jsonSerializerSettings),
            (int)CardType.Trap => JsonConvert.DeserializeObject<Trap>(jObject.ToString(), jsonSerializerSettings),
            _ => throw new ArgumentOutOfRangeException($"The given {nameof(cardType)}: [{cardType}], is not a valid value for {nameof(CardType)}")
        };
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ACard);
    }
    #endregion
}