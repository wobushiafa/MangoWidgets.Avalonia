using System.Collections.Generic;
using System.Linq;
using MangoWidgets.Avalonia.Sample.Contracts;
using ReactiveUI.Fody.Helpers;

namespace MangoWidgets.Avalonia.Sample.ViewModels;

public class ShellViewModel : ViewModelBase
{
    [Reactive] public IMainModule? CurrentModule { get; set; }
    public List<IMainModule> Modules { get; set; } = new();

    public ShellViewModel(IEnumerable<IMainModule> mainModules)
    {
        Modules.AddRange(mainModules.OrderBy(x => x.Index));
        CurrentModule = Modules.FirstOrDefault();
    }
}