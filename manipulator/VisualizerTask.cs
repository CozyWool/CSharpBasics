using System;
using System.Globalization;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

namespace Manipulation;

public static class VisualizerTask
{
    public static double X = 220;
    public static double Y = -100;
    public static double Alpha = 0.05;
    public static double Wrist = 2 * Math.PI / 3;
    public static double Elbow = 3 * Math.PI / 4;
    public static double Shoulder = Math.PI / 2;

    public static Brush UnreachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 255, 230, 230));
    public static Brush ReachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 230, 255, 230));
    public static Pen ManipulatorPen = new Pen(Brushes.Black, 3);
    public static Brush JointBrush = new SolidColorBrush(Colors.Gray);

    public static void KeyDown(Visual visual, KeyEventArgs key)
    {
        var step = Math.PI / 24;
        switch (key.Key)
        {
            case Key.Q:
                Shoulder += step;
                break;
            case Key.A:
                Shoulder -= step;
                break;
            case Key.W:
                Elbow += step;
                break;
            case Key.S:
                Elbow -= step;
                break;
        }

        Wrist = -Alpha - Shoulder - Elbow;
        visual.InvalidateVisual();
    }

    public static void MouseMove(Visual visual, PointerEventArgs e)
    {
        var windowPoint = e.GetPosition(visual);
        var newP = ConvertWindowToMath(windowPoint, GetShoulderPos(visual));
        X = newP.X;
        Y = newP.Y;
        UpdateManipulator();
        visual.InvalidateVisual();
    }

    public static void MouseWheel(Visual visual, PointerWheelEventArgs e)
    {
        Alpha += e.Delta.Y * 0.1;

        UpdateManipulator();
        visual.InvalidateVisual();
    }

    public static void UpdateManipulator()
    {
        var values = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
        if (double.IsNaN(values[0]) && double.IsNaN(values[1]) && double.IsNaN(values[2]))
        {
            return;
        }

        Shoulder = values[0];
        Elbow = values[1];
        Wrist = values[2];
    }

    public static void DrawManipulator(DrawingContext context, Point shoulderPos)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);


        DrawReachableZone(context, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos);
        joints[0] = ConvertMathToWindow(joints[0], shoulderPos);
        joints[1] = ConvertMathToWindow(joints[1], shoulderPos);
        joints[2] = ConvertMathToWindow(joints[2], shoulderPos);

        var formattedText = new FormattedText(
                                              $"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}",
                                              CultureInfo.InvariantCulture,
                                              FlowDirection.LeftToRight,
                                              Typeface.Default,
                                              18,
                                              Brushes.DarkRed
                                             )
                            {
                                TextAlignment = TextAlignment.Center
                            };
        context.DrawText(formattedText, new Point(10, 10));


        var radius = 5.0;
        context.DrawLine(ManipulatorPen, shoulderPos, joints[0]);
        context.DrawEllipse(JointBrush, null, shoulderPos, radius, radius);
        for (var i = 0; i < 2; i++)
        {
            context.DrawLine(ManipulatorPen, joints[i], joints[i + 1]);
            context.DrawEllipse(JointBrush, null, joints[i], radius, radius);
        }

        context.DrawEllipse(JointBrush, null, joints[2], radius / 2, radius / 2);
    }

    private static void DrawReachableZone(
        DrawingContext context,
        Brush reachableBrush,
        Brush unreachableBrush,
        Point shoulderPos)
    {
        var rMin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
        var rMax = Manipulator.UpperArm + Manipulator.Forearm;

        context.DrawEllipse(reachableBrush,
                            null,
                            shoulderPos,
                            rMax, rMax);
        context.DrawEllipse(unreachableBrush,
                            null,
                            shoulderPos,
                            rMin, rMin);
    }

    public static Point GetShoulderPos(Visual visual)
    {
        return new Point(visual.Bounds.Width / 2, visual.Bounds.Height / 2);
    }

    public static Point ConvertMathToWindow(Point mathPoint, Point shoulderPos)
    {
        return new Point(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);
    }

    public static Point ConvertWindowToMath(Point windowPoint, Point shoulderPos)
    {
        return new Point(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
    }
}