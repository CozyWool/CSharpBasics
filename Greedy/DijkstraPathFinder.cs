using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class DijkstraPathFinder
{
    private static readonly IReadOnlyList<Point> _possibleDirections = [new(0, -1), new(0, 1), new(-1, 0), new(1, 0)];

    private record DijkstraData(int Price, Point? Previous);

    public IEnumerable<PathWithCost> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
    {
        var targetsSet = targets.ToHashSet();
        var track = new Dictionary<Point, DijkstraData> {[start] = new(0, null)};
        var visited = new HashSet<Point>();
        var queue = new PriorityQueue<Point, int>();

        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            var toOpen = queue.Dequeue();

            if (!visited.Add(toOpen))
            {
                continue;
            }

            if (targetsSet.Contains(toOpen))
            {
                yield return BuildPath(track, toOpen);
            }

            ProcessNextPoints(state, toOpen, track, queue);
        }
    }

    private static void ProcessNextPoints(State state, Point toOpen, Dictionary<Point, DijkstraData> track,
                                          PriorityQueue<Point, int> queue)
    {
        foreach (var next in GetNextPoints(state, toOpen))
        {
            var currentPrice = track[toOpen].Price + state.CellCost[next.X, next.Y];

            if (track.TryGetValue(next, out var oldData) && oldData.Price <= currentPrice)
            {
                continue;
            }

            oldData = new DijkstraData(currentPrice, toOpen);
            track[next] = oldData;
            queue.Enqueue(next, currentPrice);
        }
    }

    private static PathWithCost BuildPath(Dictionary<Point, DijkstraData> track, Point end)
    {
        var path = new List<Point>();
        Point? current = end;

        while (current is not null)
        {
            path.Add(current.Value);
            current = track[current.Value].Previous;
        }

        path.Reverse();

        return new PathWithCost(track[end].Price, path.ToArray());
    }

    private static IEnumerable<Point> GetNextPoints(State state, Point currentPoint) =>
        _possibleDirections
            .Select(direction => new Point(currentPoint.X + direction.X, currentPoint.Y + direction.Y))
            .Where(nextPoint => state.InsideMap(nextPoint) && !state.IsWallAt(nextPoint));
}