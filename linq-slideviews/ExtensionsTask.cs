using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    /// <summary>
    /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
    /// Медиана списка из четного количества элементов — это среднее арифметическое
    /// двух серединных элементов списка после сортировки.
    /// </summary>
    /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
    public static double Median(this IEnumerable<double> items)
    {
        var listItems = items.OrderBy(x => x).ToList();
        var count = listItems.Count;
        if (count == 0)
        {
            throw new InvalidOperationException();
        }

        var middle = count / 2;
        if (count % 2 == 0)
        {
            return (listItems[middle] + listItems[middle - 1]) / 2;
        }

        return listItems[middle];
    }

    /// <returns>
    /// Возвращает последовательность, состоящую из пар соседних элементов.
    /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
    /// </returns>
    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        var previous = default(T);
        var hasPrevious = false;
        foreach (var item in items)
        {
            if (hasPrevious)
            {
                yield return (previous, item);
            }

            previous = item;
            hasPrevious = true;
        }
    }
}