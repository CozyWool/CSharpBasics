using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete;

public class RightBorderTask
{
    /// <returns>
    /// Возвращает индекс правой границы.
    /// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
    /// Если такого нет, то возвращает items.Length
    /// </returns>
    public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
    {
        while (right - left > 1)
        {
            var m = left + (right - left) / 2;
            if (string.Compare(prefix, phrases[m], StringComparison.InvariantCultureIgnoreCase) < 0 &&
                !phrases[m].StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
            {
                right = m;
            }
            else
            {
                left = m;
            }
        }

        return right;
    }
}