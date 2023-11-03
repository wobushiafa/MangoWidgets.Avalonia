using System.Collections.Concurrent;
using MangoWidgets.Avalonia.Contracts;

namespace MangoWidgets.Avalonia.Services;

public class DialogService : IDialogService
{
    private readonly ConcurrentDictionary<string, IDialogHost> _dic = new();
    
    public void SetDialogHost(IDialogHost host, string? token = null)
    {
        if (string.IsNullOrEmpty(token))
            token = nameof(DialogService);
        _dic.AddOrUpdate(token!, host,(k, v) => host);
    }

    public IDialogHost? GetDialogHost(string? token = null)
    {
        if (string.IsNullOrEmpty(token))
            token = nameof(DialogService);
        return _dic.TryGetValue(token!, out var dialogHost) ? dialogHost : null;
    }

    public async Task<object?> ShowDialogAsync(IDialogContent content,string? token = null)
    {
        var dialogHost = GetDialogHost(token);
        if (dialogHost is null)
            throw new Exception($"Not Found IDialogHost with token: {token}");
        return await dialogHost.ShowDialogAsync(content);
    }
}