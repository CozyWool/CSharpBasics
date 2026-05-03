using System;
using System.Collections.Generic;

namespace DiskTree;

public static class DiskTreeTask
{
    private class DiskTreeNode
    {
        public SortedList<string, DiskTreeNode> Children { get; } = new(StringComparer.Ordinal);
    }

    public static List<string> Solve(List<string> input)
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

        var result = new List<string>();
        result.FillResult(root);
        return result;
    }

    private static void FillResult(this List<string> result, DiskTreeNode node, int depth = -1)
    {
        foreach (var (directory, nextNode) in node.Children)
        {
            var indent = new string(' ', depth + 1);
            result.Add($"{indent}{directory}");
            result.FillResult(nextNode, depth + 1);
        }
    }
}