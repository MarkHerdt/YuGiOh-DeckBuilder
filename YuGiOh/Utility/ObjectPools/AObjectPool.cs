using System.Collections.Generic;

namespace YuGiOh_DeckBuilder.Utility.ObjectPools;

/// <summary>
/// Generic object pool
/// </summary>
/// <typeparam name="T">Must have a parameterless constructor</typeparam>
internal abstract class AObjectPool<T> where T : new()
{
    #region Members
    /// <summary>
    /// Stores all items for this object pool
    /// </summary>
    private readonly Queue<PoolItem<T>> pool;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes <see cref="pool"/> with one item
    /// </summary>
    protected AObjectPool()
    {
        this.pool = new Queue<PoolItem<T>>();
        this.pool.Enqueue(new PoolItem<T>(this, new T()));
    }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the first item from <see cref="pool"/> <br/>
    /// If <see cref="pool"/> is empty, a new item will be created
    /// </summary>
    /// <returns><see cref="PoolItem{T}"/></returns>
    internal PoolItem<T> GetObject()
    {
        if (this.pool.TryDequeue(out var item))
        {
            return item;
        }

        return new PoolItem<T>(this, new T());
    }
    
    /// <summary>
    /// Enqueues the given <see cref="PoolItem{T}"/> to <see cref="pool"/> 
    /// </summary>
    /// <param name="poolItem">The item to enqueue to <see cref="pool"/></param>
    internal virtual void ReturnItem(PoolItem<T> poolItem)
    {
        this.pool.Enqueue(poolItem);
    }

    /// <summary>
    /// Gets the number of items in <see cref="pool"/>
    /// </summary>
    /// <returns>The number of items in <see cref="pool"/></returns>
    internal int GetPoolCount()
    {
        return this.pool.Count;
    }
    #endregion
}