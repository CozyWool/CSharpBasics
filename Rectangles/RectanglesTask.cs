using System;

namespace Rectangles;

public static class RectanglesTask
{
    /// <summary>
    /// Проверяет пересечение двух прямоугольников.
    /// </summary>
    /// <param name="r1">Первый прямоугольник.</param>
    /// <param name="r2">Второй прямоугольник.</param>
    /// <returns>
    /// Возвращает true, если прямоугольники пересекаются, иначе false.
    /// </returns>
    public static bool AreIntersected(Rectangle r1, Rectangle r2)
    {
        return r1.Left <= r2.Right && r2.Left <= r1.Right &&
               r1.Top <= r2.Bottom && r2.Top <= r1.Bottom;
    }

    /// <param name="r1">Первый прямоугольник</param>
    /// <param name="r2">Второй прямоугольник</param>
    /// <returns>Площадь пересечения двух прямоугольников</returns>
    public static int IntersectionSquare(Rectangle r1, Rectangle r2)
    {
        if (!AreIntersected(r1, r2))
        {
            return 0;
        }

        var x = GetLength(r1.Left, r1.Right, r2.Left, r2.Right);
        var y = GetLength(r1.Top, r1.Bottom, r2.Top, r2.Bottom);

        return x * y;
    }

    private static int GetLength(int r1Start, int r1End, int r2Start, int r2End)
    {
        return Math.Min(r1End, r2End) - Math.Max(r1Start, r2Start);
    }

    /// <summary>
    /// Определяет вложенность одного прямоугольника в другой
    /// </summary>
    /// <param name="r1">Первый прямоугольник</param>
    /// <param name="r2">Второй прямоугольник</param>
    /// <returns>
    /// 0 - если первый вложен во второй<br/>
    /// 1 - если второй вложен в первый<br/>
    /// -1 - в остальных случаях<br/>
    /// </returns>
    public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
    {
        return IsInner(r1, r2) ? 1 : IsInner(r2, r1) ? 0 : -1;
    }

    private static bool IsInner(Rectangle r1, Rectangle r2)
    {
        return r1.Left <= r2.Left && r2.Right <= r1.Right &&
               r1.Top <= r2.Top && r2.Bottom <= r1.Bottom;
    }
}