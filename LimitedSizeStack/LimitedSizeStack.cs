using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private readonly int _undoLimit;
    private LinkedList<T> _linkedList;

    public LimitedSizeStack(int undoLimit)
    {
        _undoLimit = undoLimit;
        _linkedList = new LinkedList<T>();
    }

    public void Push(T item)
    {
        _linkedList.AddLast(item);
        if (_linkedList.Count > _undoLimit)
        {
            _linkedList.RemoveFirst();
        }
    }

    public T Pop()
    {
        var item = _linkedList.Last.Value;
        _linkedList.RemoveLast();
        return item;
    }

    public int Count => _linkedList.Count;
}