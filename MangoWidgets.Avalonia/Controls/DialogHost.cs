using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using MangoWidgets.Avalonia.Contracts;

namespace MangoWidgets.Avalonia.Controls;

[TemplatePart("PART_LayoutRoot", typeof(Panel))]
[PseudoClasses(Shown)]
public class DialogHost : ContentControl, IDialogHost
{
    private const string Shown = ":shown";
    public bool IsShown
    {
        get => GetValue(IsShownProperty);
        private set => SetValue(IsShownProperty,value);
    }
    public double DialogHeight
    {
        get => GetValue(DialogHeightProperty);
        set => SetValue(DialogHeightProperty,value);
    }
    public double DialogWidth
    {
        get => GetValue(DialogWidthProperty);
        set => SetValue(DialogWidthProperty,value);
    }

    public IBrush ShadeBrush
    {
        get => GetValue(ShadeBrushProperty);
        set => SetValue(ShadeBrushProperty, value);
    }

    public static readonly StyledProperty<bool> IsShownProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(IsShown));
    public static readonly StyledProperty<double> DialogHeightProperty =
        AvaloniaProperty.Register<DialogHost, double>(nameof(DialogHeight));
    public static readonly StyledProperty<double> DialogWidthProperty =
        AvaloniaProperty.Register<DialogHost, double>(nameof(DialogWidth));
    public static readonly StyledProperty<IBrush> ShadeBrushProperty =
        AvaloniaProperty.Register<DialogHost, IBrush>(nameof(ShadeBrush),new SolidColorBrush(Brushes.Black.Color,0.3));
    
    
    public event EventHandler<RoutedEventArgs> Opened
    {
        add => AddHandler(OpenedEvent,value);
        remove => RemoveHandler(OpenedEvent,value);
    }
    public event EventHandler<RoutedEventArgs> Closed
    {
        add => AddHandler(ClosedEvent,value);
        remove => RemoveHandler(ClosedEvent,value);
    }

    public static readonly RoutedEvent<RoutedEventArgs> OpenedEvent =
        RoutedEvent.Register<DialogHost, RoutedEventArgs>(nameof(Opened), RoutingStrategies.Bubble);
    public static readonly RoutedEvent<RoutedEventArgs> ClosedEvent =
        RoutedEvent.Register<DialogHost, RoutedEventArgs>(nameof(Closed), RoutingStrategies.Bubble);

    protected virtual void OnOpened()
    {
        var newEvent = new RoutedEventArgs(OpenedEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnClosed()
    {
        var newEvent = new RoutedEventArgs(ClosedEvent, this);
        RaiseEvent(newEvent);
    }
    
    public bool Show()
    {
        if (IsShown)
            return false;
        IsShown = true;
        return IsShown;
    }

    public bool Hide()
    {
        if (!IsShown)
            return false;
        IsShown = false;
        return true;
    }

    // protected override Type StyleKeyOverride => Type.GetType(nameof(DialogHost))!;
    private TaskCompletionSource<object?>? _tsc ;
    private void OnContentClosed(IDialogContent sender, object? result)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            _tsc?.TrySetResult(result);
            sender.Closed -= OnContentClosed;
            IsShown = false;
            Content = null;
            PseudoClasses.Set(Shown, IsShown);
        });
    }
    
    public void CloseDialog()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            _tsc?.TrySetResult(null);
            if (Content is IDialogContent content)
                content.Closed -= OnContentClosed;
            IsShown = false;
            Content = null;
            _tsc = null;
            PseudoClasses.Set(Shown, IsShown);
        });
    }

    public Task<object?> ShowDialogAsync(IDialogContent content)
    {
        if(_tsc is not null)
            CloseDialog();
		_tsc = new TaskCompletionSource<object?>();
		Dispatcher.UIThread.Invoke(() =>
        {
            Content = content;
            content.Closed -= OnContentClosed;
            content.Closed += OnContentClosed;
            IsShown = true;
            PseudoClasses.Set(Shown, IsShown);
        });
        return _tsc!.Task;
    }
}