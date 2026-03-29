using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dungeon.Dungeons;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Dungeon;

[TestFixture]
public class Dungeon_Should
{
    [Test]
    public void ReturnPathToExit_IfChestIsUnreachable()
    {
        var textMap = new[]
                      {
                          "PE#",
                          "###",
                          "0  "
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new[] {MoveDirection.Right}, path);
    }

    [Test]
    public void ReturnPathToExit_IfNoChests()
    {
        var textMap = new[]
                      {
                          "PE",
                          "  "
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new[] {MoveDirection.Right}, path);
    }

    [Test]
    public void ReturnEmptyPath_WhenNoPathsToChestAndExit()
    {
        var textMap = new[]
                      {
                          "P ",
                          "##",
                          "0E"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new MoveDirection[0], path);
    }

    [Test]
    public void ReturnEmptyPath_WhenHasPathToChestButNoPathToExit()
    {
        var textMap = new[]
                      {
                          "P0",
                          "##",
                          "0E"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new MoveDirection[0], path);
    }

    [Test]
    public void ReturnEmptyPath_WhenNoPathToExit()
    {
        var textMap = new[]
                      {
                          "P#",
                          "#E"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new MoveDirection[0], path);
    }

    [Test]
    public void ReturnCorrectPath_OnEmptyDungeon()
    {
        var textMap = new[]
                      {
                          "P ",
                          "0E"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new[] {MoveDirection.Down, MoveDirection.Right}, path);
    }

    [Test]
    public void ReturnCorrectPath_OnSimpleDungeon()
    {
        var textMap = new[]
                      {
                          "P #",
                          "#6#",
                          "E  "
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new[] {MoveDirection.Right, MoveDirection.Down, MoveDirection.Down, MoveDirection.Left}, path);
    }

    [Test]
    public void ReturnCorrectPath_WhenMultiplePaths_OnEmptyDungeon()
    {
        var textMap = new[]
                      {
                          "P  ",
                          "45 ",
                          "E  "
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new[] {MoveDirection.Down, MoveDirection.Down}, path);
    }

    [Test]
    public void ReturnCorrectPath_WhenMultiplePaths_OnSimpleDungeon()
    {
        var textMap = new[]
                      {
                          "P 5",
                          "#7 ",
                          "##E"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new[] {MoveDirection.Right, MoveDirection.Down, MoveDirection.Right, MoveDirection.Down}, path);
    }

    [Test]
    public void ReturnCorrectPath_WhenMultiplePaths_OnHardDungeon()
    {
        var textMap = new[]
                      {
                          "P#9",
                          " 56",
                          "# E"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new[] {MoveDirection.Down, MoveDirection.Right, MoveDirection.Right, MoveDirection.Down}, path);
    }

    [Test]
    public void ReturnCorrectPath_WhenMultipleEqualPaths()
    {
        var textMap = new[]
                      {
                          "P##",
                          " 66",
                          "66E"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        var expected = new[]
                       {
                           new[] {MoveDirection.Down, MoveDirection.Right, MoveDirection.Right, MoveDirection.Down},
                           new[] {MoveDirection.Down, MoveDirection.Right, MoveDirection.Down, MoveDirection.Right},
                           new[] {MoveDirection.Down, MoveDirection.Down, MoveDirection.Right, MoveDirection.Right}
                       };

        Assert.Contains(path, expected);
    }

    [Test]
    public void Return_ShortestPath1()
    {
        var textMap = new[]
                      {
                          "   ",
                          " P9",
                          " 0E"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        Assert.AreEqual(new[] {MoveDirection.Right, MoveDirection.Down}, path);
    }

    [Test]
    public void Return_ShortestPath2()
    {
        var textMap = new[]
                      {
                          "E59",
                          "5P ",
                          "999"
                      };
        var map = Map.FromLines(textMap);

        var path = DungeonTask.FindShortestPath(map);

        var expected = new[]
                       {
                           new[] {MoveDirection.Left, MoveDirection.Up},
                           new[] {MoveDirection.Up, MoveDirection.Left}
                       };

        Assert.Contains(path, expected);
    }

    [Test]
    public void ReturnShortestPath3()
    {
        var map = Map.FromText(DungeonsLoader.Load(DungeonsName.BigDungeon));
        var expectedLength = 211;

        var path = DungeonTask.FindShortestPath(map);

        IsValidPath(map, path, expectedLength);
    }

    private void IsValidPath(Map map, MoveDirection[] path, int expectedPathLength)
    {
        var chestTaken = false;
        var chestsPoints = new HashSet<Point>(map.Chests.Select(c => c.Location));
        var walker = new Walker(map.InitialPosition);
        foreach (var step in path)
        {
            walker = walker.WalkInDirection(map, step);
            if (!walker.PointOfCollision.IsNull)
            {
                Assert.Fail($"Collided with wall at {walker.PointOfCollision}");
            }

            if (chestsPoints.Contains(walker.Position))
            {
                chestTaken = true;
            }
        }

        Assert.True(chestTaken, "Player did not take any chest.");
        Assert.AreEqual(map.Exit, walker.Position, "Player did not reach the exit.");
        Assert.GreaterOrEqual(path.Length, expectedPathLength,
                              "Hmm.... Seems to be an error in the checker. Please notify us in the comments below.");
        Assert.AreEqual(expectedPathLength, path.Length, "Path must be shortest.");
    }

    [Test]
    public void LargeZigzag()
    {
        var rnd = new Random();
        var textMap = new string[121];
        textMap[0] = 'P' + new string('2', 59);
        for (var i = 1; i < 118; i += 4)
        {
            textMap[i] = new string('#', 59) + rnd.Next(0, 9).ToString().ToCharArray()[0];
            textMap[i + 1] = new string(rnd.Next(0, 9).ToString().ToCharArray()[0], 60);
            textMap[i + 2] = '2' + new string('#', 59);
            textMap[i + 3] = new string(rnd.Next(0, 9).ToString().ToCharArray()[0], 60);
        }

        textMap[120] = new string('5', 59) + 'E';
        var map = Map.FromLines(textMap);
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var path = DungeonTask.FindShortestPath(map);
        stopwatch.Stop();
        Console.WriteLine($"{path.Length} - {stopwatch.Elapsed}");
    }
}