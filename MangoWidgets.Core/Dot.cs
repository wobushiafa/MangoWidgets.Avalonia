namespace MangoWidgets.Core;

public struct Dot
{
    public int X { get; set; }
    public int Y { get; set; }

    public ulong Timestamp { get; set; }
    public double Force { get; set; } = 0.5f;

    public Dot(int x, int y)
    {
        (X, Y) = (x, y);
    }

    public Dot(int x, int y, ulong timestamp, double force) : this(x, y)
    {
        (Timestamp, Force) = (timestamp, force);
    }
}