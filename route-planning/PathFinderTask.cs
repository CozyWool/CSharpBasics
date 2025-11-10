using System;
using System.Drawing;

namespace RoutePlanning;

public static class PathFinderTask
{
    private static double minDistance = double.MaxValue;
    private static int[] best;

    public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
    {
        minDistance = double.MaxValue;
        var bestOrder = MakeTrivialPermutation(checkpoints);
        return bestOrder;
    }

    private static int[] MakeTrivialPermutation(Point[] checkpoints)
    {
        var bestOrder = new int[checkpoints.Length];
        for (var i = 0; i < bestOrder.Length; i++)
            bestOrder[i] = i;
        MakePermutations(checkpoints, bestOrder);
        return best;
    }

    static void MakePermutations(Point[] checkpoints, int[] permutation, int position = 0, double distance = 0)
    {
        if (position == permutation.Length)
        {
            var currentDistance = checkpoints.GetPathLength(permutation);
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                best = new int[permutation.Length];
                for (var i = 0; i < permutation.Length; i++)
                {
                    best[i] = permutation[i];
                }
            }
            return;
        }

        for (var i = 0; i < permutation.Length; i++)
        {
            var index = Array.IndexOf(permutation, i, 0, position);
            if (index != -1)
            {
                continue;
            }

            permutation[position] = i;
            if (position > 0)
            {
                var previous = checkpoints[permutation[position - 1]];
                var current = checkpoints[permutation[position]];
                distance += previous.DistanceTo(current);
            }

            if (distance >= minDistance)
            {
                break;
            }
            MakePermutations(checkpoints, permutation, position + 1);
        }
    }
}