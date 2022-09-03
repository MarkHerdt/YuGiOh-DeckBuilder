using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using YuGiOh_DeckBuilder.Utility;
using YuGiOh_DeckBuilder.Utility.Images;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.YuGiOh;
using YuGiOh_DeckBuilder.YuGiOh.Logging;

namespace YuGiOh_DeckBuilder.Web;

/// <summary>
/// Contains methods to download the content of websites
/// </summary>
internal static class WebClient
{
    #region Members
    /// <summary>
    /// <see cref="HttpClient"/>
    /// </summary>
    private static readonly HttpClient client = new();
    #endregion
    
    #region Methods
    /// <summary>
    /// Asynchronously reads the content of a website
    /// </summary>
    /// <param name="requestUri">The uri of the website to download</param>
    /// <param name="line">The line that is currently being read</param>
    /// <param name="stopCondition">Stops the reading when this condition is true</param>
    /// <returns><see cref="HttpStatusCode.BadRequest"/> when the website couldn't be reached, otherwise <see cref="HttpStatusCode.OK"/></returns>
    internal static async Task<HttpStatusCode> ReadWebsiteAsStreamAsync(string requestUri, Action<string> line, Func<string, bool> stopCondition)
    {
        await using var websiteStream = await DownloadStreamAsync(requestUri);

        if (websiteStream == null)
        {
            return HttpStatusCode.BadRequest;
        }
        
        using var streamReader = new StreamReader(websiteStream);
        
        while ((await streamReader.ReadLineAsync())! is { } currentLine)
        {
            line(currentLine);
            
            if (stopCondition(currentLine))
            {
                break;
            }
        }

        return HttpStatusCode.OK;
    }
    
    /// <summary>
    /// Downloads the content of [https://yugioh.fandom.com/wiki/ + endpoint]
    /// </summary>
    /// <param name="endpoint">The endpoint of the site to download</param>
    /// <param name="folder">Folder to get the path to</param>
    /// <param name="fileName">Name of the file, will be set to endPoint if empty</param>
    /// <param name="fileExtension">The extension of the file</param>
    internal static async Task DownloadWebsiteAsync(string endpoint, Folder folder, string fileName = "", Extension fileExtension = Extension.txt)
    {
        fileName = fileName == string.Empty ? endpoint : fileName;
        
        var requestUri = YuGiOhFandom.BaseUri + endpoint;
        var filePath = Structure.BuildPath(folder, fileName, fileExtension);
        
        await CopyWebsiteAsync(requestUri, filePath);
    }
    
    /// <summary>
    /// Downloads an <see cref="Image"/> from the given website and compresses it to the given quality
    /// </summary>
    /// <param name="requestUri">The uri of the image to download</param>
    /// <param name="imageCodecInfo"><see cref="ImageCodecInfo"/> that was used to compress the <see cref="Image"/></param>
    /// <param name="encoderParameters"><see cref="EncoderParameters"/> that were used to compress the <see cref="Image"/></param>
    /// <param name="width">Uses the given <see cref="Stream"/> <see cref="Image"/> width if null</param>
    /// <param name="height">Uses the given <see cref="Stream"/> <see cref="Image"/> height if null</param>
    /// <returns>The compressed <see cref="Image"/> in a <see cref="StreamWrapper{T}"/></returns>
    internal static async Task<StreamWrapper<Image>?> DownloadImage(string requestUri, ImageCodecInfo imageCodecInfo, EncoderParameters encoderParameters, int? width = null, int? height = null)
    {
        var bytes = await DownloadBytesAsync(requestUri);

        if (bytes != null)
        {
            await using var memoryStream = new MemoryStream(bytes);
            var image = ImageUtils.CompressImage(memoryStream, imageCodecInfo, encoderParameters, width, height);

            return image;   
        }

        return null;
    }
    
    /// <summary>
    /// Asynchronously writes the content of the given website to the file at the given path
    /// </summary>
    /// <param name="requestUri">The uri of the website to download</param>
    /// <param name="filePath">Path of the file to write the content to</param>
    private static async Task CopyWebsiteAsync(string requestUri, string filePath)
    {
        await using var websiteStream = await DownloadStreamAsync(requestUri);
        await using var fileStream = File.Open(filePath, FileMode.OpenOrCreate);

        if (websiteStream != null)
        {
            await websiteStream.CopyToAsync(fileStream);   
        }
    }
    
    /// <summary>
    /// Asynchronously downloads the content of the given uri
    /// </summary>
    /// <param name="requestUri">The uri of the website to download</param>
    /// <returns>The downloaded content as a <see cref="byte"/> <see cref="Array"/></returns>
    private static async Task<byte[]?> DownloadBytesAsync(string requestUri)
    {
        try
        {
            return await client.GetByteArrayAsync(requestUri);
        }
        catch (Exception exception)
        {
            Log.Error(Error.HttpRequestError, requestUri, exception.Message);
            
            return Task.FromResult<byte[]?>(null).Result;
        }
    }
    
    /// <summary>
    /// Asynchronously downloads the content of the given uri
    /// </summary>
    /// <param name="requestUri">The uri of the website to download</param>
    /// <returns>The downloaded content as a <see cref="Stream"/></returns>
    private static async Task<Stream?> DownloadStreamAsync(string requestUri)
    {
        try
        {
            return await client.GetStreamAsync(requestUri);
        }
        catch (Exception exception)
        {
            Log.Error(Error.HttpRequestError, requestUri, exception.Message);
            
            return Task.FromResult<Stream?>(null).Result;
        }
    }

    /// <summary>
    /// Asynchronously downloads the content of the given uri
    /// </summary>
    /// <param name="requestUri">The uri of the website to download</param>
    /// <returns>The downloaded content as a <see cref="string"/></returns>
    private static async Task<string?> DownloadStringAsync(string requestUri)
    {
        try
        {
            return await client.GetStringAsync(requestUri);
        }
        catch (Exception exception)
        {
            Log.Error(Error.HttpRequestError, requestUri, exception.Message);
            
            return Task.FromResult<string?>(null).Result;
        }
    }
    #endregion
}