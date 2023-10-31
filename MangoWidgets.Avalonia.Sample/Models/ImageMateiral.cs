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
}
