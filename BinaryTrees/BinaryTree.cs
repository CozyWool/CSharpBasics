using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    private class Node(T value)
    {
        public readonly T Value = value;
        public Node? Left;
        public Node? Right;
        public int Size = 1;
    }

    private Node? _root;

    public void Add(T item)
    {
        if (_root is null)
        {
            _root = new Node(item);
            return;
        }

        Insert(item);
    }

    private void Insert(T item)
    {
        var current = _root;
        while (true)
        {
            current.Size++;
            var compareToResult = item.CompareTo(current.Value);
            switch (compareToResult)
            {
                case < 0 when current.Left is null:
                    current.Left = new Node(item);
                    return;
                case < 0:
                    current = current.Left;
                    break;
                case >= 0 when current.Right is null:
                    current.Right = new Node(item);
                    return;
                case >= 0:
                    current = current.Right;
                    break;
            }
        }
    }

    public bool Contains(T item)
    {
        var current = _root;
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
            if (index < 0 || index >= _root.Size)
            {
                throw new IndexOutOfRangeException($"Index was {index}");
            }

            return GetByIndex(_root, index);
        }
    }

    private T GetByIndex(Node node, int index)
    {
        var leftSize = node.Left?.Size ?? 0;

        if (index < leftSize)
        {
            return GetByIndex(node.Left, index);
        }

        if (index == leftSize)
        {
            return node.Value;
        }

        return GetByIndex(node.Right, index - leftSize - 1);
    }

    public IEnumerator<T> GetEnumerator() => EnumerateTree(_root).GetEnumerator();

    private IEnumerable<T> EnumerateTree(Node? node)
    {
        if (node is null)
        {
            yield break;
        }

        foreach (var leftValue in EnumerateTree(node.Left))
        {
            yield return leftValue;
        }

        yield return node.Value;

        foreach (var rightValue in EnumerateTree(node.Right))
        {
            yield return rightValue;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}