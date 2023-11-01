using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MangoWidgets.Avalonia.AttachtedProperties;

namespace MangoWidgets.Avalonia.Controls;

public class ImageElement : ContentControl
{
	public IImage Source
	{
		get => GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}
	public static readonly StyledProperty<IImage> SourceProperty = AvaloniaProperty.Register<ImageElement, IImage>(nameof(Source));

	public double Ratio
	{
		get => GetValue(RatioProperty);
		set => SetValue(RatioProperty, value);
	}
	public static readonly StyledProperty<double> RatioProperty = AvaloniaProperty.Register<ImageElement, double>(nameof(Ratio));

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		if (this.Parent is not Control control) return;
		control.ZIndex = -1;
	}

	protected override void OnPointerPressed(PointerPressedEventArgs e)
	{
		base.OnPointerPressed(e);
		if(e.ClickCount >= 2)
			DraggedElement.SetCanDragged(this, true);
	}

	public override void Render(DrawingContext context)
	{
		base.Render(context);
		context.DrawImage(Source,new Rect(0,0,this.Bounds.Width, this.Bounds.Height));
	}
}
