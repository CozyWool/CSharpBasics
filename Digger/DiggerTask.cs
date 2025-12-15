using System;
using System.Windows.Input;
using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

public class Terrain : ICreature
{
    public string GetImageFileName()
    {
        return "Terrain.png";
    }

    public int GetDrawingPriority()
    {
        return 0;
    }

    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand();
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        return conflictedObject is Player;
    }
}

public class Player : ICreature
{
    public string GetImageFileName()
    {
        return "Digger.png";
    }

    public int GetDrawingPriority()
    {
        return -100;
    }

    public CreatureCommand Act(int x, int y)
    {
        var command = new CreatureCommand();
        switch (Game.KeyPressed)
        {
            case Key.Left when CanMove(x - 1, y):
                command.DeltaX = -1;
                break;
            case Key.Right when CanMove(x + 1, y):
                command.DeltaX = 1;
                break;
            case Key.Down when CanMove(x, y + 1):
                command.DeltaY = 1;
                break;
            case Key.Up when CanMove(x, y - 1):
                command.DeltaY = -1;
                break;
        }

        return command;
    }

    private static bool CanMove(int x, int y)
    {
        if ((x < 0 || x >= Game.MapWidth) || (y < 0 || y >= Game.MapHeight))
        {
            return false;
        }

        return Game.Map[x, y] is not Sack;
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        return conflictedObject is Sack or Monster or MonsterRandom;
    }
}

public class Sack : ICreature
{
    private int _passedCells = 0;

    public string GetImageFileName()
    {
        return "Sack.png";
    }

    public int GetDrawingPriority()
    {
        return -9;
    }

    public CreatureCommand Act(int x, int y)
    {
        var creatureCommand = new CreatureCommand();
        var canFall = y < Game.MapHeight - 1 &&
                      (Game.Map[x, y + 1] is null || (Game.Map[x, y + 1] is Player or Monster && _passedCells >= 1));
        if (canFall)
        {
            creatureCommand.DeltaY = 1;
            _passedCells++;
            return creatureCommand;
        }

        if (_passedCells > 1)
        {
            creatureCommand.TransformTo = new Gold();
        }
        else
        {
            _passedCells = 0;
        }

        return creatureCommand;
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        return false;
    }
}

public class Gold : ICreature
{
    public string GetImageFileName()
    {
        return "Gold.png";
    }

    public int GetDrawingPriority()
    {
        return -8;
    }

    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand();
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        if (conflictedObject is Player)
        {
            Game.Scores += 10;
            return true;
        }

        return conflictedObject is Monster or MonsterRandom;
    }
}

public abstract class MonsterBase : ICreature
{
    public abstract string GetImageFileName();

    public int GetDrawingPriority()
    {
        return -10;
    }

    public abstract CreatureCommand Act(int x, int y);

    protected static bool CanMove(int x, int y)
    {
        if ((x < 0 || x >= Game.MapWidth) || (y < 0 || y >= Game.MapHeight))
        {
            return false;
        }

        return Game.Map[x, y] is not Sack and not Monster and not Terrain and not MonsterRandom;
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        return conflictedObject is Sack or Monster or MonsterRandom;
    }
}

public class Monster : MonsterBase
{
    public override string GetImageFileName()
    {
        return "Monster.png";
    }

    public override CreatureCommand Act(int x, int y)
    {
        var creatureCommand = new CreatureCommand();
        var (playerX, playerY) = FindPlayer();
        if ((playerX, playerY) is (-1, -1))
        {
            return creatureCommand;
        }

        var deltaX = Math.Sign(playerX - x);
        var deltaY = Math.Sign(playerY - y);

        if (deltaY != 0 && CanMove(x, y + deltaY))
        {
            creatureCommand.DeltaY = deltaY;
        }
        else if (deltaX != 0 && CanMove(x + deltaX, y))
        {
            creatureCommand.DeltaX = deltaX;
        }

        return creatureCommand;
    }

    private static (int, int) FindPlayer()
    {
        for (var x = 0; x < Game.MapWidth; ++x)
        {
            for (var y = 0; y < Game.MapHeight; ++y)
            {
                if (Game.Map[x, y] is Player)
                {
                    return (x, y);
                }
            }
        }

        return (-1, -1);
    }
}

public class MonsterRandom : MonsterBase
{
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private readonly Random _random = new();
    private Direction? _currentDirection;

    public override string GetImageFileName()
    {
        return "MonsterRandom.png";
    }

    public override CreatureCommand Act(int x, int y)
    {
        var creatureCommand = new CreatureCommand();
        var deltaX = 0;
        var deltaY = 0;
        _currentDirection ??= (Direction) _random.Next(4);
        switch (_currentDirection)
        {
            case Direction.Left:
                deltaX++;
                break;
            case Direction.Right:
                deltaX--;
                break;
            case Direction.Up:
                deltaY--;
                break;
            case Direction.Down:
                deltaY++;
                break;
        }

        if (deltaY != 0 && CanMove(x, y + deltaY))
        {
            creatureCommand.DeltaY = deltaY;
        }
        else if (deltaX != 0 && CanMove(x + deltaX, y))
        {
            creatureCommand.DeltaX = deltaX;
        }
        else
        {
            _currentDirection = (Direction) _random.Next(4);
        }

        return creatureCommand;
    }
}