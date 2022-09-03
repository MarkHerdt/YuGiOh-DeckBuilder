using System;
using System.Runtime.InteropServices;

namespace YuGiOh_DeckBuilder.Utility;

/// <summary>
/// Contains external methods
/// </summary>
internal static class ExternalMethods
{
    #region Methods
    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool DeleteObject(IntPtr hObject);
    #endregion
}