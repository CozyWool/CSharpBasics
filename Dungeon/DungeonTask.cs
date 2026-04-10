using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
    public static MoveDirection[] FindShortestPath(Map map)
    {
        var startToChest = BfsTask.FindPaths(map, map.InitialPosition, map.Chests)
                                  .ToDictionary(path => path.Value);
        if (map.Chests.Length == 0 || startToChest.Count == 0)
        {
            return FindPathWithoutChests(map);
        }

        var bestPath = FindBestPathThroughChests(map, startToChest);
        return bestPath is null ? [] : ParseToMoveDirections(bestPath);
    }

    private static List<Point>? FindBestPathThroughChests(Map map, Dictionary<Point, SinglyLinkedList<Point>> startDict)
    {
        var endDict = BfsTask.FindPaths(map, map.Exit, map.Chests)
                             .ToDictionary(path => path.Value);

        SinglyLinkedList<Point>? bestStart = null;
        SinglyLinkedList<Point>? bestEnd = null;
        var bestLength = int.MaxValue;
        var bestValue = -1;

        foreach (var chest in map.Chests
                                 .Where(c => startDict.ContainsKey(c.Location) && endDict.ContainsKey(c.Location)))
        {
            var startPath = startDict[chest.Location];
            var endPath = endDict[chest.Location];

            var length = startPath.Length + endPath.Length - 1;

            if (length < bestLength ||
                (length == bestLength && chest.Value > bestValue))
            {
                bestLength = length;
                bestValue = chest.Value;
                bestStart = startPath;
                bestEnd = endPath;
            }
        }

        return bestStart?.Reverse()
                        .Concat(bestEnd.Skip(1))
                        .ToList();
    }

    private static MoveDirection[] FindPathWithoutChests(Map map)
    {
        var bestPath = BfsTask.FindPaths(map, map.InitialPosition, [new Chest(map.Exit, 0)])
                              .ToList()
                              .MinBy(x => x.Count());
        return bestPath is null ? [] : ParseToMoveDirections(bestPath.Reverse().ToList());
    }

    private static MoveDirection[] ParseToMoveDirections(List<Point> bestPath)
    {
        return bestPath
               .Zip(bestPath.Skip(1), (from, to) => Walker.ConvertOffsetToDirection(to - from))
               .ToArray();
    }
}