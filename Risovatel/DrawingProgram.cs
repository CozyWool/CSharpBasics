using System;
using Avalonia.Media;
using RefactorMe.Common;

namespace RefactorMe
{
    class GraphicsDrawer
    {
        private static float x, y;
        private static IGraphics? _graphics;

        public static void Initialize(IGraphics newGraphics)
        {
            _graphics = newGraphics;
            _graphics.Clear(Colors.Black);
        }

        public static void SetPosition(float x0, float y0)
        {
            x = x0;
            y = y0;
        }

        public static void DrawLine(Pen pen, double length, double angle)
        {
            var x1 = (float) (x + length * Math.Cos(angle));
            var y1 = (float) (y + length * Math.Sin(angle));
            _graphics?.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void ChangeDirection(double length, double angle)
        {
            x = (float) (x + length * Math.Cos(angle));
            y = (float) (y + length * Math.Sin(angle));
        }
    }

    public class ImpossibleSquare
    {
        private static float innerLineMultiplier = 0.375f;
        private static float outerLineMultiplier = 0.04f;

        public static void Draw(int width, int height, double rotationAngle, IGraphics graphics)
        {
            GraphicsDrawer.Initialize(graphics);

            var size = Math.Min(width, height);

            var diagonalLength = Math.Sqrt(2) * (size * 0.375f + size * 0.04f) / 2;
            var x0 = (float) (diagonalLength * Math.Cos(Math.PI / 4 + Math.PI)) + width / 2f;
            var y0 = (float) (diagonalLength * Math.Sin(Math.PI / 4 + Math.PI)) + height / 2f;

            var color = new Pen(Brushes.Yellow);
            DrawSquare(rotationAngle,
                       x0,
                       y0,
                       innerLineMultiplier * size,
                       outerLineMultiplier * size,
                       color);
            GraphicsDrawer.SetPosition(x0, y0);
        }

        private static void DrawSquare(double rotationAngle, float x0, float y0, float innerLineLength,
                                       float outerLineLength, Pen color)
        {
            GraphicsDrawer.SetPosition(x0, y0);
            DrawSide(innerLineLength, outerLineLength, 0 + rotationAngle, color);
            DrawSide(innerLineLength, outerLineLength, -Math.PI / 2 + rotationAngle, color);
            DrawSide(innerLineLength, outerLineLength, Math.PI + rotationAngle, color);
            DrawSide(innerLineLength, outerLineLength, Math.PI / 2 + rotationAngle, color);
        }

        private static void DrawSide(float innerLineLength, float outerLineLength, double offsetAngle, Pen color)
        {
            GraphicsDrawer.DrawLine(color, innerLineLength, 0 + offsetAngle);
            GraphicsDrawer.DrawLine(color, outerLineLength * Math.Sqrt(2), Math.PI / 4 + offsetAngle);
            GraphicsDrawer.DrawLine(color, innerLineLength, Math.PI + offsetAngle);
            GraphicsDrawer.DrawLine(color, innerLineLength - outerLineLength, Math.PI / 2 + offsetAngle);

            GraphicsDrawer.ChangeDirection(outerLineLength, offsetAngle - Math.PI);
            GraphicsDrawer.ChangeDirection(outerLineLength * Math.Sqrt(2), 3 * Math.PI / 4 + offsetAngle);
        }
    }
}