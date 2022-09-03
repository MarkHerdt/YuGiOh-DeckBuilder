using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using YuGiOh_DeckBuilder.Utility;

namespace YuGiOh_DeckBuilder.Extensions;

/// <summary>
/// Contains extension methods for <see cref="Bitmap"/> instances
/// </summary>
internal static class BitmapExtensions
{
    #region Methods
    /// <summary>
    /// Converts a <see cref="Bitmap"/> into a WPF <see cref="BitmapSource"/>.
    /// </summary>
    /// <param name="source">The source <see cref="Bitmap"/></param>
    /// <returns>A <see cref="BitmapSource"/></returns>
    /// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.</remarks>
    internal static BitmapSource? ToBitmapSource(this Bitmap source)
    {
        BitmapSource? bitmapSource;

        var hBitmap = source.GetHbitmap();

        try
        {
            bitmapSource = Imaging.CreateBitmapSourceFromHBitmap
            (
                bitmap:      hBitmap,
                palette:     IntPtr.Zero,
                sourceRect:  Int32Rect.Empty,
                sizeOptions: BitmapSizeOptions.FromEmptyOptions()
            );
        }
        catch (Win32Exception)
        {
            bitmapSource = null;
        }
        finally
        {
            ExternalMethods.DeleteObject(hBitmap);
        }

        return bitmapSource;
    }
    #endregion
}