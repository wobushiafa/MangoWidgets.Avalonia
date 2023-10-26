using Avalonia.ReactiveUI;
using MangoWidgets.Avalonia.Sample.ViewModels;

namespace MangoWidgets.Avalonia.Sample.Views;

public partial class ShellView : ReactiveWindow<ShellViewModel>
{
    public ShellView()
    {
        InitializeComponent();
    }
}