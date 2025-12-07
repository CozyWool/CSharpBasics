using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle;

public class Indexer : IIndexer
{
    private readonly Dictionary<string, Dictionary<int, List<int>>> _index = new();
    private readonly List<char> _separators = [' ', '.', ',', '!', '?', ':', '-', '\r', '\n', '–'];

    public void Add(int id, string documentText)
    {
        var end = 0;
        while (end < documentText.Length)
        {
            var start = end;
            (var word, end) = FindNextWord(documentText, start, end);
            AddNewIndex(id, word, start);
        }
    }

    private void AddNewIndex(int id, string word, int start)
    {
        if (!_index.TryGetValue(word, out var ids))
        {
            _index.Add(word, new Dictionary<int, List<int>>());
            ids = _index[word];
        }

        if (ids.TryGetValue(id, out var indexes))
        {
            indexes.Add(start);
        }
        else
        {
            ids.Add(id, [start]);
        }
    }

    private (string word, int end) FindNextWord(string documentText, int start, int end)
    {
        while (end < documentText.Length && !_separators.Contains(documentText[end]))
        {
            end++;
        }

        var word = documentText[start..end];

        while (end < documentText.Length && _separators.Contains(documentText[end]))
        {
            end++;
        }

        return (word, end);
    }

    public List<int> GetIds(string word)
    {
        return _index.TryGetValue(word, out var ids) ? ids.Keys.ToList() : [];
    }

    public List<int> GetPositions(int id, string word)
    {
        if (_index.TryGetValue(word, out var ids)
            && ids.TryGetValue(id, out var indexes))
        {
            return indexes;
        }

        return [];
    }

    public void Remove(int id)
    {
        foreach (var (_, ids) in _index)
        {
            ids.Remove(id);
        }
    }
}