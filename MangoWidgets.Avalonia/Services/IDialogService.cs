using MangoWidgets.Avalonia.Contracts;

namespace MangoWidgets.Avalonia.Services;

public interface IDialogService
{
    void SetDialogHost(IDialogHost host, string? token = null);

    IDialogHost? GetDialogHost(string? token = null);

    Task<object?> ShowDialogAsync(IDialogContent content,string? token = null);
}