using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace MangoWidgets.Avalonia.Controls;

public class ImageElement : Control
{
	public IImage Source
	{
		get => GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}
	public static readonly StyledProperty<IImage> SourceProperty = AvaloniaProperty.Register<ImageElement, IImage>(nameof(Source));


	public double Left
	{
		get => GetValue(LeftProperty);
		set => SetValue(LeftProperty, value);
	}
	public static readonly StyledProperty<double> LeftProperty = AvaloniaProperty.Register<ImageElement, double>(nameof(Left));

	public double Top
	{
		get => GetValue(TopProperty);
		set => SetValue(TopProperty, value);
	}
	public static readonly StyledProperty<double> TopProperty = AvaloniaProperty.Register<ImageElement, double>(nameof(Top));

	static ImageElement()
	{
		LeftProperty.Changed.AddClassHandler<ImageElement>((s,e) => OnLeftChangeCallback(e));
		TopProperty.Changed.AddClassHandler<ImageElement>((s,e) => OnLeftChangeCallback(e));
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		if (this.Parent is not ContentPresenter contentPresenter) return;
		contentPresenter.SetValue(Canvas.LeftProperty, this.Left);
		contentPresenter.SetValue(Canvas.TopProperty, this.Top);
	}

	private static void OnLeftChangeCallback(AvaloniaPropertyChangedEventArgs e)
	{
		if (e.Sender is not ImageElement element) return;
		if (element.Parent is not ContentPresenter contentPresenter) return;
		contentPresenter.SetValue(Canvas.LeftProperty,element.Left);
		contentPresenter.SetValue(Canvas.TopProperty,element.Top);
	}

	public override void Render(DrawingContext context)
	{
		base.Render(context);
		context.DrawImage(Source,new Rect(0,0,this.Bounds.Width, this.Bounds.Height));
	}
}
