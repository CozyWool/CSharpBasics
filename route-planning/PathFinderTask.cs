using System;
using System.Drawing;

namespace RoutePlanning;

public static class PathFinderTask
{
    private static double _minDistance = double.MaxValue;
    private static int[] _bestOrder;

    public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
    {
        _minDistance = double.MaxValue;
        _bestOrder = new int[checkpoints.Length];
        var startOrder = new int[checkpoints.Length];
        for (var i = 0; i < startOrder.Length; i++)
        {
            startOrder[i] = i;
        }

        MakePermutations(checkpoints, startOrder, 1);
        return _bestOrder;
    }

    private static void MakePermutations(Point[] checkpoints, int[] permutation, int position = 0, double distance = 0)
    {
        if (position == permutation.Length)
        {
            UpdateBestOrder(permutation, distance);
            return;
        }

        for (var i = 1; i < permutation.Length; i++)
        {
            var index = Array.IndexOf(permutation, i, 0, position);
            if (index != -1)
            {
                continue;
            }

            permutation[position] = i;
            var newDistance = distance + CalculateNextDistance(checkpoints, permutation, position);
            if (newDistance >= _minDistance)
            {
                break;
            }

            MakePermutations(checkpoints, permutation, position + 1, newDistance);
        }
    }

    private static void UpdateBestOrder(int[] permutation, double distance)
    {
        _minDistance = distance;
        for (var i = 0; i < permutation.Length; i++)
        {
            _bestOrder[i] = permutation[i];
        }
    }

    private static double CalculateNextDistance(Point[] checkpoints, int[] permutation, int position)
    {
        var previous = checkpoints[permutation[position - 1]];
        var current = checkpoints[permutation[position]];
        var nextDistance = previous.DistanceTo(current);
        return nextDistance;
    }
}