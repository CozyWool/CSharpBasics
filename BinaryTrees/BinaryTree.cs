using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    private bool _hasValue;
    public T? Value { get; private set; }
    public BinaryTree<T>? Left { get; private set; }
    public BinaryTree<T>? Right { get; private set; }
    public int Size { get; private set; }

    public BinaryTree()
    {
    }

    private BinaryTree(T item)
    {
        AddFirstValue(item);
    }

    private void AddFirstValue(T item)
    {
        _hasValue = true;
        Value = item;
        Size = 1;
    }

    public void Add(T item)
    {
        if (!_hasValue)
        {
            AddFirstValue(item);
            return;
        }

        Insert(item);
    }

    private void Insert(T item)
    {
        var compareToResult = item.CompareTo(Value);
        switch (compareToResult)
        {
            case < 0 when Left is null:
                Left = new BinaryTree<T>(item);
                break;
            case < 0:
                Left.Add(item);
                break;
            case >= 0 when Right is null:
                Right = new BinaryTree<T>(item);
                break;
            case >= 0:
                Right.Add(item);
                break;
        }

        Size = 1 + (Left?.Size ?? 0) + (Right?.Size ?? 0);
    }

    public bool Contains(T item)
    {
        if (!_hasValue)
        {
            return false;
        }

        var compareToResult = item.CompareTo(Value);
        if (compareToResult == 0)
        {
            return true;
        }

        return compareToResult < 0
                   ? Left?.Contains(item) ?? false
                   : Right?.Contains(item) ?? false;
    }

    public T this[int index]
    {
        get
        {
            if (!_hasValue || index < 0 || index >= Size)
            {
                throw new IndexOutOfRangeException($"Index was {index}");
            }

            var leftSize = Left?.Size ?? 0;
            if (index < leftSize)
            {
                return Left[index];
            }

            if (index == leftSize)
            {
                return Value;
            }

            return Right[index - leftSize - 1];
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (!_hasValue)
        {
            yield break;
        }

        if (Left is not null)
        {
            foreach (var leftValue in Left)
            {
                yield return leftValue;
            }
        }

        yield return Value;

        if (Right is not null)
        {
            foreach (var rightValue in Right)
            {
                yield return rightValue;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}