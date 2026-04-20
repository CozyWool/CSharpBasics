using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    public Rocket GetNextMove(Rocket rocket)
    {
        var bestResult = GetBestMoveAsync(rocket).Result;

        return rocket.Move(bestResult.Turn, level);
    }

    private async Task<(Turn Turn, double Score)> GetBestMoveAsync(Rocket rocket)
    {
        var tasks = CreateTasks(rocket);
        var results = await Task.WhenAll(tasks);

        return results.MaxBy(x => x.Score);
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        var tasks = new List<Task<(Turn Turn, double Score)>>();

        for (var i = 0; i < threadsCount; i++)
        {
            var seed = random.Next();
            tasks.Add(Task.Run(() => SearchBestMove(rocket, new Random(seed), iterationsCount / threadsCount)));
        }

        return tasks;
    }
}