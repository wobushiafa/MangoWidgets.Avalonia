using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MangoWidgets.Avalonia.Sample.ViewModels;
using MangoWidgets.Avalonia.Sample.Views;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;

namespace MangoWidgets.Avalonia.Sample;

public partial class App : Application
{
    public new static App Current => (App)Application.Current!;
    public IServiceProvider? ServiceProvider { get; private set; }
    public static T Resolve<T>() where T : notnull => Current.ServiceProvider!.GetRequiredService<T>();
    public static object Resolve(Type type) => Current.ServiceProvider!.GetRequiredService(type);
    
    public App()
    {
        Locator.CurrentMutable.InitializeReactiveUI();
        Locator.CurrentMutable.InitializeSplat();
        Locator.CurrentMutable.RegisterLazySingleton(() => new ViewLocator(), typeof(IViewLocator));
        //https://github.com/AvaloniaUI/Avalonia/issues/5144#issuecomment-763160616
        Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
        Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
        var services = new ServiceCollection();
        services.ConfigureServices();
        ServiceProvider = services.BuildServiceProvider();
    }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var shellViewModel = Resolve<ShellViewModel>();
        var shellView = Resolve<IViewFor<ShellViewModel>>();
        shellView.ViewModel = shellViewModel;
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = shellView as Window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}