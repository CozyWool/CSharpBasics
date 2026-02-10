using System.Collections.Generic;

namespace Clones;

public class LinkedListStack<T>
{
    private readonly LinkedList<T> _linkedList;

    public LinkedListStack() : this([])
    {
    }

    public LinkedListStack(LinkedList<T> linkedList)
    {
        _linkedList = linkedList;
    }

    public void Push(T item)
    {
        _linkedList.AddLast(item);
    }

    public T Pop()
    {
        var item = _linkedList.Last.Value;
        _linkedList.RemoveLast();
        return item;
    }

    public object Peek()
    {
        if (_linkedList.Last is null)
        {
            return null;
        }

        var item = _linkedList.Last.Value;
        return item;
    }

    public void Clear()
    {
        _linkedList.Clear();
    }

    public int Count => _linkedList.Count;

    public LinkedListStack<T> Clone()
    {
        var linkedList = new LinkedList<T>();
        foreach (var item in _linkedList)
        {
            linkedList.AddLast(item);
        }

        return new LinkedListStack<T>(linkedList);
    }
}

public class Clone
{
    private LinkedListStack<LearnCommand> _rollbackHistory = new();
    private LinkedListStack<LearnCommand> _learnHistory = new();

    public void AddLearnCommand(LearnCommand learnCommand)
    {
        _learnHistory.Push(learnCommand);
    }

    public void RemoveLastLearnCommand()
    {
        _learnHistory.Pop();
    }

    public void AddRollbackCommand(LearnCommand learnCommand)
    {
        _rollbackHistory.Push(learnCommand);
    }

    public void RemoveLastRollbackCommand()
    {
        _rollbackHistory.Pop();
    }

    public void Learn(int programNumber)
    {
        var command = new LearnCommand(this, programNumber);
        command.Execute();
        _rollbackHistory.Clear();
    }

    public string Check()
    {
        var item = _learnHistory.Peek();
        if (item is LearnCommand command)
        {
            return command.ProgramNumber.ToString();
        }

        return "basic";
    }

    public void Rollback()
    {
        var command = _learnHistory.Peek() as LearnCommand;
        command.Rollback();
    }

    public void Relearn()
    {
        var command = _rollbackHistory.Peek() as LearnCommand;
        command.Relearn();
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

public class LearnCommand
{
    private readonly Clone _learningClone;
    public int ProgramNumber { get; set; }

    public LearnCommand(Clone learningClone, int programNumber)
    {
        _learningClone = learningClone;
        ProgramNumber = programNumber;
    }

    public void Execute()
    {
        _learningClone.AddLearnCommand(this);
    }

    public void Rollback()
    {
        _learningClone.RemoveLastLearnCommand();
        _learningClone.AddRollbackCommand(this);
    }

    public void Relearn()
    {
        Execute();
        _learningClone.RemoveLastRollbackCommand();
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