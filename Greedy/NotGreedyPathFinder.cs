using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    private Dictionary<Point, Dictionary<Point, PathWithCost>> _chestGraph;
    private List<Point> _bestPath;
    private int _maxChestsFound;

    public List<Point> FindPathToCompleteGoal(State state)
    {
        var pathFinder = new DijkstraPathFinder();
        var chests = state.Chests.ToList();

        List<Point> allPoints = [state.Position];
        allPoints.AddRange(chests);
        _chestGraph = new Dictionary<Point, Dictionary<Point, PathWithCost>>();

        foreach (var startPoint in allPoints)
        {
            _chestGraph[startPoint] = pathFinder
                                    .GetPathsByDijkstra(state, startPoint, chests.Where(c => c != startPoint))
                                    .ToDictionary(p => p.End, p => p);
        }

        _bestPath = [];
        _maxChestsFound = 0;

        Search(state.Position, state.Energy, [], [], chests);

        return _bestPath;
    }

    private void Search(Point currentPos,
                        int energyLeft,
                        HashSet<Point> visitedChests,
                        List<Point> currentPath,
                        List<Point> chests)
    {
        UpdateBest(visitedChests, currentPath);

        foreach (var chest in chests)
        {
            if (visitedChests.Contains(chest)
             || !_chestGraph[currentPos].TryGetValue(chest, out var pathToChest)
             || energyLeft < pathToChest.Cost)
            {
                continue;
            }

            var addedPoints = pathToChest.Path.Skip(1).ToList();
            currentPath.AddRange(addedPoints);
            visitedChests.Add(chest);

            Search(chest, energyLeft - pathToChest.Cost, visitedChests, currentPath, chests);

            currentPath.RemoveRange(currentPath.Count - addedPoints.Count, addedPoints.Count);
            visitedChests.Remove(chest);
        }
    }

    private void UpdateBest(HashSet<Point> visitedChests, List<Point> currentPath)
    {
        if (visitedChests.Count > _maxChestsFound)
        {
            _maxChestsFound = visitedChests.Count;
            _bestPath = currentPath.ToList();
        }
    }
}