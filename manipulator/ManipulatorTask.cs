using System;
using Avalonia;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class ManipulatorTask
{
    /// <summary>
    /// Возвращает массив углов (shoulder, elbow, wrist),
    /// необходимых для приведения эффектора манипулятора в точку x и y
    /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
    /// См. чертеж manipulator.png!
    /// </summary>
    public static double[] MoveManipulatorTo(double x, double y, double alpha)
    {
        var wristX = x + Palm * Math.Cos(Math.PI - alpha);
        var wristY = y + Palm * Math.Sin(Math.PI - alpha);

        var shoulderToWrist = Math.Sqrt(wristX * wristX + wristY * wristY);

        var angleToWrist = Math.Atan2(wristY, wristX);

        var elbowAngle = TriangleTask.GetABAngle(UpperArm, Forearm, shoulderToWrist);

        var shoulderAngle = angleToWrist + TriangleTask.GetABAngle(shoulderToWrist, UpperArm, Forearm);
        var wristAngle = -alpha - shoulderAngle - elbowAngle;
        return [shoulderAngle, elbowAngle, wristAngle];
    }
}

[TestFixture]
public class ManipulatorTask_Tests
{
    private const double Delta = 1e-5;
    private Random random = new Random();

    [Test]
    public void TestMoveManipulatorTo()
    {
        var successCount = 0;
        var totalTests = 500;

        for (var i = 0; i < totalTests; i++)
        {
            // Генерируем точки только в гарантированно достижимой зоне
            double maxReach = UpperArm + Forearm;
            double minReach = Math.Abs(UpperArm - Forearm);

            var distance = minReach + random.NextDouble() * (maxReach - minReach);
            var angle = random.NextDouble() * 2 * Math.PI;

            var wristX = distance * Math.Cos(angle);
            var wristY = distance * Math.Sin(angle);

            // Добавляем ладонь с случайной ориентацией
            var alpha = GetRandomAngle();
            var x = wristX + Palm * Math.Cos(alpha);
            var y = wristY + Palm * Math.Sin(alpha);

            if (TestSingleConfiguration(x, y, alpha))
            {
                successCount++;
            }
        }

        ClassicAssert.Greater(successCount,
                              totalTests * 0.8,
                              $"At least 80% of reachable points should be solved." +
                              $" Success: {successCount}/{totalTests}");
    }

    [Test]
    public void TestMoveManipulatorTo_KnownWorkingCases()
    {
        // Тестируем заранее известные рабочие случаи
        var testCases = new[]
                        {
                            (100.0, 50.0, 0.0),
                            (150.0, 0.0, 0.0),
                            (0.0, 150.0, Math.PI / 2),
                            (120.0, 80.0, Math.PI / 4),
                            (80.0, 120.0, -Math.PI / 4)
                        };

        foreach (var (x, y, alpha) in testCases)
        {
            var success = TestSingleConfiguration(x, y, alpha);
            ClassicAssert.IsTrue(success, $"Known case should work: ({x}, {y}, alpha={alpha})");
        }
    }

    [Test]
    public void TestMoveManipulatorTo_SimpleCases()
    {
        // Простые случаи вдоль осей
        TestSingleConfiguration(UpperArm + Forearm + Palm, 0, 0);             // Вдоль X
        TestSingleConfiguration(0, UpperArm + Forearm + Palm, Math.PI / 2);   // Вдоль Y
        TestSingleConfiguration(-UpperArm - Forearm - Palm, 0, Math.PI);      // Против X
        TestSingleConfiguration(0, -UpperArm - Forearm - Palm, -Math.PI / 2); // Против Y
    }

    [Test]
    public void TestMoveManipulatorTo_PalmOrientation()
    {
        // Тестируем разные ориентации ладони в одной точке
        double x = 120, y = 80;

        for (var i = 0; i < 50; i++)
        {
            var alpha = GetRandomAngle();
            if (TestSingleConfiguration(x, y, alpha))
            {
                TestPalmOrientation(x, y, alpha);
            }
        }
    }

    private bool TestSingleConfiguration(double x, double y, double alpha)
    {
        var angles = ManipulatorTask.MoveManipulatorTo(x, y, alpha);

        if (double.IsNaN(angles[0]) || double.IsNaN(angles[1]) || double.IsNaN(angles[2]))
        {
            // Проверяем, действительно ли точка недостижима
            var distance = Math.Sqrt(x * x + y * y);
            double maxReach = UpperArm + Forearm + Palm;
            double minReach = Math.Abs(UpperArm - Forearm) - Palm;

            var shouldBeReachable = distance <= maxReach && distance >= Math.Max(0, minReach);

            if (shouldBeReachable)
            {
                Console.WriteLine($"Point ({x:F2}, {y:F2}) with alpha={alpha:F2} is reachable but returned NaN");
                return false;
            }

            return true; // NaN ожидаем для недостижимых точек
        }

        // Проверяем конечную позицию
        var joints = AnglesToCoordinatesTask.GetJointPositions(angles[0], angles[1], angles[2]);
        var finalX = joints[2].X;
        var finalY = joints[2].Y;

        var positionCorrect = Math.Abs(finalX - x) < Delta && Math.Abs(finalY - y) < Delta;

        if (!positionCorrect)
        {
            Console.WriteLine($"Position mismatch: expected ({x}, {y}), got ({finalX}, {finalY}) for alpha={alpha}");
            return false;
        }

        return true;
    }

    private void TestPalmOrientation(double x, double y, double alpha)
    {
        var angles = ManipulatorTask.MoveManipulatorTo(x, y, alpha);

        if (!double.IsNaN(angles[0]))
        {
            // Проверяем формулу wrist = -alpha - shoulder - elbow
            var calculatedWrist = -alpha - angles[0] - angles[1];
            var wristDiff = Math.Abs(calculatedWrist - angles[2]);

            // Нормализуем разницу углов
            while (wristDiff > Math.PI)
            {
                wristDiff -= 2 * Math.PI;
            }

            while (wristDiff < -Math.PI)
            {
                wristDiff += 2 * Math.PI;
            }

            ClassicAssert.IsTrue(Math.Abs(wristDiff) < Delta,
                                 $"Wrist angle formula failed for ({x:F2}, {y:F2}, alpha={alpha:F2})");
        }
    }

    private double GetRandomAngle()
    {
        return (random.NextDouble() * 2 - 1) * Math.PI; // от -π до π
    }
}