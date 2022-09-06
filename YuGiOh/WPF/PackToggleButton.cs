using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using YuGiOh_DeckBuilder.YuGiOh;
using YuGiOh_DeckBuilder.YuGiOh.Decks.Packs;

namespace YuGiOh_DeckBuilder.WPF;

/// <summary>
/// <see cref="FilterSettings"/> for a <see cref="Pack"/>
/// </summary>
internal class PackToggleButton : ToggleButton
{
    #region Properties
    /// <summary>
    /// Index of a <see cref="Pack"/> in <see cref="MainWindow.Packs"/>
    /// </summary>
    internal int Index { get; }
    /// <summary>
    /// The date when this <see cref="Pack"/> was released
    /// </summary>
    internal DateTime ReleaseDate { get; }
    #endregion

    #region Constrcutor
    /// <param name="index">Index of a <see cref="Pack"/> in <see cref="MainWindow.Packs"/></param>
    internal PackToggleButton(int index)
    {
        var pack = MainWindow.Packs![index];
        this.Index = index;
        this.ReleaseDate = pack.ReleaseDate;
        this.Content = $"{pack.Name} [{pack.ReleaseDate:dd MMMM yyyy}]";

        base.HorizontalContentAlignment = HorizontalAlignment.Right;
        base.FontWeight = FontWeights.Medium;

        FilterSettingsWindow.OnSizeChanged += OnParentSizeChanged;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Sets the <see cref="ToggleButton.Width"/> of this <see cref="PackToggleButton"/> when the parents size changes
    /// </summary>
    /// <param name="newWidth">The new width of the parent object</param>
    private void OnParentSizeChanged(double newWidth)
    {
        base.Width = newWidth - 15;
    }
    #endregion
}