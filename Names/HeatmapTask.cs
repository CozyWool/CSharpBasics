namespace Names;

internal static class HeatmapTask
{
    public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
    {
        var days = GenerateLabels(30, 2);
        var months = GenerateLabels(12, 1);

        var birthsCounts = new double[30, 12];
        foreach (var n in names)
        {
            if (n.BirthDate.Day != 1)
            {
                birthsCounts[n.BirthDate.Day - 2, n.BirthDate.Month - 1]++;
            }
        }

        return new HeatmapData(
                               "Рождаемость в течение года",
                               birthsCounts,
                               days,
                               months);
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