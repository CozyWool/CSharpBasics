using System;
using System.Collections.Generic;
using Avalonia.Media;
using RefactorMe.Common;

namespace RefactorMe
{
    class GraphicsDrawer
    {
        private static float _x, _y;
        private static IGraphics? _graphics;

        public static void Initialize(IGraphics newGraphics)
        {
            _graphics = newGraphics;
            _graphics.Clear(Colors.Black);
        }

        public static void SetPosition(float x0, float y0)
        {
            _x = x0;
            _y = y0;
        }

        public static void DrawLine(Pen pen, double length, double angle)
        {
            var x1 = (float) (_x + length * Math.Cos(angle));
            var y1 = (float) (_y + length * Math.Sin(angle));
            _graphics?.DrawLine(pen, _x, _y, x1, y1);
            _x = x1;
            _y = y1;
        }

        public static void ChangeDirection(double length, double angle)
        {
            _x = (float) (_x + length * Math.Cos(angle));
            _y = (float) (_y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        public static void Draw(int width, int height, double rotationAngle, IGraphics graphics, float innerLineMultiplier, float outerLineMultiplier)
        {
            GraphicsDrawer.Initialize(graphics);

            var size = Math.Min(width, height);

            var diagonalLength = Math.Sqrt(2) * (size * 0.375f + size * 0.04f) / 2;
            var x0 = (float) (diagonalLength * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
            var y0 = (float) (diagonalLength * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;

            var random = new Random();
            var squaresCount = random.Next(5, 20);
            List<Pen> colors =
            [
                new Pen(Brushes.Green),
                new Pen(Brushes.Red),
                new Pen(Brushes.Blue),
                new Pen(Brushes.White),
                new Pen(Brushes.BlueViolet),
                new Pen(Brushes.Crimson),
                new Pen(Brushes.SteelBlue),
            ];

            for (var i = 0; i < squaresCount; i++)
            {
                var sizeMultiplier = random.Next(1, 4) * size;
                DrawSquare(rotationAngle,
                           x0 + random.Next(-1000, 1000),
                           y0 + random.Next(-1000, 1000),
                           innerLineMultiplier * sizeMultiplier,
                           outerLineMultiplier * sizeMultiplier,
                           colors[random.Next(0, colors.Count)]);
            }
        }

        private static void DrawSquare(double rotationAngle, float x0, float y0, float innerLineLength, float outerLineLength, Pen color)
        {
            GraphicsDrawer.SetPosition(x0, y0);
            DrawSide(innerLineLength, outerLineLength, 0 + rotationAngle, color);
            DrawSide(innerLineLength, outerLineLength, -Math.PI / 2 + rotationAngle, color);
            DrawSide(innerLineLength, outerLineLength, Math.PI + rotationAngle, color);
            DrawSide(innerLineLength, outerLineLength, Math.PI / 2 + rotationAngle, color);
        }


        private static void DrawSide(float innerLineLength, float outerLineLength, double offsetAngle, Pen color)
        {;
            GraphicsDrawer.DrawLine(color, innerLineLength, 0 + offsetAngle);
            GraphicsDrawer.DrawLine(color, outerLineLength * Math.Sqrt(2), Math.PI / 4 + offsetAngle);
            GraphicsDrawer.DrawLine(color, innerLineLength, Math.PI + offsetAngle);
            GraphicsDrawer.DrawLine(color, innerLineLength - outerLineLength, Math.PI / 2 + offsetAngle);

            GraphicsDrawer.ChangeDirection(outerLineLength, offsetAngle - Math.PI);
            GraphicsDrawer.ChangeDirection(outerLineLength * Math.Sqrt(2), 3 * Math.PI / 4 + offsetAngle);
        }
    }
}