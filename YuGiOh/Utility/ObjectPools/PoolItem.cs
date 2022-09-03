namespace YuGiOh_DeckBuilder.Utility.ObjectPools;

/// <summary>
/// Container for <see cref="AObjectPool{T}"/> items
/// </summary>
/// <typeparam name="T"></typeparam>
internal class PoolItem<T> where T : new()
{
    #region Members
    /// <summary>
    /// Reference to the <see cref="AObjectPool{T}"/>, this <see cref="PoolItem{T}"/> belongs to
    /// </summary>
    private readonly AObjectPool<T> aObjectPool;
    #endregion

    #region Properties
    /// <summary>
    /// The actual item in the <see cref="AObjectPool{T}"/>
    /// </summary>
    internal T Item { get; }
    #endregion
    
    #region Constructor
    /// <summary>
    /// Creates a new <see cref="PoolItem{T}"/>
    /// </summary>
    /// <param name="aObjectPool">Reference to the <see cref="AObjectPool{T}"/>, this <see cref="PoolItem{T}"/> belongs to</param>
    /// <param name="item">The actual item in the <see cref="AObjectPool{T}"/></param>
    internal PoolItem(AObjectPool<T> aObjectPool, T item)
    {
        this.aObjectPool = aObjectPool;
        this.Item = item;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Returns this <see cref="PoolItem{T}"/> to its <see cref="aObjectPool"/>
    /// </summary>
    internal void Return()
    {
        this.aObjectPool.ReturnItem(this);
    }
    #endregion
}