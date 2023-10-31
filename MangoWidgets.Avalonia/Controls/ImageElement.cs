using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using MangoWidgets.Avalonia.AttachtedProperties;

namespace MangoWidgets.Avalonia.Controls;

public class ImageElement : Control
{
	public IImage Source
	{
		get => GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}
	public static readonly StyledProperty<IImage> SourceProperty = AvaloniaProperty.Register<ImageElement, IImage>(nameof(Source));

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
