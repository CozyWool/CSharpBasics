using System.Collections.Generic;

namespace yield;

public class MaxWindowItem
{
    public double Y { get; set; }
    public int Index { get; set; }
}

public static class MovingMaxTask
{
    public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
    {
        var window = new LinkedList<MaxWindowItem>();
        var currentIndex = 0;
        foreach (var current in data)
        {
            while (window.Count > 0 && window.First.Value.Index < currentIndex - windowWidth + 1)
            {
                window.RemoveFirst();
            }

            while (window.Count > 0 && window.Last.Value.Y < current.OriginalY)
            {
                window.RemoveLast();
            }

            window.AddLast(new MaxWindowItem
                           {
                               Y = current.OriginalY,
                               Index = currentIndex++
                           });
            yield return current.WithMaxY(window.First.Value.Y);
        }
    }
}