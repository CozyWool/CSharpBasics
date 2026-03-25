using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        return visits
               .GroupBy(visit => visit.UserId)
               .SelectMany(group => group
                                    .OrderBy(visit => visit.DateTime)
                                    .Bigrams()
                                    .Where(bigram => bigram.First.SlideType == slideType)
                                    .Select(bigram => (bigram.Second.DateTime - bigram.First.DateTime).TotalMinutes)
                                    .Where(delta => TimeSpan.FromMinutes(1).TotalMinutes <= delta &&
                                                    delta <= TimeSpan.FromHours(2).TotalMinutes))
               .DefaultIfEmpty(0.0)
               .Median();
    }
}