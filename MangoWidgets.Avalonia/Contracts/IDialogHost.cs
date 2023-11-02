using Avalonia.Interactivity;
using Avalonia.Media;

namespace MangoWidgets.Avalonia.Contracts;

public interface IDialogHost
{
    /// <summary>
    /// 获取 <see cref="IDialogHost"/> 是否在显示状态
    /// </summary>
    bool IsShown { get; }

    /// <summary>
    /// 获取或设置<see cref="IDialogContent"/>的宽度最大值
    /// </summary>
    double DialogWidth { get; set; }

    /// <summary>
    /// 获取或设置<see cref="IDialogContent"/>的高度最大值
    /// </summary>
    double DialogHeight { get; set; }
    
    IBrush ShadeBrush { get; set; }

    /// <summary>
    /// 对话框打开时
    /// </summary>
    public event EventHandler<RoutedEventArgs> Opened;

    /// <summary>
    /// 对话框被关闭时
    /// </summary>
    public event EventHandler<RoutedEventArgs> Closed;

    /// <summary>
    /// 显示 <see cref="IDialogHost"/>
    /// </summary>
    /// <returns></returns>
    bool Show();

    /// <summary>
    /// 隐藏 <see cref="IDialogHost"/>
    /// </summary>
    /// <returns></returns>
    bool Hide();

    /// <summary>
    /// 显示<see cref="IDialogHost"/>并等待<see cref="IDialogContent"/>控件退出
    /// </summary>
    /// <returns></returns>
    Task<object?> ShowDialogAsync(IDialogContent content);
}