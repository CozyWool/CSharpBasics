using System;

namespace Recognizer;

internal static class SobelFilterTask
{
    public static double[,] SobelFilter(double[,] g, double[,] sx)
    {
        var width = g.GetLength(0);
        var height = g.GetLength(1);
        var result = new double[width, height];

        var delta = sx.GetLength(0) / 2;
        for (var x = delta; x < width - delta; x++)
        {
            for (var y = delta; y < height - delta; y++)
            {
                result[x, y] = ApplySobelFilter(g, sx, delta, x, y);
            }
        }

        return result;
    }

    private static double ApplySobelFilter(double[,] g, double[,] sx, int delta, int x, int y)
    {
        var gx = 0.0;
        var gy = 0.0;
        for (var i = x - delta; i <= x + delta; i++)
        {
            for (var j = y - delta; j <= y + delta; j++)
            {
                gx += g[i, j] * sx[i - x + delta, j - y + delta];
                gy += g[i, j] * sx[j - y + delta, i - x + delta];
            }
        }

        return Math.Sqrt(gx * gx + gy * gy);
    }
}