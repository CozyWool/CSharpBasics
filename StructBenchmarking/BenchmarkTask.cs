using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking;

public class Benchmark : IBenchmark
{
    public double MeasureDurationInMs(ITask task, int repetitionCount)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();

        var stopwatch = new Stopwatch();
        task.Run();
        stopwatch.Start();
        for (var i = 0; i < repetitionCount; i++)
        {
            task.Run();
        }

        stopwatch.Stop();
        return stopwatch.Elapsed.TotalMilliseconds / repetitionCount;
    }
}

public class StringBuilderTask : ITask
{
    public void Run()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < 10000; ++i)
        {
            sb.Append('a');
        }

        var s = sb.ToString();
    }
}

public class StringConstructorTask : ITask
{
    public void Run()
    {
        var s = new string('a', 10000);
    }
}

[TestFixture]
public class RealBenchmarkUsageSample
{
    [Test]
    public void StringConstructorFasterThanStringBuilder()
    {
        var stringBuilderTask = new StringBuilderTask();
        var stringConstructorTask = new StringConstructorTask();
        var benchmark = new Benchmark();
        var measureStringBuilder = benchmark.MeasureDurationInMs(stringBuilderTask, 7500);
        var measureStringConstructor = benchmark.MeasureDurationInMs(stringConstructorTask, 7500);
        Assert.That(measureStringConstructor, Is.LessThan(measureStringBuilder));
    }
}