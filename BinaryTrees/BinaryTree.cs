using System;

namespace BinaryTrees;

public class BinaryTree<T> where T : IComparable
{
    private class Node(T value)
    {
        public readonly T Value = value;
        public Node? Left;
        public Node? Right;
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
}