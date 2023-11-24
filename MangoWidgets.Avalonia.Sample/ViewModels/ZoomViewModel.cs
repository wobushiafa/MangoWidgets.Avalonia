using MangoWidgets.Avalonia.Sample.Contracts;
using MangoWidgets.Core;
using ReactiveUI.Fody.Helpers;

namespace MangoWidgets.Avalonia.Sample.ViewModels;

public class ZoomViewModel : ViewModelBase , IMainModule
{
    public string? DisplayName { get; set; }
    public int Index { get; set; }
    
    [Reactive] public Zoom Zoom { get; set; } = Zoom.Default;

    public ZoomViewModel()
    {
        DisplayName = "变焦";
        Index = 1;
        /*this.WhenAnyValue(x => x.Zoom)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(e =>
            {
                Console.WriteLine("zoom改变了!");
                OtherZoom = e;
            });*/
    }
}