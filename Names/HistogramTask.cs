namespace Names;

internal static class HistogramTask
{
    public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
    {
        var days = GenerateLabels(31, 1);
        var birthsCounts = new double[31];
        foreach (var n in names)
        {
            if (Equals(n.Name, name))
            {
                birthsCounts[n.BirthDate.Day - 1]++;
            }
        }

        birthsCounts[0] = 0;

        return new HistogramData($"Рождаемость людей с именем '{name}'",
                                 days,
                                 birthsCounts);
    }

    public static string[] GenerateLabels(int size, int offsetFromIndex)
    {
        var labels = new string[size];
        for (var i = 0; i < size; i++)
        {
            labels[i] = (i + offsetFromIndex).ToString();
        }

        return labels;
    }
}