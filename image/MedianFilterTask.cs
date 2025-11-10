using System;
using System.Collections.Generic;

namespace Recognizer;

internal static class MedianFilterTask
{
    private static double FindMedian(this List<double> neighbours)
    {
        neighbours.Sort();
        if (neighbours.Count % 2 == 0)
        {
            return (neighbours[neighbours.Count / 2] + neighbours[neighbours.Count / 2 - 1]) / 2;
        }

        return neighbours[neighbours.Count / 2];
    }

    private static List<double> FindNeighbours(this double[,] original, int i, int j, int row, int col)
    {
        var neighbours = new List<double>();
        var lowerBoundX = Math.Max(i - 1, 0);
        var upperBoundX = Math.Min(i + 1, row - 1);
        var lowerBoundY = Math.Max(j - 1, 0);
        var upperBoundY = Math.Min(j + 1, col - 1);
        for (var x = lowerBoundX; x <= upperBoundX; x++)
        {
            for (var y = lowerBoundY; y <= upperBoundY; y++)
            {
                neighbours.Add(original[x, y]);
            }
        }

        return neighbours;
    }

    public static double[,] MedianFilter(double[,] original)
    {
        var row = original.GetLength(0);
        var col = original.GetLength(1);
        var clearImage = new double[row, col];
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                clearImage[i, j] = original
                                   .FindNeighbours(i, j, row, col)
                                   .FindMedian();
            }
        }

        return clearImage;
    }
}