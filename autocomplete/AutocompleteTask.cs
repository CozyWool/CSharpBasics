using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Autocomplete;

internal class AutocompleteTask
{
    public AutocompleteTask()
    {
        GetTopByPrefix(["aa", "ab", "ac", "bc"], "a", 10);
    }

    /// <returns>
    /// Возвращает первую фразу словаря, начинающуюся с prefix.
    /// </returns>
    public static string FindFirstByPrefix(IReadOnlyList<string> phrases, string prefix)
    {
        var index = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count) + 1;
        if (index < phrases.Count && phrases[index].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
        {
            return phrases[index];
        }

        return null;
    }

    /// <returns>
    /// Возвращает первые в лексикографическом порядке count (или меньше, если их меньше count)
    /// элементов словаря, начинающихся с prefix.
    /// </returns>
    public static string[] GetTopByPrefix(IReadOnlyList<string> phrases, string prefix, int count)
    {
        var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
        var result = new List<string>();
        count = Math.Min(count, GetCountByPrefix(phrases, prefix));
        for (var i = leftBorder + 1; i < Math.Min(phrases.Count, leftBorder + count); i++)
        {
            result.Add(phrases[i]);
        }

        return result.ToArray();
    }

    /// <returns>
    /// Возвращает количество фраз, начинающихся с заданного префикса
    /// </returns>
    public static int GetCountByPrefix(IReadOnlyList<string> phrases, string prefix)
    {
        var leftBorder = LeftBorderTask.GetLeftBorderIndex(phrases, prefix, -1, phrases.Count);
        var rightBorder = RightBorderTask.GetRightBorderIndex(phrases, prefix, -1, phrases.Count);

        if (rightBorder == phrases.Count)
        {
            rightBorder--;
        }

        return rightBorder - leftBorder + 1;
    }
}

[TestFixture]
public class AutocompleteTests
{
    [Test]
    public void TopByPrefix_IsEmpty_WhenNoPhrases()
    {
        // ...
        //CollectionAssert.IsEmpty(actualTopWords);
    }

    // ...

    [Test]
    public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
    {
        // ...
        //Assert.AreEqual(expectedCount, actualCount);
    }

    // ...
}