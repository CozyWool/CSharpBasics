namespace ConsoleApp2;

internal class MyStringBuilder
{
    private List<char> _chars = [];

    public void Append(string s)
    {
        _chars.AddRange(s.ToCharArray());
    }

    public void Insert(string s, int pos)
    {
        _chars.InsertRange(pos, s.ToCharArray());
    }

    public void Remove(int pos, int count)
    {
        _chars.RemoveRange(pos, count);
    }

    public void Clear()
    {
        _chars.Clear();
    }

    public int Length()
    {
        return _chars.Count;
    }

    public override string ToString()
    {
        return string.Join("", _chars);
    }
}