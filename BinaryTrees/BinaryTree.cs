using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    public T Value { get; private set; }
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
        Value = item;
        Size = 1;
    }

    public void Add(T item)
    {
        if (Size == 0)
        {
            AddFirstValue(item);
            return;
        }

        Insert(item);
    }

    private void Insert(T item)
    {
        var current = this;
        while (true)
        {
            current.Size++;
            var compareToResult = item.CompareTo(current.Value);
            switch (compareToResult)
            {
                case < 0 when current.Left is null:
                    current.Left = new BinaryTree<T>(item);
                    return;
                case < 0:
                    current = current.Left;
                    break;
                case >= 0 when current.Right is null:
                    current.Right = new BinaryTree<T>(item);
                    return;
                case >= 0:
                    current = current.Right;
                    break;
            }
        }
    }

    public bool Contains(T item)
    {
        if (Size == 0)
        {
            return false;
        }
        
        var current = this;
        while (current is not null)
        {
            var compareToResult = item.CompareTo(current.Value);
            if (compareToResult == 0)
            {
                return true;
            }

            current = compareToResult < 0 ? current.Left : current.Right;
        }

        return false;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= Size)
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
        if (Size == 0)
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