using System.Text;

namespace YuGiOh_DeckBuilder.Utility.ObjectPools;

/// <summary>
/// Object pool for <see cref="StringBuilder"/>
/// </summary>
internal sealed class StringBuilderPool : AObjectPool<StringBuilder>
{
    #region Methods
    internal override void ReturnItem(PoolItem<StringBuilder> poolItem)
    {
        poolItem.Item.Clear();
        
        base.ReturnItem(poolItem);
    }
    #endregion
}