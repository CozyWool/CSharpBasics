namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var biGrams = GetNGramms(2, text);
        var triGrams = GetNGramms(3, text);
        var result = ParseToMostFrequentNGramm(biGrams)
                     .Union(ParseToMostFrequentNGramm(triGrams))
                     .ToDictionary();

        return result;
    }

    private static Dictionary<string, string> ParseToMostFrequentNGramm(
        Dictionary<string, Dictionary<string, int>> nGramm)
    {
        var result = new Dictionary<string, string>();
        foreach (var (key, value) in nGramm)
        {
            var valueList = value.ToList();
            valueList.Sort((kv1, kv2) =>
                           {
                               if (kv1.Value == kv2.Value)
                               {
                                   return string.CompareOrdinal(kv2.Key, kv1.Key);
                               }

                               return kv1.Value - kv2.Value;
                           });
            result.Add(key, valueList.Last().Key);
        }

        return result;
    }

    public static Dictionary<string, Dictionary<string, int>> GetNGramms(int n, List<List<string>> text)
    {
        var result = new Dictionary<string, Dictionary<string, int>>();
        foreach (var sentence in text)
        {
            for (var i = 0; i < sentence.Count - n + 1; i++)
            {
                var key = string.Join(' ', sentence.Skip(i).Take(n - 1));
                var value = sentence[i + n - 1];
                
                // Выделил метод, но все равно передаю result
                // А если его не передавать, то получится лишнее копирование
                // В общем, я за то, чтобы не выделять тут этот метод
                ChangeValue(key, value, result);
            }
        }

        return result;
    }

    private static void ChangeValue(string key, string value, Dictionary<string, Dictionary<string, int>> result)
    {
        if (!result.ContainsKey(key))
        {
            result[key] = new Dictionary<string, int>
                          {
                              {value, 1}
                          };
        }
        else if (!result[key].ContainsKey(value))
        {
            result[key][value] = 1;
        }
        else
        {
            result[key][value]++;
        }
    }
}