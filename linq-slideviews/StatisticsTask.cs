using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    private const double MinDeltaMinutes = 1;
    private const double MaxDeltaMinutes = 120;

    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        return visits
               .GroupBy(visit => visit.UserId)
               .SelectMany(group => group
                                .OrderBy(visit => visit.DateTime)
                                .Bigrams()
                                .Where(bigram => bigram.First.SlideType == slideType)
                                .Select(bigram => (bigram.Second.DateTime - bigram.First.DateTime).TotalMinutes)
                                .Where(delta => delta is >= MinDeltaMinutes and <= MaxDeltaMinutes))
               .DefaultIfEmpty(0.0)
               .Median();
    }
}