namespace MangoWidgets.Avalonia.Sample.Models;

public class MaterialBase : NotifyBase
{
	private int _x;
	public int X
	{
		get => _x;
		set => SetField(ref _x, value);
	}


	private int _y;
	public int Y
	{
		get => _y;
		set => SetField(ref _y, value);
	}
}
