namespace TextAnalysis;

static class SentencesParserTask
{
    private static readonly char[] SentencesSeparators = ['.', '!', '?', ';', ':', '(', ')'];

    public static List<List<string>> ParseSentences(string text)
    {
        var wordsSeparators = text
                              .Where(symbol => !char.IsLetter(symbol) && symbol != '\'')
                              .ToHashSet()
                              .ToArray();
        var parsedSentences = text
                              .ToLower()
                              .Split(SentencesSeparators, StringSplitOptions.RemoveEmptyEntries)
                              .Select(sentence => sentence
                                                  .Split(wordsSeparators, StringSplitOptions.RemoveEmptyEntries)
                                                  .ToList())
                              .Where(sentence => sentence.Count > 0)
                              .ToList();
        return parsedSentences;
    }
}