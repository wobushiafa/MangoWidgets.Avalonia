using System.Collections.Generic;
using System.Reactive;
using MangoWidgets.Avalonia.Sample.Contracts;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace MangoWidgets.Avalonia.Sample.ViewModels;

public class TestViewModel : ViewModelBase,IMainModule
{
    public string? DisplayName { get; set; }
    public int Index { get; set; }


    [Reactive] public string? SelectedItem { get; set; }

    public List<string> SampleData { get; set; } = new List<string>();


    public ReactiveCommand<Unit,Unit> TestCommand { get; set; }

    public TestViewModel()
    {
        DisplayName = "测试";
        Index = 2;
        for (int i = 0; i < 100; i++)
        {
            SampleData.Add("示例数据" + i);
        }
        SelectedItem = SampleData[0];

        TestCommand = ReactiveCommand.Create(() => { SelectedItem = null; });
    }
   
}