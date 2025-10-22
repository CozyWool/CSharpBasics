namespace TextAnalysis;

static class TextGeneratorTask
{
    public static string ContinuePhrase(
        Dictionary<string, string> nextWords,
        string phraseBeginning,
        int wordsCount)
    {
        var phraseWords = phraseBeginning.Split(' ');
        if (phraseWords.Length < 1)
        {
            return phraseBeginning;
        }

        var phrases = new List<string>();
        if (phraseBeginning.Length >= 1)
        {
            phrases.Add(phraseWords[0]);
        }

        if (phraseBeginning.Length >= 2)
        {
            phrases.AddRange(phraseWords[1..]);
        }

        while (phrases.Count - phraseWords.Length < wordsCount)
        {
            if (phrases.Count >= 2 && nextWords.ContainsKey(string.Join(' ', phrases[^2..])))
            {
                phrases.Add(nextWords[string.Join(' ', phrases[^2..])]);
            }
            else if (nextWords.ContainsKey(phrases[^1]))
            {
                phrases.Add(nextWords[phrases[^1]]);
            }
            else
            {
                return string.Join(' ', phrases);
            }
        }

        return string.Join(' ', phrases);
    }
}