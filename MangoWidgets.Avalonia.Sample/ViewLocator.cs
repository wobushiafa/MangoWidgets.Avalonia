using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MangoWidgets.Avalonia.Sample.ViewModels;
using ReactiveUI;
using Splat;

namespace MangoWidgets.Avalonia.Sample;

public class ViewLocator : IDataTemplate, IViewLocator
{
    public Control Build(object data)
    {
        try
        {
            var viewModelType = data?.GetType();
            if (viewModelType is null)
                return new TextBlock() { Text = $"Could not find the viewmodel:{data}" };
            var viewType = typeof(IViewFor<>);
            viewType = viewType.MakeGenericType(viewModelType);
            return App.Resolve(viewType) as Control;
        }
        catch
        {
            return new TextBlock() { Text = $"Could not find the viewmodel:{data}" };
        }
    }

    public bool Match(object data)
    {
        return data is ViewModelBase;
    }

    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
    {
        try
        {
            var viewModelType = viewModel?.GetType();
            if (viewModelType is null)
                throw new Exception($"Could not find the viewmodel:{viewModel}");
            var viewType = typeof(IViewFor<>);
            viewType = viewType.MakeGenericType(viewModelType);
            return App.Resolve(viewType) as IViewFor;
        }
        catch (Exception)
        {
            this.Log().Error($"Could not instantiate view {viewModel}.");
            throw;
        }
    }
}