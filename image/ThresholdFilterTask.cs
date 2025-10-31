using System.Linq;

namespace Recognizer;

public static class ThresholdFilterTask
{
    public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
    {
        var row = original.GetLength(0);
        var col = original.GetLength(1);
        var thresholdFiltered = new double[row, col];
        var whitePixels = (int) (whitePixelsFraction * thresholdFiltered.Length);

        if (whitePixels == 0)
        {
            return thresholdFiltered;
        }

        var t = original.Cast<double>()
                        .OrderBy(x => -x)
                        .Take(whitePixels)
                        .LastOrDefault();

        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                if (original[i, j] >= t)
                {
                    thresholdFiltered[i, j] = 1.0;
                }
            }
        }

        return thresholdFiltered;
    }
}