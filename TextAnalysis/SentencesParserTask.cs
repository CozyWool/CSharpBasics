namespace TextAnalysis;

static class SentencesParserTask
{
    public static readonly char[] SentencesSeparators = ['.', '!', '?', ';', ':', '(', ')'];

    public static List<List<string>> ParseSentences(string text)
    {
        var sentencesList = text
                            .Split(SentencesSeparators, StringSplitOptions.RemoveEmptyEntries);

        var wordsSeparators = sentencesList
                              .SelectMany(s => s.Where(symbol => !char.IsLetter(symbol) && symbol != '\''))
                              .ToHashSet()
                              .ToArray();
        var parsedSentences = sentencesList
                              .Select(sentence => sentence
                                           .ToLower()
                                           .Split(wordsSeparators, StringSplitOptions.RemoveEmptyEntries)
                                           .ToList())
                              .Where(sentence => sentence.Count > 0)
                              .ToList();
        return parsedSentences;
    }
}