using System.Collections.Generic;
using MangoWidgets.Avalonia.Sample.Contracts;

namespace MangoWidgets.Avalonia.Sample.ViewModels;

public class TestViewModel : ViewModelBase,IMainModule
{
    public string? DisplayName { get; set; }
    public int Index { get; set; }
    
    public List<string> SampleData { get; set; } = new List<string>();

    public TestViewModel()
    {
        DisplayName = "测试";
        Index = 2;
        for (int i = 0; i < 100; i++)
        {
            SampleData.Add("示例数据" + i);
        }
    }
}