using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews;

public class ParsingTask
{
    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
    /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
    /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
    public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
    {
        return lines
               .Skip(1)
               .Select(TryParseSlideRecord)
               .Where(x => x is not null)
               .ToDictionary(x => x.SlideId, x => x);
    }

    private static SlideRecord? TryParseSlideRecord(string x)
    {
        var parts = x.Split(';', StringSplitOptions.TrimEntries);
        if (parts.Length != 3 ||
            !int.TryParse(parts[0], out var slideId) ||
            !Enum.TryParse<SlideType>(parts[1], true, out var slideType))
        {
            return null;
        }

        return new SlideRecord(slideId, slideType, parts[2]);
    }

    /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
    /// <param name="slides">Словарь информации о слайдах по идентификатору слайда.
    /// Такой словарь можно получить методом ParseSlideRecords</param>
    /// <returns>Список информации о посещениях</returns>
    /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
    public static IEnumerable<VisitRecord> ParseVisitRecords(
        IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
    {
        return lines
               .Skip(1)
               .Select(x => ParseVisitRecord(x, slides));
    }

    private static VisitRecord ParseVisitRecord(string x, IDictionary<int, SlideRecord> slides)
    {
        var parts = x.Split(';', StringSplitOptions.TrimEntries);
        if (parts.Length != 4 ||
            !int.TryParse(parts[0], out var userId) ||
            !int.TryParse(parts[1], out var slideId) ||
            !DateOnly.TryParse(parts[2], CultureInfo.InvariantCulture, out var date) ||
            !TimeOnly.TryParse(parts[3], CultureInfo.InvariantCulture, out var time) ||
            !slides.TryGetValue(slideId, out var slide))
        {
            throw new FormatException($"Wrong line [{x}]");
        }

        return new VisitRecord(userId, slideId, new DateTime(date, time), slide.SlideType);
    }
}