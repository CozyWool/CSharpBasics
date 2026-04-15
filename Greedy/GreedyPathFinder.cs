using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var pathFinder = new DijkstraPathFinder();
        var resultPath = new List<Point>();
        var remainingGoals = state.Goal;
        
        while (remainingGoals > 0)
        {
            var path = pathFinder.GetPathsByDijkstra(state, state.Position, state.Chests)
                                 .FirstOrDefault();

            if (path is null || path.Cost > state.Energy)
            {
                return [];
            }

            resultPath.AddRange(path.Path.Skip(1));
            state.Position = path.End;
            state.Energy -= path.Cost;
            state.Chests.Remove(path.End);
            remainingGoals--;
        }

        return resultPath;
    }
}