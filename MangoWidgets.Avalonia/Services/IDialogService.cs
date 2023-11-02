using MangoWidgets.Avalonia.Contracts;

namespace MangoWidgets.Avalonia.Services;

public interface IDialogService
{
    void SetDialogHost(IDialogHost host, string? token = nameof(MangoWidgets));

    IDialogHost GetDialogHost(string? token = nameof(MangoWidgets));

    Task<bool?> ShowDialogAsync(IDialogContent content,string? token = nameof(MangoWidgets));
}