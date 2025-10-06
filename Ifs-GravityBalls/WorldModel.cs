using System;
using Avalonia.Input;
using Avalonia.Media;

namespace GravityBalls;

public class WorldModel
{
    public double BallX;
    public double BallY;
    public double BallRadius;
    public double WorldWidth;
    public double WorldHeight;
    public double VelocityX = 0.5;
    public double VelocityY = 0.5;
    public double Resistance = 0.2;
    public double Gravity = 2.0;
    public double CursorX = 0;
    public double CursorY = 0;
    public IBrush? Fill = Brushes.GreenYellow;

    public double CalculateCoordinate(double coordinate, double velocity, double deltaTime, double upperBorder, double lowerBorder = 0, double gravity = 0)
    {
        // var distanceToCursor = Math.Sqrt((CursorX - BallX) * (CursorX - BallX) + (CursorY - BallY) * (CursorY - BallY));
        // var cursorPushForce = 0.01 * distanceToCursor  * (velocity > 0 ? 1 : -1); 
        var delta = 200 * deltaTime * velocity * (1 - Resistance) + gravity;
        
        return Math.Min(Math.Max(coordinate + delta, lowerBorder + BallRadius), upperBorder - BallRadius);
    }

    private double CalculateDirection(double coordinate, double velocity, double upperBorder, double lowerBorder = 0)
    {
        if (coordinate >= upperBorder - BallRadius || coordinate <= BallRadius + lowerBorder)
        {
            Fill = Equals(Fill, Brushes.GreenYellow) ? Brushes.Red : Brushes.GreenYellow;
            return -1 * velocity;
        }
        return velocity;
    }
    public void SimulateTimeframe(double dt)
    {
        Process(dt);
    }

    private void Process(double dt)
    {
        BallX = CalculateCoordinate(BallX, VelocityX, dt, WorldWidth);
        BallY = CalculateCoordinate(BallY, VelocityY, dt, WorldHeight, 0, Gravity);
        
        VelocityX = CalculateDirection(BallX, VelocityX, WorldWidth);
        VelocityY = CalculateDirection(BallY, VelocityY, WorldHeight);
    }
}