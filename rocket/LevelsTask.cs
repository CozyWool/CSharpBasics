using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
    private static readonly Physics _standardPhysics = new();
    private static readonly Rocket _rocket = new(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI);
    private static readonly Vector _target = new(700, 500);
    private const int WhiteHoleCoefficient = 140;
    private const int BlackHoleCoefficient = 300;

    public static IEnumerable<Level> CreateLevels()
    {
        var anomaly = (_rocket.Location + _target) / 2;

        var whiteHoleGravity = GetHoleGravity(WhiteHoleCoefficient, false);
        var blackHoleGravity = GetHoleGravity(BlackHoleCoefficient, true, anomaly);

        yield return CreateLevel("Zero", (_, _) => Vector.Zero);

        yield return CreateLevel("Heavy", (_, _) => new Vector(0, 0.9));
        yield return CreateLevel("Up", GetUpGravity());
        yield return CreateLevel("WhiteHole", whiteHoleGravity);
        yield return CreateLevel("BlackHole", blackHoleGravity);
        yield return CreateLevel("BlackAndWhite",
                                 (size, v) => (whiteHoleGravity(size, v) + blackHoleGravity(size, v)) / 2);
    }

    private static Level CreateLevel(string name, Gravity gravity, Rocket? rocket = null, Vector? target = null) =>
        new(name, rocket ?? _rocket, target ?? _target, gravity, _standardPhysics);

    private static Gravity GetUpGravity()
    {
        return (size, v) =>
               {
                   var lowerBorder = new Vector(v.X, size.Y);
                   var d = (lowerBorder - v).Length;
                   var module = -(300 / (d + 300.0));
                   return new Vector(0, module);
               };
    }

    private static Gravity GetHoleGravity(int coefficient, bool isAttractive, Vector? target = null)
    {
        return (_, v) =>
               {
                   var direction = v - (target ?? _target);
                   if (isAttractive)
                   {
                       direction *= -1;
                   }

                   var d = direction.Length;
                   var module = coefficient * d / (d * d + 1);
                   return direction.Normalize() * module;
               };
    }
}