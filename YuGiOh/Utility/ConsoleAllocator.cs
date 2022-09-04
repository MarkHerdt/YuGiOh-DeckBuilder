using System;
using System.Runtime.InteropServices;

namespace YuGiOh_DeckBuilder.Utility;

/// <summary>
/// Class to open and close a console window to display output
/// </summary>
internal static class ConsoleAllocator
{
    #region Extern
    [DllImport(@"kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();
    [DllImport(@"kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();
    [DllImport(@"user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    #endregion

    #region Members
    private const int swHide = 0;
    private const int swShow = 5;
    #endregion

    #region Properties
    /// <summary>
    /// Indicates whether the console is currently enabled
    /// </summary>
    internal static bool IsEnabled { get; private set; }
    #endregion
    
    #region Methods
    /// <summary>
    /// Opens the console window
    /// </summary>
    internal static void ShowConsoleWindow()
    {
        var handle = GetConsoleWindow();

        if (handle == IntPtr.Zero)
        {
            AllocConsole();
        }
        else
        {
            ShowWindow(handle, swShow);
            IsEnabled = true;
        }
    }

    /// <summary>
    /// Closes the console window
    /// </summary>
    internal static void HideConsoleWindow()
    {
        var handle = GetConsoleWindow();

        ShowWindow(handle, swHide);
        IsEnabled = false;
    }
    #endregion
}