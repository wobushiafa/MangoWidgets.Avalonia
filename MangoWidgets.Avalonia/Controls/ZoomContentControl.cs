using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace MangoWidgets.Avalonia.Controls;

public class ZoomContentControl : ContentControl
{
    public static readonly StyledProperty<bool> CanZoomProperty
        = AvaloniaProperty.Register<ZoomContentControl, bool>(nameof(CanZoom));
    public bool CanZoom
    {
        get => GetValue(CanZoomProperty);
        set => SetValue(CanZoomProperty,value);
    }
    
    private static readonly RoutedEvent<ZoomedEventArgs> ZoomedEvent = RoutedEvent.Register<ZoomContentControl,ZoomedEventArgs>(nameof(Zoomed), RoutingStrategies.Bubble);
    public event EventHandler<ZoomedEventArgs> Zoomed
    {
        add => AddHandler(ZoomedEvent, value);
        remove => RemoveHandler(ZoomedEvent, value);
    }
    
    protected override Type StyleKeyOverride => typeof(ContentControl);
    
    static ZoomContentControl()
    {
        ClipToBoundsProperty.OverrideDefaultValue<LayoutTransformControl>(false);
        ContentProperty.Changed.AddClassHandler<ZoomContentControl>(HandleContentChanged);
    }

    private static void HandleContentChanged(ZoomContentControl sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender.Presenter?.Child is not { } ctrl) return;
        ctrl.RenderTransform = CreateTransformGroup();
    }
    
    private static TransformGroup CreateTransformGroup()
    {
        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(new ScaleTransform());
        transformGroup.Children.Add(new TranslateTransform());
        return transformGroup;
    }

    private bool EnsureTransformGroup(out TransformGroup transformGroup)
    {
        transformGroup = CreateTransformGroup();
        if (Presenter is null) return false;
        if (Presenter?.Child?.RenderTransform is not TransformGroup ts)
        {
            Presenter!.Child!.RenderTransform = transformGroup;
            Presenter!.Child!.RenderTransformOrigin = RelativePoint.TopLeft;
        }
        else
            transformGroup = ts;
        return true;
    }
    
    private bool TryParseTransformGroup(out ScaleTransform scaleTransform, out TranslateTransform translateTransform)
    {
        (scaleTransform,translateTransform) = (new ScaleTransform(),new TranslateTransform());
        if (Presenter is null) return false;
        if (Presenter?.Child?.RenderTransform is not TransformGroup ts)
        {
            ts = CreateTransformGroup();
            Presenter!.Child!.RenderTransform = ts;
            Presenter!.Child!.RenderTransformOrigin = RelativePoint.TopLeft;
        }
        
        if (ts.Children.Count < 2)
            ts = CreateTransformGroup();
        
        if (ts.Children[0] is not ScaleTransform s || ts.Children[1] is not TranslateTransform t)
        {
            ts = CreateTransformGroup();
            s = (ScaleTransform)ts.Children[0];
            t = (TranslateTransform)ts.Children[1];
        }
        (scaleTransform,translateTransform) = (s,t);
        return true;
        // switch (transformGroup.Children)
        // {
        //     case [ScaleTransform s, TranslateTransform t]:
        //         (scaleTransform, translateTransform) = (s, t);
        //         return true;
        //     case [TranslateTransform t2, ScaleTransform s2]:
        //         (scaleTransform, translateTransform) = (s2, t2);
        //         return true;
        // }
    }
    
    /// <summary>
    /// 校正可视区域
    /// </summary>
    private void FixTransformArea(ScaleTransform scaleTransform, TranslateTransform translateTransform)
    {
        if (scaleTransform.ScaleX < 1 || scaleTransform.ScaleY < 1)
        {
            scaleTransform.ScaleX = scaleTransform.ScaleY = 1;
            translateTransform.X = translateTransform.Y = 0;
        }
        else
        {
            //缩放后的大小
            var width = scaleTransform.ScaleX * this.Bounds.Width;
            var height = scaleTransform.ScaleY * this.Bounds.Height;

            //缩放后的大小与原始大小的差
            var widthOffset = this.Bounds.Width - width;
            var heightOffset = this.Bounds.Height - height;

            translateTransform.X = translateTransform.X < widthOffset ? widthOffset : translateTransform.X > 0 ? 0 : translateTransform.X;
            translateTransform.Y = translateTransform.Y < heightOffset ? heightOffset : translateTransform.Y > 0 ? 0 : translateTransform.Y;
        }
        RaiseZoomedEvent(scaleTransform, translateTransform);
    }

    /// <summary>
    /// 触发Zoomed事件
    /// </summary>
    /// <param name="scaleTransform"></param>
    /// <param name="translateTransform"></param>
    private void RaiseZoomedEvent(ScaleTransform scaleTransform,TranslateTransform translateTransform)
    {
        var moveX = translateTransform.X / Width;
        var moveY = translateTransform.Y / Height;
        var args = new ZoomedEventArgs(ZoomedEvent,this)
        {
            ScaleX = scaleTransform.ScaleX,
            ScaleY= scaleTransform.ScaleY,
            MoveX= moveX is double.NaN ? 0 : moveX,
            MoveY= moveY is double.NaN ? 0 : moveY,
        };
        RaiseEvent(args);
    }
    
    
    #region  MouseEvent

    private Point? _pressedPoint;

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!CanZoom) return;
        _pressedPoint = e.GetPosition(this);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (!CanZoom) return;
        _pressedPoint = null;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (!CanZoom) return;
        if (!e.GetCurrentPoint(e.Source as Visual).Properties.IsLeftButtonPressed) return;
        if (!TryParseTransformGroup(out var scaleTransform, out var translateTransform)) return;
        if (_pressedPoint is null) return;

        var position = e.GetPosition(this);

        //缩放后的尺寸
        var scaleWidth = this.Width * scaleTransform.ScaleX;
        var scaleHeight = this.Height * scaleTransform.ScaleY;

        //缩放后与原始大小的差距
        var widthOffset = this.Width - scaleWidth;
        var heightOffset = this.Height - scaleHeight;

        //当前申请的平移分量
        var diffX = position.X - _pressedPoint.Value.X;
        var diffY = position.Y - _pressedPoint.Value.Y;

        if (((translateTransform.X + diffX) < widthOffset) || ((translateTransform.Y + diffY) < heightOffset) || ((translateTransform.X + diffX) > 0) ||
            ((translateTransform.Y + diffY) > 0))
        {
            //如果申请平移量使图像边界进入容器内则不执行平移
        }
        else
        {
            translateTransform.X += diffX;
            translateTransform.Y += diffY;
        }

        _pressedPoint = position;
        FixTransformArea(scaleTransform,translateTransform);
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        
        if (!CanZoom) return;
        if (!EnsureTransformGroup(out var transformGroup)) return;
        if (!TryParseTransformGroup(out var scaleTransform, out var translateTransform)) return;

        var point = e.GetCurrentPoint(this).Position;
        var point2Content = transformGroup.Value.Invert().Transform(point);
        var delta = (e.Delta.Y + e.Delta.X) / 2 * 0.1 ;
        scaleTransform.ScaleX = scaleTransform.ScaleY = scaleTransform.ScaleX + delta;
        translateTransform.X = -1 * (point2Content.X * scaleTransform.ScaleX - point.X);
        translateTransform.Y = -1 * (point2Content.Y * scaleTransform.ScaleY - point.Y);
        FixTransformArea(scaleTransform,translateTransform);
    }

    #endregion
    
}

public class ZoomedEventArgs : RoutedEventArgs
{
    public double ScaleX { get; set; }
    public double ScaleY { get; set; }
    public double MoveX { get; set; }
    public double MoveY { get; set; }
    
    public ZoomedEventArgs(RoutedEvent? routedEvent) :base (routedEvent){ }
    public ZoomedEventArgs(RoutedEvent? routedEvent, object? source) : base(routedEvent, source){ }
}