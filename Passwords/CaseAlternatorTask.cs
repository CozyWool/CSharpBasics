namespace Passwords;

public class CaseAlternatorTask
{
    public static List<string> AlternateCharCases(string lowercaseWord)
    {
        var result = new List<string>();
        AlternateCharCases(lowercaseWord.ToCharArray(), 0, result);
        return result.Distinct().ToList();
    }

    static void AlternateCharCases(char[] word, int startIndex, List<string> result)
    {
        result.Add(new string(word));

        if (startIndex == word.Length)
        {
            return;
        }
        AlternateCharCases(word, startIndex + 1, result);

        var current = word[startIndex];
        if (!char.IsLetter(current) || char.ToUpper(current) == char.ToLower(current))
        {
            return;
        }

        word[startIndex] = char.ToUpper(current);
        AlternateCharCases(word, startIndex + 1, result);
        word[startIndex] = current;
    }
}