namespace ReadOnlyVector;

public class ReadOnlyVector(double x, double y)
{
    public readonly double X = x;
    public readonly double Y = y;

    public ReadOnlyVector WithX(double x)
    {
        return new ReadOnlyVector(x, Y);
    }

    public ReadOnlyVector WithY(double y)
    {
        return new ReadOnlyVector(X, y);
    }

    public ReadOnlyVector Add(ReadOnlyVector other)
    {
        return new ReadOnlyVector(X + other.X, Y + other.Y);
    }
}