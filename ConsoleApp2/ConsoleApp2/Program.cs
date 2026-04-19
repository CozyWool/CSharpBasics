namespace ConsoleApp2;

internal class Program
{
    public static void Main(string[] args)
    {
#if !ONLINE_JUDGE
        if (File.Exists("in.txt"))
        {
            Console.SetIn(new StreamReader("in.txt"));
        }
#endif
        using var writer = new StreamWriter(Console.OpenStandardOutput());
        Console.SetOut(writer);
        var sc = new Scanner(Console.In);
    }
}

internal class Scanner(TextReader reader)
{
    private readonly Stack<string> _tokens = new();

    public string? Next()
    {
        while (_tokens.Count == 0)
        {
            var line = reader.ReadLine();
            if (line == null)
            {
                return null;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Reverse();
            foreach (var p in parts)
            {
                _tokens.Push(p);
            }
        }

        return _tokens.Pop();
    }

    public int NextInt() => int.Parse(Next() ?? "0");
    public long NextLong() => long.Parse(Next() ?? "0");
}