namespace MangoWidgets.Core;

public class Zoom
{
    public static readonly Zoom Default = new Zoom(1d, 1d, 0d, 0d);
    public double ScaleX { get; set; }
    public double ScaleY { get; set; }
    public double TranslateX { get; set; }
    public double TranslateY { get; set; }

    public Zoom(double scaleX, double scaleY, double translateX, double translateY)
    {
        (ScaleX, ScaleY, TranslateX, TranslateY) = (scaleX, scaleY, translateX, translateY);
    }
}