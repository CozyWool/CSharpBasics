using NUnit.Framework;

namespace ConsoleApp2;

internal class Program
{
    public static void Main2(string[] args)
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

    public static void Main()
    {
        var tree = new TreeNode {Value = 10};
        Assert.AreEqual(null, Search(tree, 15));
        Assert.AreEqual(null, Search(tree, 5));
        Assert.AreEqual(10, Search(tree, 10).Value);

        tree.Left = new TreeNode {Value = 5};
        tree.Right = new TreeNode {Value = 15};
        Assert.AreEqual(null, Search(tree, 6));
        Assert.AreEqual(null, Search(tree, 3));
    }

    public class TreeNode
    {
        public int Value;
        public TreeNode Left, Right;
    }

    public static TreeNode Search(TreeNode? root, int element) =>
        root == null ? null :
        element == root.Value ? root :
        element < root.Value ? Search(root.Left, element) : Search(root.Right, element);
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