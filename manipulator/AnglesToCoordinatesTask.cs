using System;
using Avalonia;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class AnglesToCoordinatesTask
{
    /// <summary>
    /// По значению углов суставов возвращает массив координат суставов
    /// в порядке new []{elbow, wrist, palmEnd}
    /// </summary>
    public static Point[] GetJointPositions(double shoulder, double elbow, double wrist)
    {
        var elbowAngle = shoulder - Math.PI + elbow;
        var wristAngle = shoulder + elbow + wrist;

        var elbowPos = new Point(UpperArm * Math.Cos(shoulder),
                                 UpperArm * Math.Sin(shoulder));
        var wristPos = new Point(elbowPos.X + Forearm * Math.Cos(elbowAngle),
                                 elbowPos.Y + Forearm * Math.Sin(elbowAngle));
        var palmEndPos = new Point(wristPos.X + Palm * Math.Cos(wristAngle),
                                   wristPos.Y + Palm * Math.Sin(wristAngle));
        return
        [
            elbowPos,
            wristPos,
            palmEndPos
        ];
    }
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests
{
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, 180.0, 150.0)]
    [TestCase(0, Math.PI, Math.PI, 330, 0)]
    [TestCase(Math.PI / 2, Math.PI, Math.PI, 0, 330)]
    [TestCase(0, Math.PI / 2, Math.PI, 150, -180)]
    [TestCase(0, Math.PI, 0, 210, 0)]
    public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);

        CheckDistance(joints[0], new Point(0, 0), UpperArm, "Плечо-Локоть");
        CheckDistance(joints[1], joints[0], Forearm, "Локоть-Запястье");
        CheckDistance(joints[2], joints[1], Palm, "Запястье-Конец ладони");

        ClassicAssert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
        ClassicAssert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
    }

    [TestCase(0, 0, 0)]            // Прямая рука вправо
    [TestCase(Math.PI, 0, 0)]      // Прямая рука влево
    [TestCase(Math.PI / 2, 0, 0)]  // Прямая рука вверх
    [TestCase(-Math.PI / 2, 0, 0)] // Прямая рука вниз
    [TestCase(Math.PI / 4, 0, 0)]  // Прямая рука под 45 градусов
    public void TestFullyExtendedArm(double shoulder, double elbow, double wrist)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);

        // Проверяем, что все сегменты выстроены в линию
        CheckCollinear(joints, "Все суставы должны быть на одной линии при вытянутой руке");

        CheckDistance(joints[0], new Point(0, 0), UpperArm, "Плечо-Локоть");
        CheckDistance(joints[1], joints[0], Forearm, "Локоть-Запястье");
        CheckDistance(joints[2], joints[1], Palm, "Запястье-Конец ладони");
    }

    [TestCase(0, -Math.PI / 2, 0)]           // Локоть согнут на 90 градусов вперед
    [TestCase(0, Math.PI / 2, 0)]            // Локоть согнут на 90 градусов назад
    [TestCase(Math.PI / 2, -Math.PI / 2, 0)] // Плечо вверх, локоть согнут вперед
    [TestCase(Math.PI / 2, Math.PI / 2, 0)]  // Плечо вверх, локоть согнут назад
    public void TestRightAngleElbow(double shoulder, double elbow, double wrist)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);

        // Проверяем правильность треугольника плечо-локоть-запястье
        var shoulderToElbow = Distance(joints[0], new Point(0, 0));
        var elbowToWrist = Distance(joints[1], joints[0]);
        var shoulderToWrist = Distance(joints[1], new Point(0, 0));

        // Теорема Пифагора для проверки прямого угла
        var expectedShoulderToWrist = Math.Sqrt(UpperArm * UpperArm + Forearm * Forearm);
        ClassicAssert.AreEqual(expectedShoulderToWrist, shoulderToWrist, 1e-5,
                               "Расстояние от плеча до запястья должно соответствовать теореме Пифагора");

        CheckDistance(joints[0], new Point(0, 0), UpperArm, "Плечо-Локоть");
        CheckDistance(joints[1], joints[0], Forearm, "Локоть-Запястье");
    }

    [TestCase(0, 0, Math.PI / 2)]           // Кисть согнута на 90 градусов
    [TestCase(0, 0, -Math.PI / 2)]          // Кисть согнута на -90 градусов
    [TestCase(Math.PI / 2, 0, Math.PI / 2)] // Вся рука вверх с согнутой кистью
    public void TestWristBend(double shoulder, double elbow, double wrist)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);

        CheckDistance(joints[0], new Point(0, 0), UpperArm, "Плечо-Локоть");
        CheckDistance(joints[1], joints[0], Forearm, "Локоть-Запястье");
        CheckDistance(joints[2], joints[1], Palm, "Запястье-Конец ладони");
    }

    [Test]
    public void TestMultipleRevolutions()
    {
        // Тест с углами больше 2*PI (несколько полных оборотов)
        var joints1 = AnglesToCoordinatesTask.GetJointPositions(0, 0, 0);
        var joints2 = AnglesToCoordinatesTask.GetJointPositions(2 * Math.PI, 2 * Math.PI, 2 * Math.PI);

        // Должны получить одинаковые позиции
        for (var i = 0; i < 3; i++)
        {
            ClassicAssert.AreEqual(joints1[i].X, joints2[i].X, 1e-5, $"Координата X сустава {i} должна совпадать");
            ClassicAssert.AreEqual(joints1[i].Y, joints2[i].Y, 1e-5, $"Координата Y сустава {i} должна совпадать");
        }
    }

    [TestCase(-Math.PI)]         // Минимальный угол
    [TestCase(Math.PI)]          // Максимальный угол
    [TestCase(-3 * Math.PI / 4)] // Граничное значение
    [TestCase(3 * Math.PI / 4)]  // Граничное значение
    public void TestExtremeShoulderAngles(double shoulder)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, 0, 0);

        CheckDistance(joints[0], new Point(0, 0), UpperArm, "Плечо-Локоть");
        CheckDistance(joints[1], joints[0], Forearm, "Локоть-Запястье");
        CheckDistance(joints[2], joints[1], Palm, "Запястье-Конец ладони");
    }

    [Test]
    public void TestAllJointsBentForward()
    {
        // Все суставы согнуты вперед
        var joints = AnglesToCoordinatesTask.GetJointPositions(-Math.PI / 4, -Math.PI / 4, -Math.PI / 4);

        CheckDistance(joints[0], new Point(0, 0), UpperArm, "Плечо-Локоть");
        CheckDistance(joints[1], joints[0], Forearm, "Локоть-Запястье");
        CheckDistance(joints[2], joints[1], Palm, "Запястье-Конец ладони");
    }

    [Test]
    public void TestAllJointsBentBackward()
    {
        // Все суставы согнуты назад
        var joints = AnglesToCoordinatesTask.GetJointPositions(Math.PI / 4, Math.PI / 4, Math.PI / 4);

        CheckDistance(joints[0], new Point(0, 0), UpperArm, "Плечо-Локоть");
        CheckDistance(joints[1], joints[0], Forearm, "Локоть-Запястье");
        CheckDistance(joints[2], joints[1], Palm, "Запястье-Конец ладони");
    }

    [TestCase(0.1, 0.2, 0.3)] // Малые углы
    [TestCase(1.0, 1.5, 2.0)] // Средние углы
    [TestCase(2.5, 2.8, 3.0)] // Большие углы
    public void TestRandomCombinations(double shoulder, double elbow, double wrist)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);

        CheckDistance(joints[0], new Point(0, 0), UpperArm, "Плечо-Локоть");
        CheckDistance(joints[1], joints[0], Forearm, "Локоть-Запястье");
        CheckDistance(joints[2], joints[1], Palm, "Запястье-Конец ладони");
    }

    private void CheckDistance(Point point1, Point point2, double expectedDistance, string segmentName)
    {
        var dx = point1.X - point2.X;
        var dy = point1.Y - point2.Y;
        var actualDistance = Math.Sqrt(dx * dx + dy * dy);
        ClassicAssert.AreEqual(expectedDistance, actualDistance, 1e-5,
                               $"Расстояние {segmentName} неверно." +
                               $" Ожидалось: {expectedDistance}, Получено: {actualDistance}");
    }

    private double Distance(Point p1, Point p2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private void CheckCollinear(Point[] joints, string message)
    {
        // Проверяем, что точки лежат на одной прямой
        // Используем площадь треугольника (должна быть близка к 0 для коллинеарных точек)
        for (var i = 0; i < joints.Length - 2; i++)
        {
            var area = Math.Abs(
                                (joints[i].X * (joints[i + 1].Y - joints[i + 2].Y) +
                                 joints[i + 1].X * (joints[i + 2].Y - joints[i].Y) +
                                 joints[i + 2].X * (joints[i].Y - joints[i + 1].Y)) / 2.0
                               );
            ClassicAssert.Less(area, 1e-5, message);
        }
    }
}