using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Autocomplete;

internal class AutocompleteTask
{
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
        for (var i = leftBorder + 1; i < Math.Min(phrases.Count, leftBorder + 1 + count); i++)
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

        return rightBorder - leftBorder - 1;
    }
}

[TestFixture]
public class AutocompleteTests
{
    // Тесты для FindFirstByPrefix
    [Test]
    public void FindFirstByPrefix_ReturnsFirstMatchingPhrase()
    {
        var phrases = new List<string> { "apple", "banana", "cherry", "date" };
        var result = AutocompleteTask.FindFirstByPrefix(phrases, "b");
        Assert.That(result, Is.EqualTo("banana"));
    }

    [Test]
    public void FindFirstByPrefix_ReturnsNull_WhenNoMatches()
    {
        var phrases = new List<string> { "apple", "banana", "cherry" };
        var result = AutocompleteTask.FindFirstByPrefix(phrases, "z");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void FindFirstByPrefix_ReturnsNull_WhenEmptyList()
    {
        var phrases = new List<string>();
        var result = AutocompleteTask.FindFirstByPrefix(phrases, "a");
        Assert.That(result, Is.Null);

    }

    [Test]
    public void FindFirstByPrefix_IsCaseInsensitive()
    {
        var phrases = new List<string> { "Apple", "Banana", "Cherry" };
        var result = AutocompleteTask.FindFirstByPrefix(phrases, "a");
        Assert.That(result, Is.EqualTo("Apple"));
    }

    // Тесты для GetTopByPrefix
    [Test]
    public void TopByPrefix_IsEmpty_WhenNoPhrases()
    {
        var phrases = new List<string>();
        var result = AutocompleteTask.GetTopByPrefix(phrases, "a", 5);
        CollectionAssert.IsEmpty(result);
    }

    [Test]
    public void TopByPrefix_ReturnsRequestedCount_WhenEnoughMatches()
    {
        var phrases = new List<string> { "apple", "application", "banana", "appetite", "cherry" };
        var result = AutocompleteTask.GetTopByPrefix(phrases, "app", 2);
        var expected = new[] { "apple", "application" };
        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void TopByPrefix_ReturnsAllMatches_WhenCountExceedsMatches()
    {
        var phrases = new List<string> { "apple", "application", "banana" };
        var result = AutocompleteTask.GetTopByPrefix(phrases, "app", 5);
        var expected = new[] { "apple", "application" };
        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void TopByPrefix_ReturnsEmpty_WhenNoMatches()
    {
        var phrases = new List<string> { "apple", "banana", "cherry" };
        var result = AutocompleteTask.GetTopByPrefix(phrases, "z", 3);
        CollectionAssert.IsEmpty(result);
    }

    [Test]
    public void TopByPrefix_ReturnsInLexicographicalOrder()
    {
        var phrases = new List<string> { "zeta", "alpha", "beta", "gamma", "alpha2" };
        var sortedPhrases = phrases.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
        var result = AutocompleteTask.GetTopByPrefix(sortedPhrases, "al", 3);
        var expected = new[] { "alpha", "alpha2" };
        CollectionAssert.AreEqual(expected, result);
    }

    // Тесты для GetCountByPrefix
    [Test]
    public void CountByPrefix_IsTotalCount_WhenEmptyPrefix()
    {
        var phrases = new List<string> { "apple", "banana", "cherry" };
        var expectedCount = phrases.Count;
        var actualCount = AutocompleteTask.GetCountByPrefix(phrases, "");
        Assert.That(expectedCount, Is.EqualTo(actualCount));
    }

    [Test]
    public void CountByPrefix_ReturnsZero_WhenNoMatches()
    {
        var phrases = new List<string> { "apple", "banana", "cherry" };
        var result = AutocompleteTask.GetCountByPrefix(phrases, "z");
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CountByPrefix_ReturnsZero_WhenEmptyList()
    {
        var phrases = new List<string>();
        var result = AutocompleteTask.GetCountByPrefix(phrases, "a");
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CountByPrefix_IsCaseInsensitive()
    {
        var phrases = new List<string> { "Apple", "APPLE", "apple", "banana" };
        var result = AutocompleteTask.GetCountByPrefix(phrases, "app");
        Assert.That(result, Is.EqualTo(3));
    }

    // Комплексные тесты
    [Test]
    public void IntegrationTest_AllMethodsWorkTogether()
    {
        var phrases = new List<string>
        {
            "cat", "category", "catalog", "dog", "caterpillar", "cab", "car"
        };
        var sortedPhrases = phrases.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();

        var first = AutocompleteTask.FindFirstByPrefix(sortedPhrases, "cat");
        Assert.That(first, Is.EqualTo("cat"));

        var top3 = AutocompleteTask.GetTopByPrefix(sortedPhrases, "ca", 3);
        var expectedTop3 = new[] { "cab", "car", "cat" };
        CollectionAssert.AreEqual(expectedTop3, top3);

        var count = AutocompleteTask.GetCountByPrefix(sortedPhrases, "ca");
        Assert.That(count, Is.EqualTo(6)); // cab, car, cat, catalog, category, caterpillar
    }

    [Test]
    public void TestWithSpecialCharacters()
    {
        var phrases = new List<string>
        {
            "hello", "hello-world", "hello_world", "hello123", "hell"
        };
        var sortedPhrases = phrases.OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();

        var count = AutocompleteTask.GetCountByPrefix(sortedPhrases, "hello");
        Assert.That(count, Is.EqualTo(4));
    }
}