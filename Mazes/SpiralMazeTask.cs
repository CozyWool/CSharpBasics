namespace Mazes
{
    internal class SpiralMazeTask
    {
        public static void MoveOut(Robot robot, int width, int height)
        {
            var horizontalStep = width - 3;
            var verticalStep = height - 3;

            MoveIfPossible(robot, Direction.Right, horizontalStep);
            while (!robot.Finished)
            {
                MoveIfPossible(robot, Direction.Down, verticalStep);
                verticalStep -= 2;
                MoveIfPossible(robot, Direction.Left, horizontalStep);
                horizontalStep -= 2;
                MoveIfPossible(robot, Direction.Up, verticalStep);
                verticalStep -= 2;
                MoveIfPossible(robot, Direction.Right, horizontalStep);
                horizontalStep -= 2;
            }
        }

        private static void MoveIfPossible(Robot robot, Direction direction, int stepCount)
        {
            if (robot.Finished)
            {
                return;
            }
            MoveToDirection(robot, direction, stepCount);
        }
        private static void MoveToDirection(Robot robot, Direction direction, int stepCount)
        {
            for (var i = 0; i < stepCount; i++)
            {
                robot.MoveTo(direction);
            }
        }
    }
}