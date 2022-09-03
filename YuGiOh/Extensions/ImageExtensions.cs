using System.Drawing;
using System.Windows.Media.Imaging;

namespace YuGiOh_DeckBuilder.Extensions;

/// <summary>
/// Contains extension methods for <see cref="Image"/> instances
/// </summary>
internal static class ImageExtensions
{
    #region Methods
    /// <summary>
    /// Converts an <see cref="Image"/> into a WPF <see cref="BitmapSource"/>.
    /// </summary>
    /// <param name="source">The source <see cref="Image"/></param>
    /// <returns>A <see cref="BitmapSource"/></returns>
    internal static BitmapSource? ToBitmapSource(this Image source)
    {
        var bitmap = new Bitmap(source);
        var bitmapSource = bitmap.ToBitmapSource();

        bitmap.Dispose();

        return bitmapSource;
    }
    #endregion
}