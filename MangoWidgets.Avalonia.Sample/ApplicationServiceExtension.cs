using MangoWidgets.Avalonia.Sample.Contracts;
using MangoWidgets.Avalonia.Sample.ViewModels;
using MangoWidgets.Avalonia.Sample.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace MangoWidgets.Avalonia.Sample;

public static class ApplicationServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddTransient<IViewFor<ShellViewModel>, ShellView>()
            .AddTransient<ShellViewModel>()
            
            .AddTransient<IViewFor<ZoomViewModel>, ZoomView>()
            .AddTransient<IMainModule,ZoomViewModel>()
            .AddTransient<IViewFor<DrawingBoardViewModel>, DrawingBoardView>()
            .AddTransient<IMainModule,DrawingBoardViewModel>()
            .AddTransient<IViewFor<TestViewModel>, TestView>()
            .AddTransient<IMainModule,TestViewModel>()
            .AddTransient<IViewFor<RatioViewModel>, RatioView>()
            .AddTransient<IMainModule, RatioViewModel>()
            ;
        
        return serviceCollection;
    }
}