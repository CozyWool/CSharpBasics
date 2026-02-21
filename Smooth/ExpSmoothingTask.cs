using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
    public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
    {
        var previousSmoothed = double.NaN;
        foreach (var current in data)
        {
            if (!double.IsNaN(previousSmoothed))
            {
                previousSmoothed = current.OriginalY * alpha + (1 - alpha) * previousSmoothed;
            }
            else
            {
                previousSmoothed = current.OriginalY;
            }

            yield return current.WithExpSmoothedY(previousSmoothed);
        }
    }
}