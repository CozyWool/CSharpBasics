using System;
using System.Collections.Generic;

namespace DiskTree;

public static class DiskTreeTask
{
    private class DiskTreeNode
    {
        public SortedDictionary<string, DiskTreeNode> Children { get; } = new(StringComparer.Ordinal);
    }

    public static IEnumerable<string> Solve(List<string> input)
    {
        var root = new DiskTreeNode();

        foreach (var path in input)
        {
            var directories = path.Split('\\', StringSplitOptions.RemoveEmptyEntries);
            var current = root;

            foreach (var directory in directories)
            {
                current.Children.TryAdd(directory, new DiskTreeNode());
                current = current.Children[directory];
            }
        }

        return GetResult(root, -1);
    }

    private static IEnumerable<string> GetResult(DiskTreeNode node, int depth)
    {
        foreach (var (directory, nextNode) in node.Children)
        {
            yield return directory.PadLeft(directory.Length + depth + 1, ' ');

            foreach (var childResult in GetResult(nextNode, depth + 1))
            {
                yield return childResult;
            }
        }
    }
}