using System;
using NUnit.Framework;

namespace Manipulation;

public class TriangleTask
{
    /// <summary>
    /// Возвращает угол (в радианах) между сторонами a и b в треугольнике со сторонами a, b, c 
    /// </summary>
    public static double GetABAngle(double a, double b, double c)
    {
        var mx = Math.Max(Math.Max(a, b), c);
        if (2 * mx > a + b + c || a <= 0 || b <= 0 || c < 0)
        {
            return double.NaN;
        }
        var cosA = (a * a + b * b - c * c) / (2 * a * b);
        if (cosA is < -1.0 or > 1.0)
        {
            return double.NaN;
        }
        return Math.Acos(cosA);
    }
}

[TestFixture]
public class TriangleTask_Tests
{
    [TestCase(3, 4, 5, Math.PI / 2)]
    [TestCase(1, 1, 1, Math.PI / 3)]
    [TestCase(150, 120, 60, 0.3897607327974747)]
    [TestCase(60, 120, 150, 1.8886200307227774)]
    [TestCase(1, 1, 2, Math.PI)]
    [TestCase(2, 1, 1, 0)]
    [TestCase(1, 2, 1, 0)]
    [TestCase(1, 1, 2.001, double.NaN)]
    [TestCase(1, 2.001, 1, double.NaN)]
    [TestCase(2.001, 1, 1, double.NaN)]
    [TestCase(0, 5, 5, double.NaN)]
    [TestCase(5, 0, 5, double.NaN)]
    [TestCase(5, 5, 0, 0)]
    [TestCase(1, 1, 0, 0)]
    [TestCase(-3, -2, -4, double.NaN)]
    public void TestGetABAngle(double a, double b, double c, double expectedAngle)
    {
        Assert.That(TriangleTask.GetABAngle(a, b, c), Is.EqualTo(expectedAngle).Within(1e-5));
    }
}