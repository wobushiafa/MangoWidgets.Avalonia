using Avalonia;
using Avalonia.Controls;

namespace MangoWidgets.Avalonia.Controls;

public class RatioBox : Decorator
{
    public double Ratio
    {
        get => GetValue(RatioProperty);
        set => SetValue(RatioProperty, value);
    }

    public static readonly StyledProperty<double> RatioProperty =
        AvaloniaProperty.Register<RatioBox, double>(nameof(Ratio), 1d);

    static RatioBox()
    {
        RatioProperty.Changed.AddClassHandler<RatioBox>(OnRatioBoxChangedCallback);
    }

    private static void OnRatioBoxChangedCallback(RatioBox sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender is not { } ratioBox) return;
        ratioBox.InvalidateMeasure();
        ratioBox.InvalidateArrange();
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var mratio = Ratio;
        if (double.IsNaN(mratio))
        {
            return base.MeasureOverride(availableSize);
        }
        else
        {
            if (Child == null) return new Size();
            var h = availableSize.Height;
            var w = h * mratio;
            if (w > availableSize.Width)
            {
                w = availableSize.Width;
                h = w / mratio;
            }
            Child.Measure(new Size(w, h));
            return new Size();
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var mratio = Ratio;
        if (double.IsNaN(mratio))
        {
            return base.ArrangeOverride(finalSize);
        }
        else
        {
            if (Child == null) return finalSize;
            var h = finalSize.Height;
            var w = h * mratio;
            if (w > finalSize.Width)
            {
                w = finalSize.Width;
                h = w / mratio;
            }
            var x = (finalSize.Width - w) / 2;
            var y = (finalSize.Height - h) / 2;
            var cb = new Rect(x, y, w, h);
            Child.Arrange(cb);
            return finalSize;
        }
    }
}