using System.Collections.Generic;

namespace Clones;

public class ProgramStack
{
    private Item _current;

    public ProgramStack()
    {
    }

    public ProgramStack(ProgramStack other) =>
        _current = other._current;

    public void Clear() => _current = null;

    public int Pop()
    {
        var value = _current.Value;
        _current = _current.Previous;
        return value;
    }

    public void Push(int value) => _current = new Item(value, _current);

    public int? Peek() => _current?.Value;
}

public class Item(int value, Item previous)
{
    public int Value { get; set; } = value;
    public Item Previous { get; set; } = previous;
}

public class Clone
{
    private ProgramStack _rollbackHistory = new();
    private ProgramStack _learnHistory = new();

    public Clone()
    {
    }

    public Clone(ProgramStack learnHistory, ProgramStack rollbackHistory)
    {
        _learnHistory = new ProgramStack(learnHistory);
        _rollbackHistory = new ProgramStack(rollbackHistory);
    }

    public void Learn(int programNumber)
    {
        _rollbackHistory.Clear();
        _learnHistory.Push(programNumber);
    }

    public string Check()
    {
        var programNumber = _learnHistory.Peek();
        return programNumber is not null ? programNumber.ToString() : "basic";
    }

    public void Rollback()
    {
        var programNumber = _learnHistory.Pop();
        _rollbackHistory.Push(programNumber);
    }

    public void Relearn()
    {
        var programNumber = _rollbackHistory.Pop();
        _learnHistory.Push(programNumber);
    }

    public Clone MakeClone()
    {
        var clone = new Clone(_learnHistory, _rollbackHistory);
        return clone;
    }
}

public class CloneVersionSystem : ICloneVersionSystem
{
    private readonly List<Clone> _clones = [new Clone()];

    public string Execute(string query)
    {
        var arguments = query.Split(' ');
        var cloneNumber = int.Parse(arguments[1]) - 1;
        var clone = _clones[cloneNumber];

        switch (arguments[0])
        {
            case "learn":
                var programNumber = int.Parse(arguments[2]);
                clone.Learn(programNumber);
                break;
            case "rollback":
                clone.Rollback();
                break;
            case "relearn":
                clone.Relearn();
                break;
            case "clone":
                _clones.Add(clone.MakeClone());
                break;
            case "check":
                return clone.Check();
        }

        return null;
    }
}