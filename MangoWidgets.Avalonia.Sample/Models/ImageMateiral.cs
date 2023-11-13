namespace MangoWidgets.Avalonia.Sample.Models;

public class ImageMateiral : MaterialBase
{
	private int _width;
	public int Width
	{
		get => _width;
		set => SetField(ref _width,value);
	}

	private int _height;
	public int Height
	{
		get => _height;
		set => SetField(ref _height,value);
	}

	private double _ratio;
	public double Ratio
	{
		get => _ratio;
		set => SetField(ref _ratio,value);
	}

	private string? _source;
	public string? Source
	{
		get => _source;
		set => SetField(ref _source, value);
	}
}
