using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace YuGiOh_DeckBuilder.Utility.Images;

/// <summary>
/// Contains helper methods for <see cref="Image"/>
/// </summary>
internal static class ImageUtils
{
    /// <summary>
    /// Compresses the quality of an <see cref="Image"/> in a <see cref="Stream"/> <br/>
    /// <i><see cref="Image"/> will be .jpg</i>
    /// </summary>
    /// <param name="imageStream"><see cref="Stream"/> containing the <see cref="Image"/></param>
    /// <param name="imageCodecInfo"><see cref="ImageCodecInfo"/> that was used to compress the <see cref="Image"/></param>
    /// <param name="encoderParameters"><see cref="EncoderParameters"/> that were used to compress the <see cref="Image"/></param>
    /// <param name="width">Uses the given <see cref="Stream"/> <see cref="Image"/> width if null</param>
    /// <param name="height">Uses the given <see cref="Stream"/> <see cref="Image"/> height if null</param>
    /// <returns>
    /// The compressed <see cref="Image"/> in a <see cref="StreamWrapper{T}"/> <br/>
    /// <i>Sometimes the <see cref="Stream"/> closes for some reason and null will be returned</i>
    /// </returns>
    internal static StreamWrapper<Image> CompressImage(Stream imageStream, ImageCodecInfo imageCodecInfo, EncoderParameters encoderParameters, int? width = null, int? height = null)
    {
        using var image = Image.FromStream(imageStream);
        using Image memoryImage = new Bitmap(image, width ?? image.Width, height ?? image.Height);
        var streamWrapper = new StreamWrapper<Image>();
        
        memoryImage.Save(streamWrapper.Stream, imageCodecInfo, encoderParameters);
        
        streamWrapper.Object = Image.FromStream(streamWrapper.Stream);
        
        return streamWrapper;
    }
}