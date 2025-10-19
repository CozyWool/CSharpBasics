using System.Text;

namespace TextAnalysis;

static class SentencesParserTask
{
    public static List<List<string>> ParseSentences(string text)
    {
        var sentencesList = text.Split(new[] {'.', '!', '?', ';', ':', '(', ')'}, StringSplitOptions.RemoveEmptyEntries)
                                .ToList();

        var parsedSentences = new List<List<string>>();
        var word = new StringBuilder();
        foreach (var sentence in sentencesList)
        {
            var wordList = new List<string>();
            foreach (var symbol in sentence)
            {
                if (char.IsLetter(symbol) || symbol == '\'')
                {
                    word.Append(char.ToLower(symbol));
                }
                else if (word.Length > 0)
                {
                    wordList.Add(word.ToString());
                    word.Clear();
                }
            }

            if (word.Length > 0)
            {
                wordList.Add(word.ToString());
                word.Clear();
            }

            parsedSentences.Add(wordList);
        }

        return parsedSentences;
    }
}