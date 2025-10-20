namespace TextAnalysis;

static class FrequencyAnalysisTask
{
    public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
    {
        var result = new Dictionary<string, string>();
        var biGrams = GetNGramms(2, text);
        var triGrams = GetNGramms(3, text);
        ParseToMostFrequentNGramm(biGrams, result);
        ParseToMostFrequentNGramm(triGrams, result);

        return result;
    }

    private static void ParseToMostFrequentNGramm(Dictionary<string, Dictionary<string, int>> nGramm,
                                                  Dictionary<string, string> result)
    {
        foreach (var (key, value) in nGramm)
        {
            var valueList = value.ToList();
            valueList.Sort((kv1, kv2) =>
                           {
                               if (kv1.Value < kv2.Value)
                               {
                                   return -1;
                               }

                               if (kv1.Value == kv2.Value && string.CompareOrdinal(kv1.Key, kv2.Key) > 0)
                               {
                                   return -1;
                               }

                               return 1;
                           });
            result.Add(key, valueList.Last().Key);
        }
    }

    public static Dictionary<string, Dictionary<string, int>> GetNGramms(int n, List<List<string>> text)
    {
        var result = new Dictionary<string, Dictionary<string, int>>();
        foreach (var sentence in text)
        {
            for (var i = 0; i < sentence.Count - n + 1; i++)
            {
                var key = string.Join(' ', sentence.Skip(i).Take(n - 1));
                ;
                var value = sentence[i + n - 1];

                if (result.TryAdd(key, new Dictionary<string, int>()))
                {
                    result[key].Add(value, 1);
                }
                else if (!result[key].TryAdd(value, 1))
                {
                    result[key][value]++;
                }
            }
        }

        return result;
    }
}