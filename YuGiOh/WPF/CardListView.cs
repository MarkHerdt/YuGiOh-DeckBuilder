using System.Windows;
using System.Windows.Controls;

namespace YuGiOh_DeckBuilder.WPF;

/// <summary>
/// <see cref="ListView"/> for <see cref="CardImage"/>
/// </summary>
internal class CardListView : ListView 
{
    #region Statics
    /// <summary>
    /// A <see cref="Point"/> with <see cref="Point.X"/> and <see cref="Point.Y"/> = 0
    /// </summary>
    private static readonly Point zero = new(0, 0);
    /// <summary>
    /// Increases the <see cref="Rect.Location"/> for the starting <see cref="Point"/> of the <see cref="viewport"/> by the <see cref="CardImage.MaxHeight"/> of a <see cref="CardImage"/>
    /// </summary>
    private static readonly Point viewPortOffset = new(0, -CardImage.MaxHeight);
    /// <summary>
    /// Translates a <see cref="bool"/> to <see cref="Visibility.Hidden"/>/<see cref="Visibility.Visible"/>
    /// </summary>
    private static readonly Visibility[] visibilities = 
    {
        Visibility.Hidden,
        Visibility.Visible
    };
    #endregion

    #region Members
    /// <summary>
    /// Viewport bounds
    /// </summary>
    private Rect viewport;
    #endregion
    
    #region Constructor
    public CardListView()
    {
        this.AddHandler(ScrollViewer.ScrollChangedEvent, new RoutedEventHandler(OnScrollChanged));
    }
    
    #endregion

    #region Methods
    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);

        var renderSize = new Size(base.RenderSize.Width, base.RenderSize.Height + CardImage.MaxHeight * 2);
        
        this.viewport = new Rect(viewPortOffset, renderSize);
    }
    
    /// <summary>
    /// <see cref="ScrollViewer.ScrollChangedEvent"/>
    /// </summary>
    /// <param name="sender"><see cref="object"/> from which this method is called</param>
    /// <param name="routedEventArgs"><see cref="RoutedEventArgs"/></param>
    private async void OnScrollChanged(object? sender, RoutedEventArgs routedEventArgs)
    {
        await this.Dispatcher.InvokeAsync(() =>
        {
            foreach (CardImage item in Items)
            {
                var transform = item.TransformToAncestor(this);
                var childBounds = transform.TransformBounds(new Rect(zero, item.RenderSize));
                var intersects = this.viewport.IntersectsWith(childBounds);

                unsafe
                {
                    item.Visibility = visibilities[*(byte*)&intersects];
                }
            }
        });
    }
    #endregion
}