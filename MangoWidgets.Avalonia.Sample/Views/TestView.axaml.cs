using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MangoWidgets.Avalonia.Controls;
using MangoWidgets.Avalonia.Sample.ViewModels;
using System;

namespace MangoWidgets.Avalonia.Sample.Views;

public partial class TestView : ReactiveUserControl<TestViewModel>
{
    private readonly ScrollViewer _scrollViewer;
    private readonly ToggleSwitch _toggleButton;
    private readonly ZoomContentControl _zoomContentControl;

    public TestView()
    {
        InitializeComponent();
        _scrollViewer = this.FindControl<ScrollViewer>("ContentScrollViewer")!;
        _zoomContentControl = this.FindControl<ZoomContentControl>("ZoomHost")!;
        _toggleButton = this.FindControl<ToggleSwitch>("Switch")!;


        _scrollViewer.AddHandler(Control.PointerWheelChangedEvent, HandleScrollViewerPointerWheelChangedEvent, RoutingStrategies.Tunnel);
    }

    private void HandleScrollViewerPointerWheelChangedEvent(object? sender, PointerWheelEventArgs e)
    {
        if (_toggleButton.IsChecked is not true)
        {
            e.Handled = true;
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}