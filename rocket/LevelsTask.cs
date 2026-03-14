using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
    static readonly Physics standardPhysics = new();

    public static IEnumerable<Level> CreateLevels()
    {
        var rocket = new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
        var target = new Vector(700, 500);

        var anomaly = (rocket.Location + target) / 2;

        yield return CreateLevel("Zero", rocket, target, (_, _) => Vector.Zero);

        yield return CreateLevel("Heavy", rocket, target, (_, _) => new Vector(0, 0.9));
        yield return CreateLevel("Up", rocket, target, (size, v) => GetUpGravity(v, size));
        yield return CreateLevel("WhiteHole", rocket, target, (_, v) => GetWhiteHole(target, v));
        yield return CreateLevel("BlackHole", rocket, target, (_, v) => GetBlackHole(anomaly, v));
        yield return CreateLevel("BlackAndWhite", rocket, target,
                                 (_, v) => (GetBlackHole(anomaly, v) + GetWhiteHole(target, v)) / 2);
    }

    private static Level CreateLevel(string name, Rocket rocket, Vector target, Gravity gravity) =>
        new(name, rocket, target, gravity, standardPhysics);

    private static Vector GetUpGravity(Vector v, Vector size)
    {
        var lowerBorder = new Vector(v.X, size.Y);
        var d = (lowerBorder - v).Length;
        var module = -(300 / (d + 300.0));
        return new Vector(0, module);
    }


    private static Vector GetBlackHole(Vector anomaly, Vector v)
    {
        var direction = anomaly - v;
        var d = direction.Length;
        var module = 300 * d / (d * d + 1);
        return direction.Normalize() * module;
    }

    private static Vector GetWhiteHole(Vector target, Vector v)
    {
        var direction = v - target;
        var d = direction.Length;
        var module = 140 * d / (d * d + 1);
        return direction.Normalize() * module;
    }
}