using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace YuGiOh_DeckBuilder.YuGiOh.Enums.Utility;

internal static partial class Translator
{
    #region Properties
    /// <summary>
    /// Contains <see cref="LinkArrow"/> translations for every <see cref="Language"/> <br/>
    /// https://yugioh.fandom.com/wiki/Link_Arrow
    /// </summary>
    internal static ReadOnlyDictionary<Language, IDictionary<LinkArrow, string>> LinkArrows { get; } = new
    (
        new Dictionary<Language, IDictionary<LinkArrow, string>>
        {
            {
                Language.en, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "Top-Left" },
                    { LinkArrow.Top, "Top" },
                    { LinkArrow.TopRight, "Top-Right" },
                    { LinkArrow.Left, "Left" },
                    { LinkArrow.Right, "Right" },
                    { LinkArrow.BottomLeft, "Bottom-Left" },
                    { LinkArrow.Bottom, "Bottom" },
                    { LinkArrow.BottomRight, "Bottom-Right" }
                }
            },
            {
                Language.de, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "Oben links" },
                    { LinkArrow.Top, "Oben" },
                    { LinkArrow.TopRight, "Oben Rechts" },
                    { LinkArrow.Left, "Links" },
                    { LinkArrow.Right, "Rechts" },
                    { LinkArrow.BottomLeft, "Unten Links" },
                    { LinkArrow.Bottom, "Unten" },
                    { LinkArrow.BottomRight, "Unten Rechts" }
                }
            },
            {
                Language.es, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "Arriba a la izquierda" },
                    { LinkArrow.Top, "Parte superior" },
                    { LinkArrow.TopRight, "Parte superior derecha" },
                    { LinkArrow.Left, "Izquierda" },
                    { LinkArrow.Right, "Derecha" },
                    { LinkArrow.BottomLeft, "Abajo a la izquierda" },
                    { LinkArrow.Bottom, "Abajo" },
                    { LinkArrow.BottomRight, "Abajo a la derecha" }
                }
            },
            {
                Language.fr, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "En haut à gauche" },
                    { LinkArrow.Top, "Haut" },
                    { LinkArrow.TopRight, "En haut à droite" },
                    { LinkArrow.Left, "La gauche" },
                    { LinkArrow.Right, "Droite" },
                    { LinkArrow.BottomLeft, "En bas à gauche" },
                    { LinkArrow.Bottom, "Fond" },
                    { LinkArrow.BottomRight, "En bas à droite" }
                }
            },
            {
                Language.it, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "In alto a sinistra" },
                    { LinkArrow.Top, "Superiore" },
                    { LinkArrow.TopRight, "In alto a destra" },
                    { LinkArrow.Left, "Sono partiti" },
                    { LinkArrow.Right, "Destra" },
                    { LinkArrow.BottomLeft, "In basso a sinistra" },
                    { LinkArrow.Bottom, "Parte inferiore" },
                    { LinkArrow.BottomRight, "In basso a destra" }
                }
            },
            {
                Language.ja, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "左上" },
                    { LinkArrow.Top, "上" },
                    { LinkArrow.TopRight, "右上" },
                    { LinkArrow.Left, "左" },
                    { LinkArrow.Right, "右" },
                    { LinkArrow.BottomLeft, "左下の" },
                    { LinkArrow.Bottom, "下" },
                    { LinkArrow.BottomRight, "右下" }
                }
            },
            {
                Language.ko, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "왼쪽 상단" },
                    { LinkArrow.Top, "맨 위" },
                    { LinkArrow.TopRight, "오른쪽 상단" },
                    { LinkArrow.Left, "왼쪽" },
                    { LinkArrow.Right, "오른쪽" },
                    { LinkArrow.BottomLeft, "왼쪽 아래" },
                    { LinkArrow.Bottom, "맨 아래" },
                    { LinkArrow.BottomRight, "오른쪽 아래" }
                }
            },
            {
                Language.pt, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "Superior esquerdo" },
                    { LinkArrow.Top, "Topo" },
                    { LinkArrow.TopRight, "Canto superior direito" },
                    { LinkArrow.Left, "Esquerda" },
                    { LinkArrow.Right, "Certa" },
                    { LinkArrow.BottomLeft, "Inferior esquerdo" },
                    { LinkArrow.Bottom, "Fundo" },
                    { LinkArrow.BottomRight, "Canto inferior direito" }
                }
            },
            {
                Language.zh, new Dictionary<LinkArrow, string>
                {
                    { LinkArrow.TopLeft, "左上方" },
                    { LinkArrow.Top, "最佳" },
                    { LinkArrow.TopRight, "右上" },
                    { LinkArrow.Left, "剩下" },
                    { LinkArrow.Right, "正确的" },
                    { LinkArrow.BottomLeft, "左下方" },
                    { LinkArrow.Bottom, "底部" },
                    { LinkArrow.BottomRight, "右下角" }
                }
            }
        }
    );
    #endregion
}