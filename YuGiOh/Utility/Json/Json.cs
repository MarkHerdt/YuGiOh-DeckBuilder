using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YuGiOh_DeckBuilder.Utility.Project;

namespace YuGiOh_DeckBuilder.Utility.Json;

/// <summary>
/// Contains helper methods for de/serialize with Newtonsoft/Json.NET
/// </summary>
internal static class Json
{
    #region Members
    /// <summary>
    /// Newtonsoft/Json.NET <see cref="JsonSerializer"/>
    /// </summary>
    private static readonly JsonSerializer jsonSerializer = new()
    {
        Formatting = Formatting.Indented,
        Culture = CultureInfo.InvariantCulture,
        //StringEscapeHandling = StringEscapeHandling.EscapeHtml
    };
    #endregion
    
    #region Methods
    /// <summary>
    /// Serializes the given <see cref="object"/> to the given <see cref="Stream"/>
    /// </summary>
    /// <param name="folder">Folder to get the path to</param>
    /// <param name="fileName">Name of the file</param>
    /// <param name="object">The <see cref="object"/> to serialize to tge given <see cref="Stream"/></param>
    internal static async Task SerializeAsync(Folder folder, string fileName, object @object)
    {
        await using var fileStream = Structure.OpenStream(folder, fileName, Extension.json, FileMode.Create);
        await using var streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
        using var customJsonWriter = new CustomJsonWriter(streamWriter);

        jsonSerializer.Serialize(customJsonWriter, @object);
    }
    
    /// <summary>
    /// Deserializes the file at the give <see cref="Folder"/>
    /// </summary>
    /// <param name="folder">Folder to get the path to</param>
    /// <param name="fileName">Name of the file</param>
    /// <typeparam name="T">The <see cref="Type"/> to deserialize to</typeparam>
    /// <returns>An object of the given <see cref="Type"/></returns>
    internal static async Task<T?> DeserializeAsync<T>(Folder folder, string fileName)
    {
        var filepath = Structure.BuildPath(folder, fileName, Extension.json);

        return await DeserializeAsync<T>(filepath);
    }

    /// <summary>
    /// Deserializes the file at the given path
    /// </summary>
    /// <param name="filePath">Filepath of the file to deserialize</param>
    /// <typeparam name="T">The <see cref="Type"/> to deserialize to</typeparam>
    /// <returns>An object of the given <see cref="Type"/></returns>
    internal static async Task<T?> DeserializeAsync<T>(string filePath)
    {
        await using var json = File.OpenRead(filePath);
        using var streamReader = new StreamReader(json, Encoding.UTF8);
        using var jsonTextReader = new JsonTextReader(streamReader);
        
        return jsonSerializer.Deserialize<T>(jsonTextReader);
    }
    #endregion
}