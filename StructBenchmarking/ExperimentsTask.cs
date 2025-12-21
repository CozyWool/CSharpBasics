using System.Collections.Generic;

namespace StructBenchmarking;

public class Experiments
{
    public static ChartData BuildChartDataForArrayCreation(
        IBenchmark benchmark, int repetitionsCount)
    {
        var factory = new ArrayCreationTaskFactory();
        var (classesTimes, structuresTimes) = RunExperiment(factory, benchmark, repetitionsCount);

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
        var factory = new MethodCallTaskFactory();
        var (classesTimes, structuresTimes) = RunExperiment(factory, benchmark, repetitionsCount);

        return new ChartData
               {
                   Title = "Call method with argument",
                   ClassPoints = classesTimes,
                   StructPoints = structuresTimes,
               };
    }

    private static (List<ExperimentResult>, List<ExperimentResult>) RunExperiment(
        ITaskFactory taskFactory, IBenchmark benchmark, int repetitionsCount)
    {
        var classesTimes = new List<ExperimentResult>();
        var structuresTimes = new List<ExperimentResult>();

        foreach (var size in Constants.FieldCounts)
        {
            var classTask = taskFactory.GetClassTask(size);
            var structTask = taskFactory.GetStructTask(size);

            var averageClassesTime =
                benchmark.MeasureDurationInMs(classTask, repetitionsCount) / repetitionsCount;
            var averageStructuresTime =
                benchmark.MeasureDurationInMs(structTask, repetitionsCount) / repetitionsCount;

            classesTimes.Add(new ExperimentResult(size, averageClassesTime));
            structuresTimes.Add(new ExperimentResult(size, averageStructuresTime));
        }

        return (classesTimes, structuresTimes);
    }
}

public interface ITaskFactory
{
    ITask GetClassTask(int size);
    ITask GetStructTask(int size);
}

public class ArrayCreationTaskFactory : ITaskFactory
{
    public ITask GetClassTask(int size)
    {
        return new ClassArrayCreationTask(size);
    }

    public ITask GetStructTask(int size)
    {
        return new StructArrayCreationTask(size);
    }
}

public class MethodCallTaskFactory : ITaskFactory
{
    public ITask GetClassTask(int size)
    {
        return new MethodCallWithClassArgumentTask(size);
    }

    public ITask GetStructTask(int size)
    {
        return new MethodCallWithStructArgumentTask(size);
    }
}