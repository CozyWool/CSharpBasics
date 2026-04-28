using System.Numerics;

namespace Tickets;

internal class TicketsTask
{
    public static BigInteger Solve(int n, int totalSum)
    {
        if (totalSum % 2 != 0)
        {
            return 0;
        }

        var targetSum = totalSum / 2;

        var dp = new BigInteger[n + 1, targetSum + 1];
        dp[0, 0] = 1;

        for (var digitCount = 1; digitCount <= n; digitCount++)
        {
            FillDpWithSums(dp, targetSum, digitCount);
        }

        return dp[n, targetSum] * dp[n, targetSum];
    }

    private static void FillDpWithSums(BigInteger[,] dp, int targetSum, int digitCount)
    {
        for (var currentSum = 0; currentSum <= targetSum; currentSum++)
        {
            dp[digitCount, currentSum] = 0;
            for (var digit = 0; digit <= 9; digit++)
            {
                if (currentSum - digit >= 0)
                {
                    dp[digitCount, currentSum] += dp[digitCount - 1, currentSum - digit];
                }
            }
        }
    }
}