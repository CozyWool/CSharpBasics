using System.Collections.Generic;

namespace Recognizer;

internal static class MedianFilterTask
{
    public static double FindMedian(List<double> neighbours)
    {
        neighbours.Sort();
        if (neighbours.Count % 2 == 0)
        {
            return (neighbours[neighbours.Count / 2] + neighbours[neighbours.Count / 2 - 1]) / 2;
        }

        return neighbours[neighbours.Count / 2];
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
                var neighbours = new List<double>();
                for (var x = i - 1; x <= i + 1; x++)
                {
                    for (var y = j - 1; y <= j + 1; y++)
                    {
                        if (0 <= x && x < row && 0 <= y && y < col)
                        {
                            neighbours.Add(original[x, y]);
                        }
                    }
                }

                clearImage[i, j] = FindMedian(neighbours);
            }
        }

        return clearImage;
    }
}