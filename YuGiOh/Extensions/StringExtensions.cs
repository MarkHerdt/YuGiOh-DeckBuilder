using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace YuGiOh_DeckBuilder.Extensions;

/// <summary>
/// Contains extension methods for <see cref="string"/> instances
/// </summary>
internal static class StringExtensions
{
    #region Members
    /// <summary>
    /// <see cref="Regex"/> to add a space before every uppercase letter
    /// </summary>
    private static readonly Regex spaceBeforeUppercaseRegex = new("([a-z])([A-Z])");
    #endregion
    
    #region Methods
    /// <summary>
    /// Reports the zero-based index of the nth occurrence of the specified string in the current String object. A parameter specifies the type of search to use for the specified string
    /// </summary>
    /// <param name="string">The <see cref="string"/> to search in</param>
    /// <param name="value">The <see cref="char"/> to seek</param>
    /// <param name="occurence">The number of occurence to look for</param>
    /// <returns>The index position of the value parameter if that <see cref="string"/> is found, or -1 if it is not. If value is <see cref="string.Empty"/>, the return value is 0.</returns>
    internal static int NthIndexOf(this string @string, char value, uint occurence)
    {
        var count = 0;

        for (var i = 0; i < @string.Length; i++)
        {
            if (@string[i] == value && ++count == occurence)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the zero-based index of the last nth occurrence of the specified string in the current String object. A parameter specifies the type of search to use for the specified string
    /// </summary>
    /// <param name="string">The <see cref="string"/> to search in</param>
    /// <param name="value">The <see cref="char"/> to seek</param>
    /// <param name="occurence">The number of occurence to look for</param>
    /// <returns>The index position of the value parameter if that <see cref="string"/> is found, or -1 if it is not. If value is <see cref="string.Empty"/>, the return value is 0.</returns>
    internal static int NthIndexOfLast(this string @string, char value, uint occurence)
    {
        var count = 0;

        for (var i = @string.Length - 1; i > -1; i--)
        {
            if (@string[i] == value && ++count == occurence)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the zero-based index of the nth occurrence of the specified string in the current String object. A parameter specifies the type of search to use for the specified string
    /// </summary>
    /// <param name="string">The <see cref="string"/> to search in</param>
    /// <param name="value">The <see cref="char"/> to seek</param>
    /// <param name="occurence">The number of occurence to look for</param>
    /// <returns>The index position of the value parameter if that <see cref="string"/> is found, or -1 if it is not. If value is <see cref="string.Empty"/>, the return value is 0.</returns>
    internal static int NthOccurenceOf(this string @string, string value, uint occurence)
    {
        var index = 0;
        var tmpString = @string;
        
        for (var i = 1; i <= occurence; i++)
        {
            int tmpIndex;
            
            index += tmpIndex = tmpString.IndexOf(value, StringComparison.Ordinal);

            tmpString = i != occurence ? tmpString[(tmpIndex + value.Length)..] : tmpString[tmpIndex..];
        }

        return @string.Length - index;
    }
    
    /// <summary>
    /// Reports the zero-based index of the nth occurrence of the specified string in the current String object. A parameter specifies the type of search to use for the specified string
    /// </summary>
    /// <param name="string">The <see cref="string"/> to search in</param>
    /// <param name="value">The <see cref="char"/> to seek</param>
    /// <param name="occurence">The number of occurence to look for</param>
    /// <returns>The index position of the value parameter if that <see cref="string"/> is found, or -1 if it is not. If value is <see cref="string.Empty"/>, the return value is 0.</returns>
    internal static int NthOccurenceOfLast(this string @string, string value, uint occurence)
    {
        var index = -1;
        var tmpString = @string;
        
        for (var i = 1; i <= occurence; i++)
        {
            index = tmpString.LastIndexOf(value, StringComparison.Ordinal);

            tmpString = tmpString[..index];
        }

        return index;
    }
    
    /// <summary>
    /// Returns true when this <see cref="string"/> is null, <see cref="string"/>.<see cref="string.Empty"/> of every <see cref="char"/> is a whitespace
    /// </summary>
    /// <param name="string">The <see cref="string"/> to check</param>
    /// <returns><see cref="bool"/></returns>
    internal static bool IsNullEmptyOrWhitespace(this string? @string)
    {
        return string.IsNullOrEmpty(@string) || @string.All(@char => @char == ' ');
    }
    
    /// <summary>
    /// Returns this <see cref="string"/> with spacing between each upper case letter
    /// </summary>
    /// <param name="string">The <see cref="string"/> to format</param>
    /// <returns>The formatted <see cref="string"/></returns>
    internal static string CamelCaseSpacing(this string @string)
    {
        return spaceBeforeUppercaseRegex.Replace(@string, "$1 $2");
    }
    #endregion
}