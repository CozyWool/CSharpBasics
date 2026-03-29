using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
    public static MoveDirection[] FindShortestPath(Map map)
    {
        // var startToEnd = BfsTask.FindPaths(map, map.Exit, [new Chest(map.Exit, 0)]).ToList();
        // if (startToEnd.Any(x => map.Chests.Any(c => c.Location.Equals(x.Value))))
        // {
        //     return ParseToMoveDirections(startToEnd.First().ToList());
        // }

        var startToChest = BfsTask.FindPaths(map, map.InitialPosition, map.Chests).ToList();
        if (map.Chests.Length == 0 || startToChest.Count == 0)
        {
            return FindPathWithoutChests(map);
        }

        var bestPath = FindBestPathThroughChests(map, startToChest);
        return bestPath is null ? [] : ParseToMoveDirections(bestPath);
    }

    private static List<Point>? FindBestPathThroughChests(Map map, List<SinglyLinkedList<Point>> fromStart)
    {
        var endToChest = BfsTask.FindPaths(map, map.Exit, map.Chests);
        var joined = fromStart.Join(endToChest,
                                    x => x.First(),
                                    x => x.First(),
                                    (start, end) =>
                                        new
                                        {
                                            Path = start.Reverse().Concat(end.Skip(1)).ToList(),
                                            ChestLocation = start.First()
                                        });

        var chestDict = map.Chests.ToDictionary(c => c.Location, c => c.Value);
        var bestPath = joined
                       .OrderBy(x => x.Path.Count)
                       .ThenByDescending(x => chestDict[x.ChestLocation])
                       .FirstOrDefault();
        return bestPath?.Path;
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
               .Zip(bestPath.Skip(1), GetDirection)
               .ToArray();
    }

    private static MoveDirection GetDirection(Point from, Point to)
    {
        switch (to.X - from.X)
        {
            case 1:
                return MoveDirection.Right;
            case -1:
                return MoveDirection.Left;
        }

        switch (to.Y - from.Y)
        {
            case 1:
                return MoveDirection.Down;
            case -1:
                return MoveDirection.Up;
        }

        throw new ArgumentException("Invalid move");
    }
}