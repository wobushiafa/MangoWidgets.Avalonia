using System.Collections.Generic;
using System.Collections.ObjectModel;
using MangoWidgets.Core;

namespace MangoWidgets.Avalonia.Sample.Models;

public class NoteMaterial : NotifyBase
{
    private double _thickness = 2d;
    public double Thickness 
    { 
        get => _thickness;
        set => SetField(ref _thickness,value);
    }
    
    private string _color = "#000000";
    public string Color 
    { 
        get => _color;
        set => SetField(ref _color,value);
    }

    private ICollection<Dot> _dots = new ObservableCollection<Dot>();
    public ICollection<Dot> Dots
    {
        get => _dots;
        set => SetField(ref _dots, value);
    }
}