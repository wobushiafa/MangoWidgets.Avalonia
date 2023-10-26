using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MangoWidgets.Avalonia.Sample.ViewModels;

namespace MangoWidgets.Avalonia.Sample.Views;

public partial class ZoomView : ReactiveUserControl<ZoomViewModel>
{
    public ZoomView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}