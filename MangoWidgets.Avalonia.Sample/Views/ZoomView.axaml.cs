using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MangoWidgets.Avalonia.Controls;
using MangoWidgets.Avalonia.Sample.ViewModels;

namespace MangoWidgets.Avalonia.Sample.Views;

public partial class ZoomView : ReactiveUserControl<ZoomViewModel>
{
    private readonly ZoomContentControl _content;
    
    public ZoomView()
    {
        InitializeComponent();
        _content = this.FindControl<ZoomContentControl>("MainZoom")!;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void ZoomContentControl_OnZoomed(object? sender, ZoomedEventArgs e)
    {
       // _content.Zoom = new Zoom(e.Zoom.ScaleX,e.Zoom.ScaleY,e.Zoom.TranslateX,e.Zoom.TranslateY);
    }
}