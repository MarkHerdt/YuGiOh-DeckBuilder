using System.IO;
using Newtonsoft.Json;

namespace YuGiOh_DeckBuilder.Utility.Json;

/// <summary>
/// Custom <see cref="JsonTextWriter"/> that serializes arrays in one line
/// </summary>
internal class CustomJsonWriter : JsonTextWriter
{
    #region Constructor
    /// <param name="writer">Use any writer you want</param>
    internal CustomJsonWriter(TextWriter writer) : base(writer) { }
    #endregion

    #region Methods
    protected override void WriteIndent()
    {
        if (WriteState != WriteState.Array)
            base.WriteIndent();
        else
            WriteIndentSpace();
    }
    #endregion
}