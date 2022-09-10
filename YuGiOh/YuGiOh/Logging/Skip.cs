namespace YuGiOh_DeckBuilder.YuGiOh.Logging;

/// <summary>
/// Reason for skipping a pack/card
/// </summary>
internal enum Skip
{
    /// <summary>
    /// Http 400 response
    /// </summary>
    BadRequest,
}