namespace GeometryTasks;

public class Vector
{
    public double X;
    public double Y;

    public double GetLength()
    {
        return Geometry.GetLength(this);
    }

    public Vector Add(Vector vector)
    {
        return Geometry.Add(this, vector);
    }

    public bool Belongs(Segment segment)
    {
        return Geometry.IsVectorInSegment(this, segment);
    }
}

public class Segment
{
    public Vector Begin;
    public Vector End;

    public double GetLength()
    {
        return Geometry.GetLength(this);
    }

    public bool Contains(Vector vector)
    {
        return Geometry.IsVectorInSegment(vector, this);
    }
}

public class Geometry
{
    public static double GetLength(Vector vector)
    {
        return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
    }

    public static double GetLength(Segment segment)
    {
        var x = segment.End.X - segment.Begin.X;
        var y = segment.End.Y - segment.Begin.Y;
        return Math.Sqrt(x * x + y * y);
    }

    public static bool IsVectorInSegment(Vector vector, Segment segment)
    {
        var leftSegment = new Segment
                          {
                              Begin = segment.Begin,
                              End = vector
                          };
        var rightSegment = new Segment
                           {
                               Begin = vector,
                               End = segment.End
                           };
        return Math.Abs(GetLength(segment) - (GetLength(rightSegment) + GetLength(leftSegment))) < 1e-6;
    }

    public static Vector Add(Vector vector1, Vector vector2)
    {
        return new Vector
               {
                   X = vector1.X + vector2.X,
                   Y = vector1.Y + vector2.Y
               };
    }
}