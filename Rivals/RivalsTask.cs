using System.Collections.Generic;
using System.Linq;

namespace Rivals;

public class RivalsTask
{
    public static readonly IReadOnlyList<Point> PossibleDirections = [new(0, -1), new(0, 1), new(-1, 0), new(1, 0)];

    public static IEnumerable<OwnedLocation> AssignOwners(Map map)
    {
        var used = new HashSet<Point>();
        var queue = InitQueue(map, used);

        return BreadthFirstSearch(map, queue, used);
    }

    private static IEnumerable<OwnedLocation> BreadthFirstSearch(Map map, Queue<OwnedLocation> queue,
                                                                 HashSet<Point> used)
    {
        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();
            yield return currentPoint;

            foreach (var nextPoint in GetNextPoints(map, currentPoint.Location))
            {
                if (used.Add(nextPoint))
                {
                    var ownedLocation = new OwnedLocation(currentPoint.Owner, nextPoint, currentPoint.Distance + 1);
                    queue.Enqueue(ownedLocation);
                }
            }
        }
    }

    private static Queue<OwnedLocation> InitQueue(Map map, HashSet<Point> used)
    {
        var queue = new Queue<OwnedLocation>();
        for (var i = 0; i < map.Players.Length; i++)
        {
            var player = map.Players[i];
            var ownedLocation = new OwnedLocation(i, player, 0);
            queue.Enqueue(ownedLocation);
            used.Add(player);
        }

        return queue;
    }

    private static IEnumerable<Point> GetNextPoints(Map map, Point currentPoint)
    {
        if (map.Chests.Contains(currentPoint))
        {
            return [];
        }

        return PossibleDirections
               .Select(direction => new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y))
               .Where(nextPoint => map.InBounds(nextPoint) && map.Maze[nextPoint.X, nextPoint.Y] == MapCell.Empty);
    }
}