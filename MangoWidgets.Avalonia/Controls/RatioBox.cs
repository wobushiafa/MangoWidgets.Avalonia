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

    public bool AlwaysFullWith
    {
        get => GetValue(AlwaysFullWithProperty);
        set => SetValue(AlwaysFullWithProperty, value);
    }

    public static readonly StyledProperty<double> RatioProperty =
        AvaloniaProperty.Register<RatioBox, double>(nameof(Ratio), 1d);
    public static readonly StyledProperty<bool> AlwaysFullWithProperty =
    AvaloniaProperty.Register<RatioBox, bool>(nameof(AlwaysFullWith), false);

    static RatioBox()
    {
        RatioProperty.Changed.AddClassHandler<RatioBox>(OnRatioBoxChangedCallback);
        AlwaysFullWithProperty.Changed.AddClassHandler<RatioBox>(OnRatioBoxChangedCallback);
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

            if (AlwaysFullWith)
            {
                w = availableSize.Width;
                h = w / mratio;
            }
            else
            {
                if (w > availableSize.Width)
                {
                    w = availableSize.Width;
                    h = w / mratio;
                }
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

            if (AlwaysFullWith)
            {
                w = finalSize.Width;
                h = w / mratio;
            }
            else
            {
                if (w > finalSize.Width)
                {
                    w = finalSize.Width;
                    h = w / mratio;
                }
            }

            var x = (finalSize.Width - w) / 2;
            var y = (finalSize.Height - h) / 2;
            var cb = new Rect(x, y, w, h);
            Child.Arrange(cb);
            return finalSize;
        }
    }
}