using System;
using System.IO;

#pragma warning disable CS8618

namespace YuGiOh_DeckBuilder.Utility;

/// <summary>
/// Wrapper class for a <see cref="Stream"/> that must be open, while <see cref="Object"/> is being used <br/>
/// <i>Closed the <see cref="Stream"/> automatically when this <see cref="StreamWrapper{T}"/> instance is not used anymore/goes out of scope</i>
/// </summary>
/// <typeparam name="T"></typeparam>
internal class StreamWrapper<T> : IDisposable
{
    #region Properties
    /// <summary>
    /// The <see cref="System.IO.Stream"/> that must be open for the lifetime of <see cref="object"/>
    /// </summary>
    internal Stream Stream { get; init; }
    /// <summary>
    /// An <see cref="object"/> that needs <see cref="Stream"/> to be open
    /// </summary>
    internal T Object { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Assigns a new <see cref="MemoryStream"/> to <see cref="Stream"/> 
    /// </summary>
    internal StreamWrapper()
    {
        this.Stream = new MemoryStream();
    }

    /// <summary>
    /// Assigns the given <see cref="System.IO.Stream"/> to <see cref="Stream"/>
    /// </summary>
    /// <param name="stream">The <see cref="System.IO.Stream"/> to assign to <see cref="Stream"/></param>
    internal StreamWrapper(Stream stream)
    {
        this.Stream = stream;
    }
    
    /// <summary>
    /// Assigns the given <see cref="System.IO.Stream"/> to <see cref="Stream"/> and <see cref="object"/> to <see cref="Object"/>
    /// </summary>
    /// <param name="stream">The <see cref="System.IO.Stream"/> to assign to <see cref="Stream"/></param>
    /// <param name="object">An <see cref="object"/> that needs <see cref="Stream"/> to be open</param>
    internal StreamWrapper(Stream stream, T @object)
    {
        this.Stream = stream;
        this.Object = @object;
    }

    /// <summary>
    /// Calls  <see cref="System.IO.Stream"/>. <see cref="System.IO.Stream.Close()"/> when this <see cref="StreamWrapper{T}"/> is being disposed
    /// </summary>
    ~StreamWrapper()
    {
        Dispose(false);
    }
    #endregion

    #region Methods
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            Stream.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}