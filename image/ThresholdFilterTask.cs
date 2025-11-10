using System.Linq;

namespace Recognizer;

public static class ThresholdFilterTask
{
    public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
    {
        var row = original.GetLength(0);
        var col = original.GetLength(1);
        var filteredByThreshold = new double[row, col];
        var whitePixels = (int) (whitePixelsFraction * filteredByThreshold.Length);

        if (whitePixels == 0)
        {
            return filteredByThreshold;
        }

        var threshold = CalculateThreshold(original, whitePixels);
        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                if (original[i, j] >= threshold)
                {
                    filteredByThreshold[i, j] = 1.0;
                }
            }
        }

        return filteredByThreshold;
    }

    private static double CalculateThreshold(this double[,] original, int whitePixels) =>
        original.Cast<double>()
                .OrderBy(x => -x)
                .Take(whitePixels)
                .LastOrDefault();
}