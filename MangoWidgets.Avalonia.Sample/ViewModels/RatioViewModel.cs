using MangoWidgets.Avalonia.Sample.Contracts;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangoWidgets.Avalonia.Sample.ViewModels;

public class RatioViewModel : ViewModelBase, IMainModule
{
    public string? DisplayName { get; set; }
    public int Index { get; set; }

    [Reactive] public double Ratio { get; set; } = 1.33d;

    public RatioViewModel()
    {
        DisplayName = "比例";
        Index = 2;
    }
}
