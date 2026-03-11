using System.Collections.Concurrent;
using System.Text;
using iText.Kernel.Pdf;

namespace ConsoleApp2;

public static class PasswordDecoder
{
    public static void FindDatabaseLabPassword(string filePath, int start = 100000, int end = 999999)
    {
        long total = end - start + 1;

        long checkedCount = 0;
        var found = false;
        string? foundPassword = null;

        var sw = System.Diagnostics.Stopwatch.StartNew();

        Console.WriteLine($"Range: {start}-{end} ({total} passwords)");
        Console.WriteLine($"Threads: {Environment.ProcessorCount}");
        Console.WriteLine("Starting...\n");

        var progressThread = new Thread(() =>
                                        {
                                            while (!found)
                                            {
                                                Thread.Sleep(1000);

                                                var done = Interlocked.Read(ref checkedCount);
                                                var speed = done / sw.Elapsed.TotalSeconds;
                                                var percent = (double) done / total * 100;

                                                var remaining = (total - done) / Math.Max(speed, 1);
                                                var eta = TimeSpan.FromSeconds(remaining);

                                                Console.WriteLine(
                                                                  $"Checked: {done:N0}/{total:N0} | " +
                                                                  $"{percent:F2}% | " +
                                                                  $"{speed:N0} pwd/s | ETA {eta:hh\\:mm\\:ss}");
                                            }
                                        });

        progressThread.Start();

        var rangePartitioner = Partitioner.Create(start, end + 1);

        Parallel.ForEach(rangePartitioner,
                         new ParallelOptions
                         {
                             MaxDegreeOfParallelism = Environment.ProcessorCount
                         },
                         (range, state) =>
                         {
                             for (var i = range.Item1; i < range.Item2; i++)
                             {
                                 if (found)
                                 {
                                     state.Stop();
                                     return;
                                 }

                                 var password = i.ToString();

                                 if (CheckPassword(filePath, password))
                                 {
                                     found = true;
                                     foundPassword = password;
                                     state.Stop();
                                     return;
                                 }

                                 Interlocked.Increment(ref checkedCount);
                             }
                         });

        sw.Stop();

        Console.WriteLine();

        if (found)
        {
            Console.WriteLine($"PASSWORD FOUND: {foundPassword}");
            Console.WriteLine($"Time: {sw.Elapsed}");
        }
        else
        {
            Console.WriteLine("Password not found");
        }
    }

    private static bool CheckPassword(string path, string password)
    {
        try
        {
            var props = new ReaderProperties()
                .SetPassword(Encoding.ASCII.GetBytes(password));

            using var reader = new PdfReader(path, props);
            using var pdf = new PdfDocument(reader);

            return true;
        }
        catch
        {
            return false;
        }
    }
}