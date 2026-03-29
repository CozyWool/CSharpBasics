using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
    private static readonly Point[] _directions =
    [
        new(1, 0),
        new(-1, 0),
        new(0, 1),
        new(0, -1)
    ];

    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
    {
        var used = new HashSet<Point> {start};
        var chestsLocations = chests.Select(c => c.Location).ToHashSet();
        var queue = new Queue<SinglyLinkedList<Point>>();
        queue.Enqueue(new SinglyLinkedList<Point>(start));

        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();
            var currentPoint = currentPath.Value;
            if (chestsLocations.Remove(currentPoint))
            {
                yield return currentPath;

                if (chestsLocations.Count == 0)
                {
                    yield break;
                }
            }

            EnqueueNextPoints(map, currentPoint, used, queue, currentPath);
        }
    }

    private static void EnqueueNextPoints(Map map,
                                          Point currentPoint,
                                          HashSet<Point> used,
                                          Queue<SinglyLinkedList<Point>> queue,
                                          SinglyLinkedList<Point> currentPath)
    {
        foreach (var nextPoint in GetNextPoints(map, currentPoint))
        {
            if (used.Add(nextPoint))
            {
                queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, currentPath));
            }
        }
    }

    private static IEnumerable<Point> GetNextPoints(Map map, Point currentPoint) =>
        _directions
            .Select(direction => new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y))
            .Where(nextPoint => map.InBounds(nextPoint) && map.Dungeon[nextPoint.X, nextPoint.Y] == MapCell.Empty);
}