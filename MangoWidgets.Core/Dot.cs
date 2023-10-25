namespace MangoWidgets.Core;

public struct Dot
{
    public int X { get; set; }
    
    public int Y { get; set; }
    
    public ulong Timestamp { get; set; }
    
    public int Force { get; set; }
    
    public Dot(int x,int y)
    {
        (X, Y) = (x, y);
    }
}