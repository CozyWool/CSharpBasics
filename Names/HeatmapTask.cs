namespace Names;

internal static class HeatmapTask
{
    public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
    {
        var birthsCounts = new double[30, 12];
        foreach (var nameData in names)
        {
            if (nameData.BirthDate.Day != 1)
            {
                birthsCounts[nameData.BirthDate.Day - 2, nameData.BirthDate.Month - 1]++;
            }
        }

        return new HeatmapData(
                               "Рождаемость в течение года",
                               birthsCounts,
                               GenerateLabels(30, 2),
                               GenerateLabels(12, 1));
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