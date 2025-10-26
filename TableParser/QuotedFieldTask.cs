using System.Text;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class QuotedFieldTaskTests
{
    [TestCase("'", 0, "", 1)]
    [TestCase("''", 0, "", 2)]
    [TestCase("'a'", 0, "a", 3)]
    [TestCase("'abc", 0, "abc", 4)]
    [TestCase("'abc'", 0, "abc", 5)]
    [TestCase(@"""abc'", 0, "abc'", 5)]
    [TestCase(@"""abc''abc""", 0, "abc''abc", 10)]
    [TestCase(@"""\\\\\\""", 0, @"\\\", 8)]
    [TestCase(@"""\\\\\""", 0, @"\\""", 7)]
    [TestCase(@"""\\""\\\""", 3, @"\""", 5)]
    [TestCase(@"hello world!""how are you?'fine thank you'!""""idk what to write in these tests",
                 12,
                 "how are you?'fine thank you'!",
                 31)]
    public void Test(string line, int startIndex, string expectedValue, int expectedLength)
    {
        var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
        var expectedToken = new Token(expectedValue, startIndex, expectedLength);
        Assert.That(actualToken, Is.EqualTo(expectedToken));
    }
}

class QuotedFieldTask
{
    public static Token ReadQuotedField(string line, int startIndex)
    {
        var finalStringBuilder = new StringBuilder();

        var firstSymbol = line[startIndex];
        var escapedCount = 1;
        var currentIndex = startIndex + 1;
        while (currentIndex < line.Length)
        {
            if (firstSymbol == line[currentIndex])
            {
                escapedCount++;
                break;
            }

            if (line[currentIndex] == '\\' && currentIndex + 1 != line.Length)
            {
                currentIndex++;
                escapedCount++;
            }
            finalStringBuilder.Append(line[currentIndex]);
            currentIndex++;
        }

        return new Token(finalStringBuilder.ToString(), startIndex, finalStringBuilder.Length + escapedCount);
    }
}