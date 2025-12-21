using System.Collections.Generic;

namespace StructBenchmarking;

public class Experiments
{
    public static ChartData BuildChartDataForArrayCreation(
        IBenchmark benchmark, int repetitionsCount)
    {
        var classesTimes = new List<ExperimentResult>();
        var structuresTimes = new List<ExperimentResult>();

        foreach (var fieldCount in Constants.FieldCounts)
        {
            var classArrayCreationTask = new ClassArrayCreationTask(fieldCount);
            var structArrayCreationTask = new StructArrayCreationTask(fieldCount);

            var averageClassesTime =
                benchmark.MeasureDurationInMs(classArrayCreationTask, repetitionsCount) / repetitionsCount;
            var averageStructuresTime =
                benchmark.MeasureDurationInMs(structArrayCreationTask, repetitionsCount) / repetitionsCount;

            classesTimes.Add(new ExperimentResult(fieldCount, averageClassesTime));
            structuresTimes.Add(new ExperimentResult(fieldCount, averageStructuresTime));
        }

        return new ChartData
               {
                   Title = "Create array",
                   ClassPoints = classesTimes,
                   StructPoints = structuresTimes,
               };
    }

    public static ChartData BuildChartDataForMethodCall(
        IBenchmark benchmark, int repetitionsCount)
    {
        var classesTimes = new List<ExperimentResult>();
        var structuresTimes = new List<ExperimentResult>();

        //...

        return new ChartData
               {
                   Title = "Call method with argument",
                   ClassPoints = classesTimes,
                   StructPoints = structuresTimes,
               };
    }
}