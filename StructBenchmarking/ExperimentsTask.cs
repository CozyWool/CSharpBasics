using System.Collections.Generic;

namespace StructBenchmarking;

public class Experiments
{
    public static ChartData BuildChartDataForArrayCreation(
        IBenchmark benchmark, int repetitionsCount)
    {
        var factory = new ArrayCreationTaskFactory();

        return RunExperiment(factory, benchmark, repetitionsCount, "Create array");
    }

    public static ChartData BuildChartDataForMethodCall(
        IBenchmark benchmark, int repetitionsCount)
    {
        var factory = new MethodCallTaskFactory();

        return RunExperiment(factory, benchmark, repetitionsCount, "Call method with argument");
    }

    private static ChartData RunExperiment(
        ITaskFactory taskFactory, IBenchmark benchmark, int repetitionsCount, string title)
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

        return new ChartData
               {
                   Title = title,
                   ClassPoints = classesTimes,
                   StructPoints = structuresTimes,
               };
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