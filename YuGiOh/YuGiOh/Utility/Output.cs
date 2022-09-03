using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using YuGiOh_DeckBuilder.Extensions;
using YuGiOh_DeckBuilder.YuGiOh.Logging;

namespace YuGiOh_DeckBuilder.YuGiOh.Utility;

/// <summary>
/// Contains methods for different <see cref="Console"/> outputs
/// </summary>
internal static class Output
{
    #region Methods
    /// <summary>
    /// Basic output for current iteration state <br/>
    /// <i>message + currentNumber/maxNumber | endpoint</i>
    /// </summary>
    /// <param name="message">The message of this output</param>
    /// <param name="currentNumber">Current iteration</param>
    /// <param name="maxNumber">Max iterations</param>
    /// <param name="endpoint">Endpoint of this iteration</param>
    /// <param name="testOutputHelper">Output for unit tests</param>
    /// <returns>The printed <see cref="string"/></returns>
    internal static string Print(string message, int currentNumber, int maxNumber, string endpoint, ITestOutputHelper? testOutputHelper = null)
    {
        var maxLength = maxNumber.ToString().Length;
        var currentLength = currentNumber.ToString().Length;
        var padding = maxLength - currentLength;
        var print = $"{message} {currentNumber.ToString().PadLeft(currentLength + padding)}/{maxNumber} | {endpoint}";

        if (testOutputHelper != null)
        {
            testOutputHelper.WriteLine(print);
        }
        else
        {
            Console.WriteLine(print);   
        }

        return print;
    }

    /// <summary>
    /// Prints the number of skipped items from the given <see cref="ConcurrentDictionary{TKey,TValue}"/> if there were any
    /// </summary>
    /// <param name="initialCount">Count of initial objects</param>
    /// <param name="currentCount">Count of current objects</param>
    /// <param name="skipped">The <see cref="ConcurrentDictionary{TKey,TValue}"/> that gold the skipped items</param>
    /// <param name="testOutputHelper">Output for unit tests</param>
    /// <returns>The printed <see cref="string"/></returns>
    internal static string PrintSkip(int initialCount, int currentCount, ConcurrentDictionary<Skip, ICollection<string>> skipped, ITestOutputHelper? testOutputHelper = null)
    {
        var skipCount = skipped.Sum(skip => skip.Value.Count);
        var print = string.Empty;

        if (skipCount > 0)
        {
            var reasons = string.Empty;

            foreach (var (reason, endpoints) in skipped)
            {
                reasons += $"-{reason.ToString().CamelCaseSpacing()}: {endpoints.Count.ToString()}\n";
            }

            print = $"Initial: {initialCount} | Current: {currentCount} | Skipped: {skipCount}\n{reasons}";
            
            if (testOutputHelper != null)
            {
                testOutputHelper.WriteLine(print);
            }
            else
            {
                Console.WriteLine(print);   
            } 
        }

        return print;
    }
    #endregion
}