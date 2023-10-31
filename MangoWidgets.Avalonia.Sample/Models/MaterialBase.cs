namespace MangoWidgets.Avalonia.Sample.Models;

public class MaterialBase : NotifyBase
{
	private double _x;
	public double X
	{
		get => _x;
		set => SetField(ref _x, value);
	}


	private double _y;
	public double Y
	{
		get => _y;
		set => SetField(ref _y, value);
	}
}
