namespace TextAnalysis;

static class TextGeneratorTask
{
    public static string ContinuePhrase(
        Dictionary<string, string> nextWords,
        string phraseBeginning,
        int wordsCount)
    {
        var phraseWords = phraseBeginning.Split(' ').ToList();

        for (var i = 0; i < wordsCount; ++i)
        {
            if (phraseWords.Count >= 2 &&
                nextWords.TryGetValue(string.Join(' ', phraseWords[^2..]), out var wordAfterTwo))
            {
                phraseWords.Add(wordAfterTwo);
            }
            else if (phraseWords.Count >= 1 &&
                     nextWords.TryGetValue(phraseWords[^1], out var wordAfterOne))
            {
                phraseWords.Add(wordAfterOne);
            }
            else
            {
                break;
            }
        }

        return string.Join(' ', phraseWords);
    }
}