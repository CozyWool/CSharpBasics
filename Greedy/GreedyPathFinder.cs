using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
    private class GreedySearchState(State state)
    {
        public Point Position { get; private set; } = state.Position;
        private int Energy { get; set; } = state.Energy;
        public int RemainingGoals { get; private set; } = state.Goal;
        public HashSet<Point> RemainingChests { get; } = [..state.Chests];

        public void Update(PathWithCost path)
        {
            Position = path.End;
            Energy -= path.Cost;
            RemainingChests.Remove(path.End);
            RemainingGoals--;
        }

        public bool IsPathPossible(PathWithCost? path) => path is not null && path.Cost <= Energy;
    }

    public List<Point> FindPathToCompleteGoal(State state)
    {
        var pathFinder = new DijkstraPathFinder();
        var searchState = new GreedySearchState(state);
        var resultPath = new List<Point>();

        while (searchState.RemainingGoals > 0)
        {
            var path = pathFinder.GetPathsByDijkstra(state, searchState.Position, searchState.RemainingChests)
                                 .FirstOrDefault();

            if (!searchState.IsPathPossible(path))
            {
                return [];
            }

            resultPath.AddRange(path!.Path.Skip(1));
            searchState.Update(path);
        }

        return resultPath;
    }
}