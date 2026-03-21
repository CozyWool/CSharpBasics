using System;
using System.Linq;

namespace GaussAlgorithm;

public class Solver
{
    public double[] Solve(double[][] matrix, double[] freeMembers)
    {
        var n = matrix.Length;
        var m = matrix[0].Length;
        var a = matrix.Select(row => row.ToArray()).ToArray();
        var b = freeMembers.ToArray();


        var row = 0;
        for (var col = 0; col < m && row < n; col++)
        {
            var pivot = Enumerable.Range(row, n - row)
                                  .FirstOrDefault(i => !IsZero(a[i][col]), row);

            if (IsZero(a[pivot][col]))
            {
                continue;
            }

            SwapRows(a, b, pivot, row);
            DivideRow(a, b, a[row][col], row);
            EliminateRow(n, row, a, col, b);

            row++;
        }


        var isAnyZeroRow = a
                           .Select((r, j) => r.All(x => IsZero(x)) && !IsZero(b[j]))
                           .Any(x => x);
        if (isAnyZeroRow)
        {
            throw new NoSolutionException(matrix, freeMembers, a);
        }

        return CalculateAnswer(n, m, a, b);
    }

    private static double[] CalculateAnswer(int n, int m, double[][] a, double[] b)
    {
        var answer = new double[m];

        for (var i = 0; i < n; i++)
        {
            var pivotCol = Array.FindIndex(a[i], x => !IsZero(x));

            if (pivotCol == -1)
            {
                continue;
            }

            answer[pivotCol] = b[i];
        }

        return answer;
    }

    private static void EliminateRow(int n, int row, double[][] a, int col, double[] b)
    {
        for (var i = 0; i < n; i++)
        {
            if (i == row || IsZero(a[i][col]))
            {
                continue;
            }

            AddRows(a, b, -a[i][col], row, i);
        }
    }

    private static void AddRows(double[][] matrix, double[] freeMembers, double coefficient, int from, int to)
    {
        for (var j = 0; j < matrix[from].Length; j++)
        {
            matrix[to][j] += matrix[from][j] * coefficient;
        }

        freeMembers[to] += freeMembers[from] * coefficient;
    }

    private static void SwapRows(double[][] matrix, double[] freeMembers, int i, int j)
    {
        (matrix[i], matrix[j]) = (matrix[j], matrix[i]);
        (freeMembers[i], freeMembers[j]) = (freeMembers[j], freeMembers[i]);
    }

    private static void DivideRow(double[][] matrix, double[] freeMembers, double coefficient, int rowIndex)
    {
        for (var j = 0; j < matrix[rowIndex].Length; j++)
        {
            matrix[rowIndex][j] /= coefficient;
        }

        freeMembers[rowIndex] /= coefficient;
    }

    private static bool IsZero(double x, double eps = 1e-9) => Math.Abs(x) < eps;
}