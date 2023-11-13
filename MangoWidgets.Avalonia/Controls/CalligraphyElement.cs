using System.Collections.Specialized;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using MangoWidgets.Core;

namespace MangoWidgets.Avalonia.Controls;

public class CalligraphyElement : Control
{
    public static readonly StyledProperty<ICollection<Dot>> DotsProperty =
        AvaloniaProperty.Register<CalligraphyElement, ICollection<Dot>>(nameof(Dots));
    public ICollection<Dot> Dots
    {
        get => GetValue(DotsProperty);
        set => SetValue(DotsProperty, value);
    }
    
    
    static CalligraphyElement()
    {
        DotsProperty.Changed.AddClassHandler<CalligraphyElement>((x, e) => OnDotsChangedCallback(e));
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        _pointsSubscription?.Dispose();
        if (Dots is not INotifyCollectionChanged notifyCollection) return;
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
    private static void OnDotsChangedCallback(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Sender is not CalligraphyElement calligraphyElement) return;
        calligraphyElement._pointsSubscription?.Dispose();
        if (e.NewValue is INotifyCollectionChanged notifyCollection)
        {
            calligraphyElement._pointsSubscription = Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    handler => handler.Invoke,
                    handler => notifyCollection.CollectionChanged += handler,
                    handler => notifyCollection.CollectionChanged -= handler)
                .Subscribe(_ => Dispatcher.UIThread.Invoke(calligraphyElement.InvalidateVisual));
        }
        calligraphyElement.InvalidateVisual();
    }

    private IDisposable? _pointsSubscription;

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        // 小于两个点的无法应用算法
        if (Dots.Count < 2)
            return;
        // 用于当成笔锋的点的数量
        var tipCount = 20;
        var pointList = Dots.ToList();
        for (int i = 0; i < pointList.Count; i++)
        {
            if ((pointList.Count - i) < tipCount)
            {
                pointList[i] = pointList[i] with
                {
                    Force = (pointList.Count - i) * 1f / tipCount
                };
            }
            else
            {
                pointList[i] = pointList[i] with
                {
                    Force = 1.0f
                };
            }
        }
        // 笔迹大小，笔迹粗细
        int inkSize = 26;
        
        var list = GetOutlinePointList(pointList,inkSize);
        var geometry = new StreamGeometry();
        using var ctx = geometry.Open();
        var first = list.First();
        ctx.BeginFigure(first,true);
        foreach (var p in list.Skip(1))
        {
            ctx.LineTo(p);
        }
        context.DrawGeometry(Brushes.Red,pen,geometry);
        //context.DrawGeometry(Brushes.Red,pen,new PolylineGeometry(list.ToList(),true));
    }

    private Pen pen = new Pen(Brushes.Red, 1,null,PenLineCap.Round, PenLineJoin.Round);
    public static Point[] GetOutlinePointList(List<Dot> pointList, int inkSize)
    {
        if (pointList.Count < 2)
        {
            throw new ArgumentException("小于两个点的无法应用算法");
        }

        var pointCount = pointList.Count * 2 /*两边的笔迹轨迹*/ + 1 /*首点重复*/ + 1 /*末重复*/;

        var outlinePointList = new Point[pointCount];

        // 用来计算笔迹点的两点之间的向量角度
        double angle = 0.0;
        for (var i = 0; i < pointList.Count; i++)
        {
            var currentPoint = pointList[i];

            // 如果不是最后一点，那就可以和笔迹当前轨迹点的下一点进行计算向量角度
            if (i < pointList.Count - 1)
            {
                var nextPoint = pointList[i + 1];
                var x = nextPoint.X - currentPoint.X;
                var y = nextPoint.Y - currentPoint.Y;
                // 拿着纸笔自己画一下吧，这个是简单的数学计算
                angle = Math.Atan2(y, x) - Math.PI / 2;
            }

            // 笔迹粗细的一半，一边用一半，合起来就是笔迹粗细了
            var halfThickness = inkSize / 2d;

            // 压感这里是直接乘法而已
            halfThickness *= currentPoint.Force;
            // 不能让笔迹粗细太小
            halfThickness = Math.Max(0.01, halfThickness);

            var leftX = currentPoint.X + (Math.Cos(angle) * halfThickness);
            var leftY = currentPoint.Y + (Math.Sin(angle) * halfThickness);

            var rightX = currentPoint.X - (Math.Cos(angle) * halfThickness);
            var rightY = currentPoint.Y - (Math.Sin(angle) * halfThickness);

            outlinePointList[i + 1] = new Point(leftX, leftY);
            outlinePointList[pointCount - i - 1] = new Point(rightX, rightY);
        }

        outlinePointList[0] = new Point(pointList[0].X,pointList[0].Y);
        outlinePointList[pointList.Count + 1] = new Point(pointList.Last().X,pointList.Last().Y);
        return outlinePointList;
    }
}