using System.Linq;

namespace Dungeon;

public class DungeonTask
{
    public static MoveDirection[] FindShortestPath(Map map)
    {
        var fromStart = BfsTask.FindPaths(map, map.InitialPosition, map.Chests).ToList();
        var fromEnd = BfsTask.FindPaths(map, map.Exit, map.Chests).ToList();
        var joined = fromStart.Join(fromEnd,
                                    x => x.Last(),
                                    x => x.First(),
                                    (start, end) => start.Concat(end.Skip(1)));
        return [];
    }
}