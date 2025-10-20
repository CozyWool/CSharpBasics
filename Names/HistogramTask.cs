using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    public static HistogramData GetHistogramBirthsByYearAndGender(NameData[] names, Gender gender)
    {
        var minYear = int.MaxValue;
        var maxYear = int.MinValue;
        foreach (var name in names)
        {
            minYear = Math.Min(minYear, name.BirthDate.Year);
            maxYear = Math.Max(maxYear, name.BirthDate.Year);
        }
        
        var genderNames = GetGenderNames(gender);
        var years = GenerateLabels(maxYear - minYear + 1, minYear);
        var birthsCounts = new double[maxYear - minYear + 1];
        foreach (var nameData in names)
        {
            if (genderNames.Contains(nameData.Name))
            {
                birthsCounts[nameData.BirthDate.Year - minYear]++;
            }
        }

        return new HistogramData($"Рождаемость {(gender == Gender.Male ? "мужчин" : "женщин")} по годам",
                                 years,
                                 birthsCounts);
    }

    private static string[] GetGenderNames(Gender gender)
    {
        var lines = File.ReadAllLines(gender == Gender.Male ? "male_names_rus.txt" : "female_names_rus.txt");
        var genderNames = new string[lines.Length];
        for (var i = 0; i < lines.Length; i++)
        {
            genderNames[i] = lines[i].ToLower();
        }

        return genderNames;
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