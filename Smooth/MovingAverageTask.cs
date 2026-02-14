using System.Collections.Generic;

namespace yield;

public static class MovingAverageTask
{
    public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var window = new Queue<double>();
        double sum = 0;

        foreach (var current in data)
        {
            if (window.Count == windowWidth)
            {
                sum -= window.Dequeue();
            }

            sum += current.OriginalY;
            window.Enqueue(current.OriginalY);
            yield return current.WithAvgSmoothedY(sum / window.Count);
        }
    }
}