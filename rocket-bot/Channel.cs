using System.Collections.Generic;

namespace rocket_bot;

public class Channel<T> where T : class
{
    private readonly List<T> _items = [];

    /// <summary>
    /// Возвращает элемент по индексу или null, если такого элемента нет.
    /// При присвоении удаляет все элементы после.
    /// Если индекс в точности равен размеру коллекции, работает как Append.
    /// </summary>
    public T this[int index]
    {
        get
        {
            lock (_items)
            {
                if (index >= 0 && index < _items.Count)
                {
                    return _items[index];
                }

                return null;
            }
        }
        set
        {
            lock (_items)
            {
                if (index >= 0 && index < _items.Count)
                {
                    _items[index] = value;
                    _items.RemoveRange(index + 1, _items.Count - (index + 1));
                }

                if (index == _items.Count)
                {
                    _items.Add(value);
                }
            }
        }
    }

    /// <summary>
    /// Возвращает последний элемент или null, если такого элемента нет
    /// </summary>
    public T LastItem()
    {
        lock (_items)
        {
            return _items.Count > 0 ? _items[^1] : null;
        }
    }

    /// <summary>
    /// Добавляет item в конец только если lastItem является последним элементом
    /// </summary>
    public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
    {
        lock (_items)
        {
            if (LastItem() != knownLastItem)
            {
                return;
            }

            _items.Add(item);
        }
    }

    /// <summary>
    /// Возвращает количество элементов в коллекции
    /// </summary>
    public int Count
    {
        get
        {
            lock (_items)
            {
                return _items.Count;
            }
        }
    }
}