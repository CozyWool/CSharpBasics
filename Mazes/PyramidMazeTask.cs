namespace Mazes;

public static class PyramidMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        var step = width - 3;
        while (true)
        {
            MoveToDirection(robot, Direction.Right, step);
            step -= 2;
            MoveToDirection(robot, Direction.Up, 2);
            MoveToDirection(robot, Direction.Left, step);
            step -= 2;
            if (robot.Finished)
            {
                break;
            }

            MoveToDirection(robot, Direction.Up, 2);
        }
    }

    private static void MoveToDirection(Robot robot, Direction direction, int stepCount)
    {
        for (var i = 0; i < stepCount; i++)
        {
            robot.MoveTo(direction);
        }
    }
}