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
    private readonly int _symbolCount;

    public StringBuilderTask(int symbolCount)
    {
        _symbolCount = symbolCount;
    }

    public void Run()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < _symbolCount; ++i)
        {
            sb.Append('a');
        }

        var s = sb.ToString();
    }
}

public class StringConstructorTask : ITask
{
    private readonly int _symbolCount;

    public StringConstructorTask(int symbolCount)
    {
        _symbolCount = symbolCount;
    }

    public void Run()
    {
        var s = new string('a', _symbolCount);
    }
}

[TestFixture]
public class RealBenchmarkUsageSample
{
	[TestCase(10000, 7500)]
    public void StringConstructorFasterThanStringBuilder(int symbolCount, int repetitionsCount)
    {
        var stringBuilderTask = new StringBuilderTask(symbolCount);
        var stringConstructorTask = new StringConstructorTask(symbolCount);
        var benchmark = new Benchmark();

        var measureStringBuilder = benchmark.MeasureDurationInMs(stringBuilderTask, repetitionsCount);
        var measureStringConstructor = benchmark.MeasureDurationInMs(stringConstructorTask, repetitionsCount);
        Assert.That(measureStringConstructor, Is.LessThan(measureStringBuilder));
    }
}