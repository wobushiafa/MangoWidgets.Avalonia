using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using MangoWidgets.Avalonia.Sample.Contracts;
using MangoWidgets.Avalonia.Sample.Models;
using MangoWidgets.Core;
using ReactiveUI;

namespace MangoWidgets.Avalonia.Sample.ViewModels;

public class DrawingBoardViewModel : ViewModelBase ,IMainModule
{
    public string? DisplayName { get; set; }
    public int Index { get; set; }
    public ObservableCollection<MaterialBase> Materials { get; set; } = new();

    private NoteMaterial? _noteMaterial;
    
    public ReactiveCommand<Point,Unit> PointerPressedCommand { get; set; }
    public ReactiveCommand<Point,Unit> PointerMovedCommand { get; set; }
    public ReactiveCommand<Unit,Unit> PointerReleasedCommand { get; set; }
    public ReactiveCommand<Unit,Unit> ClearCommand { get; set; }

    public DrawingBoardViewModel()
    {
        DisplayName = "画板";
        Index = 0;
        
        PointerPressedCommand = ReactiveCommand.Create<Point>(OnPointerPressed);
        PointerMovedCommand = ReactiveCommand.Create<Point>(OnPointerMoved);
        PointerReleasedCommand = ReactiveCommand.Create(OnPointerReleased);
        ClearCommand = ReactiveCommand.Create(Clear);
        Materials.Add(new ImageMateiral() { X = 400,Y=800,Width=500,Height=500});
	}
    
    
    private void OnPointerPressed(Point point)
    {
        _noteMaterial = new NoteMaterial();
        _noteMaterial.Dots.Add(new Dot((int)point.X,(int)point.Y));
        Materials.Add(_noteMaterial);
    }
    
    private void OnPointerMoved(Point point) => _noteMaterial?.Dots.Add(new Dot((int)point.X,(int)point.Y));

    private void OnPointerReleased() => _noteMaterial = null;

    private void Clear() => Materials.Clear();
}