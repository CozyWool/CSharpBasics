using System;

namespace Fractals;

internal static class DragonFractalTask
{
    public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
    {
        var x = 1.0;
        var y = 0.0;
        pixels.SetPixel(x, y);

        var random = new Random(seed);
        for (var i = 0; i < iterationsCount; i++)
        {
            var choice = random.Next(2);
            var (x1, y1) = CalculatePixel(choice == 1 ? 135 : 45, x, y, choice == 1 ? 1 : 0);

            pixels.SetPixel(x1, y1);
            x = x1;
            y = y1;
        }
    }

    private static (double, double) CalculatePixel(double angle,
                                                   double x,
                                                   double y,
                                                   double offsetX = 0,
                                                   double offsetY = 0)
    {
        angle = angle * Math.PI / 180;
        var x1 = (x * Math.Cos(angle) - y * Math.Sin(angle)) / Math.Sqrt(2) + offsetX;
        var y1 = (x * Math.Sin(angle) + y * Math.Cos(angle)) / Math.Sqrt(2) + offsetY;
        return (x1, y1);
    }
}