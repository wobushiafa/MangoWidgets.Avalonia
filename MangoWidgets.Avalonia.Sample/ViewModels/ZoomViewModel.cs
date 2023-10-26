using MangoWidgets.Avalonia.Sample.Contracts;

namespace MangoWidgets.Avalonia.Sample.ViewModels;

public class ZoomViewModel : ViewModelBase , IMainModule
{
    public string? DisplayName { get; set; }
    public int Index { get; set; }

    public ZoomViewModel()
    {
        DisplayName = "变焦";
        Index = 1;
    }
}