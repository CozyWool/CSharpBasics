using System;
using System.Collections.Generic;
using System.Linq;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
    {
        var result = new List<ComparisonResult>();
        for (var i = 0; i < documents.Count; i++)
        {
            for (var j = i + 1; j < documents.Count; j++)
            {
                result.Add(new ComparisonResult(documents[i],
                                                documents[j],
                                                LevenshteinDistance(documents[i], documents[j])));
            }
        }

        return result;
    }

    public static double LevenshteinDistance(DocumentTokens first, DocumentTokens second)
    {
        var previousOpt = Enumerable.Range(0, second.Count + 1).Select(x => (double) x).ToArray();
        var currentOpt = new double[second.Count + 1];

        for (var i = 1; i <= first.Count; ++i)
        {
            currentOpt[0] = i;

            for (var j = 1; j <= second.Count; ++j)
            {
                currentOpt[j] =
                    FindMin(1 + currentOpt[j - 1],
                            1 + previousOpt[j],
                            previousOpt[j - 1] + TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]));
            }

            (previousOpt, currentOpt) = (currentOpt, previousOpt);
        }

        return previousOpt[second.Count];
    }

    private static T FindMin<T>(params T[] values) where T : IComparable<T> => values.Min();
}