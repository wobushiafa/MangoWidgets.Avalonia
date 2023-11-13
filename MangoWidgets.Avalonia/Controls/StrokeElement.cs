using System.Collections.Specialized;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using MangoWidgets.Core;

namespace MangoWidgets.Avalonia.Controls;

public class StrokeElement : Control
{
    public static readonly StyledProperty<ICollection<Dot>> PointsProperty =
        AvaloniaProperty.Register<StrokeElement, ICollection<Dot>>(nameof(Points));

    public static readonly StyledProperty<IBrush> StrokeProperty =
        AvaloniaProperty.Register<StrokeElement, IBrush>(nameof(Stroke), Brushes.Black);

    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<StrokeElement, double>(nameof(StrokeThickness), 1d);

    public ICollection<Dot> Points
    {
        get => GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }

    public IBrush Stroke
    {
        get => GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    static StrokeElement()
    {
        PointsProperty.Changed.AddClassHandler<StrokeElement>((x, e) => OnPointsChangedCallback(e));
        StrokeProperty.Changed.AddClassHandler<StrokeElement>((x, e) => OnStrokeStyleChangedCallback(e));
        StrokeThicknessProperty.Changed.AddClassHandler<StrokeElement>((x, e) => OnStrokeStyleChangedCallback(e));
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        _pointsSubscription?.Dispose();
        if (Points is not INotifyCollectionChanged notifyCollection) return;
        _pointsSubscription = Observable
            .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                handler => handler.Invoke,
                handler => notifyCollection.CollectionChanged += handler,
                handler => notifyCollection.CollectionChanged -= handler)
            .Subscribe(_ => Dispatcher.UIThread.Invoke(this.InvalidateVisual));
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        _pointsSubscription?.Dispose();
    }

    private static void OnStrokeStyleChangedCallback(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Sender is not StrokeElement strokeElement) return;
        strokeElement.CreatePen(strokeElement.GetValue(StrokeProperty),
            strokeElement.GetValue(StrokeThicknessProperty));
        strokeElement.InvalidateVisual();
    }

    private IDisposable? _pointsSubscription;

    private static void OnPointsChangedCallback(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Sender is not StrokeElement strokeElement) return;
        strokeElement._pointsSubscription?.Dispose();
        if (e.NewValue is INotifyCollectionChanged notifyCollection)
        {
            strokeElement._pointsSubscription = Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    handler => handler.Invoke,
                    handler => notifyCollection.CollectionChanged += handler,
                    handler => notifyCollection.CollectionChanged -= handler)
                .Subscribe(_ => Dispatcher.UIThread.Invoke(strokeElement.InvalidateVisual));
        }

        strokeElement.InvalidateVisual();
    }

    private Pen _pen = new(Brushes.Black, 2, null, PenLineCap.Round, PenLineJoin.Round);

    private void CreatePen(IBrush stroke, double strokeThickness)
    {
        _pen = new Pen(stroke, strokeThickness, null, PenLineCap.Round, PenLineJoin.Round);
    }


    public override void Render(DrawingContext context)
    {
        if (Points.Count < 2) return;
        var geometry = new StreamGeometry();
        using var ctx = geometry.Open();

        var first = Points.First();
        for (var i = 0; i < Points.Count; i++)
            if (i == 0)
                ctx.BeginFigure(new Point(first.X, first.Y), false);
            else
            {
                var (lst, current) = (Points.ElementAt(i - 1), Points.ElementAt(i));
                var lstPoint = new Point(lst.X, lst.Y);
                var point = new Point(current.X, current.Y);
                DrawBezier(ctx, lstPoint, point);
            }
        context.DrawGeometry(null, _pen, geometry);
    }

    private static void DrawBezier(IGeometryContext ctx, Point lastPoint, Point curPoint)
    {
        var offsetX = curPoint.X - lastPoint.X;
        var offsetY = curPoint.Y - lastPoint.Y;
        var fin = Math.Sqrt(Math.Pow(offsetX, 2.0) + Math.Pow(offsetY, 2.0));
        switch (fin)
        {
            case >= 2:
            {
                var ctrl = new Point(lastPoint.X + offsetX / 3.0, lastPoint.Y + offsetY / 3.0);
                var end = new Point(lastPoint.X + offsetX / 2.0, lastPoint.Y + offsetY / 2.0);
                ctx.CubicBezierTo(lastPoint, ctrl, end);
                break;
            }
            case >= 1:
            {
                var ctrl = new Point(lastPoint.X + offsetX / 2.0, lastPoint.Y + offsetY / 2.0);
                ctx.QuadraticBezierTo(lastPoint, ctrl);
                break;
            }
        }
    }
    
}