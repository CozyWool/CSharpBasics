using System.Collections.Generic;

namespace Clones;

public class ProgramStack
{
    private readonly LinkedList<int> _linkedList;

    public ProgramStack() : this([])
    {
    }

    public ProgramStack(LinkedList<int> linkedList)
    {
        _linkedList = linkedList;
    }

    public void Push(int item)
    {
        _linkedList.AddLast(item);
    }

    public int Pop()
    {
        if (_linkedList.Last != null)
        {
            var item = _linkedList.Last.Value;
            _linkedList.RemoveLast();
            return item;
        }

        return -1;
    }

    public object Peek()
    {
        var item = _linkedList.Last?.Value;
        return item;
    }

    public void Clear()
    {
        _linkedList.Clear();
    }

    public int Count => _linkedList.Count;

    public ProgramStack Clone()
    {
        var linkedList = new LinkedList<int>();
        foreach (var item in _linkedList)
        {
            linkedList.AddLast(item);
        }

        return new ProgramStack(linkedList);
    }
}

public class Clone
{
    private ProgramStack _rollbackHistory = new();
    private ProgramStack _learnHistory = new();

    public void Learn(int programNumber)
    {
        _learnHistory.Push(programNumber);
        _rollbackHistory.Clear();
    }

    public string Check()
    {
        var item = _learnHistory.Peek();
        if (item is int programNumber)
        {
            return programNumber.ToString();
        }

        return "basic";
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
        var clone = new Clone
                    {
                        _learnHistory = _learnHistory.Clone(),
                        _rollbackHistory = _rollbackHistory.Clone()
                    };
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