using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MangoWidgets.Avalonia.Sample.Views;

public partial class ZoomView : UserControl
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