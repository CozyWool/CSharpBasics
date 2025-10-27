using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class FieldParserTaskTests
{
    public static void Test(string input, string[] expectedResult)
    {
        var actualResult = FieldsParserTask.ParseLine(input);
        Assert.That(actualResult.Count, Is.EqualTo(expectedResult.Length));
        for (var i = 0; i < expectedResult.Length; ++i)
        {
            Assert.That(actualResult[i].Value, Is.EqualTo(expectedResult[i]));
        }
    }

    [TestCase("text", new[] {"text"})]
    [TestCase("hello world", new[] {"hello", "world"})]
    [TestCase("hello  world!", new[] {"hello", "world!"})]
    [TestCase("'hello  world!'", new[] {"hello  world!"})]
    [TestCase(@"'hello """" world!'", new[] {@"hello """" world!"})]
    [TestCase(@"""hello '' world!""", new[] {@"hello '' world!"})]
    [TestCase(@"''", new[] {""})]
    [TestCase(@"", new string[0])]
    [TestCase(@"""hello \"" world!""", new[] {@"hello "" world!"})]
    [TestCase(@"\\""hello ` world!""", new[] {@"\\", @"hello ` world!"})]
    [TestCase(@"""helloworld!", new[] {"helloworld!"})]
    [TestCase("'hello\\'world!'", new[] {"hello'world!"})]
    [TestCase(@"""hello ` world!"" hello world", new[] {"hello ` world!", "hello", "world"})]
    [TestCase("'hello\\\\world!'", new[] {"hello\\world!"})]
    [TestCase("'hello\\\\'", new[] {"hello\\"})]
    [TestCase("'hello  world! ", new[] {"hello  world! "})]
    [TestCase(" 'helloworld!'", new[] {"helloworld!"})]
    [TestCase("a \"bcd ef\" 'x y'", new[] {"a", "bcd ef", "x y"})]
    public static void RunTests(string input, string[] expectedOutput)
    {
        Test(input, expectedOutput);
    }
}

public class FieldsParserTask
{
    public static List<Token> ParseLine(string line)
    {
        var tokens = new List<Token>();
        if (string.IsNullOrEmpty(line))
        {
            return tokens;
        }

        var nextIndex = FindField(line, 0);
        while (nextIndex != line.Length)
        {
            tokens.Add(GetNextField(line, nextIndex));
            nextIndex = FindField(line, tokens[^1].GetIndexNextToToken());
        }

        return tokens;
    }

    private static Token GetNextField(string line, int nextIndex)
    {
        return line[nextIndex] is '\"' or '\''
                   ? ReadQuotedField(line, nextIndex)
                   : ReadField(line, nextIndex);
    }

    private static int FindField(string line, int startIndex)
    {
        while (startIndex < line.Length && line[startIndex] is ' ')
        {
            startIndex++;
        }

        return startIndex;
    }

    private static Token ReadField(string line, int startIndex)
    {
        var finalStringBuilder = new StringBuilder();

        var skippedCount = 0;
        var currentIndex = startIndex;
        while (line[currentIndex] == ' ')
        {
            skippedCount++;
            currentIndex++;
        }

        while (currentIndex < line.Length)
        {
            if (line[currentIndex] is ' ' or '\"' or '\'')
            {
                break;
            }

            finalStringBuilder.Append(line[currentIndex++]);
        }

        return new Token(finalStringBuilder.ToString(), startIndex, finalStringBuilder.Length + skippedCount);
    }

    public static Token ReadQuotedField(string line, int startIndex)
    {
        return QuotedFieldTask.ReadQuotedField(line, startIndex);
    }
}