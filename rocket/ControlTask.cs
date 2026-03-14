using System;

namespace func_rocket;

public class ControlTask
{
    public static Turn ControlRocket(Rocket rocket, Vector target)
    {
        var toTarget = target - rocket.Location;
        var angleDiff = toTarget.Angle - rocket.Direction;

        if (Math.Abs(angleDiff) < 0.8 || Math.Abs(toTarget.Angle - rocket.Velocity.Angle) < 0.8)
        {
            angleDiff = -(rocket.Velocity.Angle + rocket.Direction) / 2 + toTarget.Angle;
        }

        return angleDiff > 0 ? Turn.Right : (angleDiff < 0 ? Turn.Left : Turn.None);
    }
}