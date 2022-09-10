namespace YuGiOh_DeckBuilder.YuGiOh.Logging;

/// <summary>
/// All possible error
/// </summary>
internal enum Error
{
    HttpRequestError,
    PackNameError,
    PackReleaseDateError,
    PackStopConditionError,
    CardTableMatchError,
    CardEndpointError,
    CardRarityError,
    CardColumnMatchError,
    CardHeaderMatchError,
    CardColumnNumberError,
    ImageError,
    NameError,
    TypeError,
    AttributeError,
    MonsterTypeError,
    MonsterLevelError,
    PendulumScaleError,
    LinkArrowError,
    MonsterStatsError,
    PropertyTypeError,
    PasscodeError,
    StatusError,
    DescriptionError,
    StopConditionError,
    PackCardPasscodeError
}