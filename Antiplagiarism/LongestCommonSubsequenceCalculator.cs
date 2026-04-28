using System;
using System.Collections.Generic;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> first, List<string> second)
    {
        var opt = CreateOptimizationTable(first, second);
        return RestoreAnswer(opt, first, second);
    }

    private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
    {
        var opt = new int[first.Count + 1, second.Count + 1];

        for (var row = 1; row < first.Count + 1; row++)
        {
            for (var j = 1; j < second.Count + 1; j++)
            {
                if (first[row - 1] == second[j - 1])
                {
                    opt[row, j] = opt[row - 1, j - 1] + 1;
                }
                else
                {
                    opt[row, j] = Math.Max(opt[row - 1, j], opt[row, j - 1]);
                }
            }
        }

        return opt;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
    {
        var path = new List<string>();
        var row = first.Count;
        var col = second.Count;

        while (row > 0 && col > 0)
        {
            if (first[row - 1] == second[col - 1])
            {
                path.Add(first[row - 1]);
                row--;
                col--;
            }
            else if (opt[row - 1, col] > opt[row, col - 1])
            {
                row--;
            }
            else
            {
                col--;
            }
        }

        path.Reverse();
        return path;
    }
}