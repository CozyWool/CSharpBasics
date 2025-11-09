using System.Drawing;

namespace RoutePlanning;

public static class PathFinderTask
{
	public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
	{
		var bestOrder = MakeTrivialPermutation(checkpoints);
		return bestOrder;
	}

	private static int[] MakeTrivialPermutation(Point[] checkpoints)
	{
		var bestOrder = new int[checkpoints];
		for (var i = 0; i < bestOrder.Length; i++)
			bestOrder[i] = i;
		return bestOrder;
	}
}