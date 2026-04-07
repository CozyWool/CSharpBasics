using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
    {
        var chestsLocations = chests.Select(c => c.Location).ToHashSet();
        foreach (var path in EnumeratePaths(map, start))
        {
            if (chestsLocations.Remove(path.Value))
            {
                yield return path;
            }

            if (chestsLocations.Count == 0)
            {
                yield break;
            }
        }
    }

    private static IEnumerable<SinglyLinkedList<Point>> EnumeratePaths(Map map, Point start)
    {
        var used = new HashSet<Point> {start};
        var queue = new Queue<SinglyLinkedList<Point>>();
        queue.Enqueue(new SinglyLinkedList<Point>(start));

        while (queue.Count > 0)
        {
            var currentPath = queue.Dequeue();
            var currentPoint = currentPath.Value;
            yield return currentPath;

            foreach (var nextPoint in GetNextPoints(map, currentPoint))
            {
                if (used.Add(nextPoint))
                {
                    queue.Enqueue(new SinglyLinkedList<Point>(nextPoint, currentPath));
                }
            }
        }
    }

    private static IEnumerable<Point> GetNextPoints(Map map, Point currentPoint) =>
        Walker.PossibleDirections
              .Select(direction => new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y))
              .Where(nextPoint => map.InBounds(nextPoint) && map.Dungeon[nextPoint.X, nextPoint.Y] == MapCell.Empty);
}