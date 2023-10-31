using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MangoWidgets.Avalonia.Sample.ViewModels;
using ReactiveUI;

namespace MangoWidgets.Avalonia.Sample.Views;

public partial class DrawingBoardView : ReactiveUserControl<DrawingBoardViewModel>
{
    private readonly ItemsControl _materialHost;
    
    public DrawingBoardView()
    {
        InitializeComponent();
        _materialHost = this.Find<ItemsControl>("MaterialHost")!;

        this.WhenActivated(disposables =>
        {
            Observable.FromEventPattern<EventHandler<PointerPressedEventArgs>,PointerPressedEventArgs>(handler => handler.Invoke,
                    handler => _materialHost.PointerPressed += handler,
                    handler => _materialHost.PointerPressed -= handler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Where(e => e.EventArgs.GetCurrentPoint(_materialHost).Properties.IsLeftButtonPressed)
                .Select(e => e.EventArgs.GetPosition(_materialHost))
                .InvokeCommand(this,x => x.ViewModel!.PointerPressedCommand)
                .DisposeWith(disposables);
            
            Observable.FromEventPattern<EventHandler<PointerEventArgs>,PointerEventArgs>(handler => handler.Invoke,
                    handler => _materialHost.PointerMoved += handler,
                    handler => _materialHost.PointerMoved -= handler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(e => e.EventArgs.GetPosition(_materialHost))
                .InvokeCommand(this,x => x.ViewModel!.PointerMovedCommand)
                .DisposeWith(disposables);
            
            Observable.FromEventPattern<EventHandler<PointerReleasedEventArgs>,PointerReleasedEventArgs>(handler => handler.Invoke,
                    handler => _materialHost.PointerReleased += handler,
                    handler => _materialHost.PointerReleased -= handler)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(e => Unit.Default)
                .InvokeCommand(this,x => x.ViewModel!.PointerReleasedCommand)
                .DisposeWith(disposables);
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}