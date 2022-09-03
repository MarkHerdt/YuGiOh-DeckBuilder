using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using YuGiOh_DeckBuilder.Utility.Project;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Cards;

namespace YuGiOh_DeckBuilder.WPF;

/// <summary>
/// <see cref="Image"/> for <see cref="ACard"/>
/// </summary>
public sealed class CardImage : Image 
{
    #region Constatns
    /// <summary>
    /// File name of a cards default image
    /// </summary>
    private const string defaultImageName = "0";
    /// <summary>
    /// <see cref="Image.MaxWidth"/>
    /// </summary>
    private const double maxWidth = 133.33;
    /// <summary>
    /// <see cref="Image.MaxHeight"/>
    /// </summary>
    internal new const double MaxHeight = 193.67;
    #endregion
    
    #region Statics
    /// <summary>
    /// Folder path, where all card images are contained
    /// </summary>
    private static readonly string imagesPath = Structure.BuildPath(Folder.Images);
    #endregion
    
    #region Members
    /// <summary>
    /// Contains the path the the <see cref="Image"/> of this card
    /// </summary>
    private readonly Uri imagePath;
    #endregion

    #region Properties
    /// <summary>
    /// Index of a <see cref="ACard"/> in <see cref="MainWindow.Cards"/>, this <see cref="CardImage"/> should display
    /// </summary>
    internal int Index { get; }
    /// <summary>
    /// <see cref="BitmapSource"/> for placeholder card <see cref="Image"/>
    /// </summary>
    internal static readonly BitmapSource CardBack = new BitmapImage(new Uri(Structure.BuildPath(Folder.Images, defaultImageName, Extension.jpg)))
    {
        CacheOption = BitmapCacheOption.OnLoad
    };
    #endregion
    
    #region Constructor
    /// <param name="index">Index of a <see cref="ACard"/> in <see cref="MainWindow.Cards"/></param>
    internal CardImage(int index)
    {
        base.MaxWidth = maxWidth;
        base.MaxHeight = MaxHeight;
        base.Width = maxWidth;
        base.Height = MaxHeight;

        var fileExtension = Extension.jpg.ToString();
        var path = Path.Combine(imagesPath, $"{MainWindow.Cards![index].Passcode.ToString()}.{fileExtension}");

        this.Index = index;
        this.imagePath = File.Exists(path) ? new Uri(path) : new Uri(Path.Combine(imagesPath, $"{defaultImageName}.{fileExtension}"));
            
        base.Visibility = Visibility.Hidden;
        base.IsVisibleChanged += OnVisibleChanged;
        
    }
    
    /// <param name="cardImage">The <see cref="CardImage"/> to get the values from</param>
    private CardImage(CardImage cardImage)
    {
        base.MaxWidth = maxWidth;
        base.MaxHeight = MaxHeight;
        base.Width = maxWidth;
        base.Height = MaxHeight;

        this.Index = cardImage.Index;
        this.imagePath = cardImage.imagePath;

        base.Source = new BitmapImage(this.imagePath)
        { 
            CreateOptions = BitmapCreateOptions.IgnoreImageCache,
            CacheOption = BitmapCacheOption.None,
        };
        base.Visibility = Visibility.Visible;
    }
    #endregion
    
    #region Methods
    /// <summary>
    /// Gets the <see cref="ACard"/> in <see cref="MainWindow.Cards"/>, at <see cref="Index"/>
    /// </summary>
    /// <returns>The <see cref="ACard"/> in <see cref="MainWindow.Cards"/>, at <see cref="Index"/></returns>
    internal ACard GetCardData() => MainWindow.Cards![this.Index];
    
    /// <summary>
    /// Is called, when the value of <see cref="UIElement"/>.<see cref="UIElement.IsVisible"/> changes
    /// </summary>
    /// <param name="object"><see cref="object"/></param>
    /// <param name="dependencyPropertyChangedEventArgs"><see cref="DependencyPropertyChangedEventArgs"/></param>
    private void OnVisibleChanged(object @object, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        base.Source = !base.IsVisible ? CardBack : new BitmapImage(this.imagePath)
        { 
            CreateOptions = BitmapCreateOptions.IgnoreImageCache,
            CacheOption = BitmapCacheOption.None,
        };
    }
    
    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs mouseButtonEventArgs)
    {
        base.OnMouseLeftButtonDown(mouseButtonEventArgs);

        MainWindow.Instance.SelectCard(this);

        if (mouseButtonEventArgs.ClickCount == 2)
        {
            MainWindow.Instance.AddCardToDeck(new CardImage(this));   
        }
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs mouseButtonEventArgs)
    {
        base.OnMouseRightButtonDown(mouseButtonEventArgs);
        
        MainWindow.Instance.RemoveCardFromDeck(this);
    }
    #endregion
}