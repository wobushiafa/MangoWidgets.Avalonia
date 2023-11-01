using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using System.Xml.Linq;

namespace MangoWidgets.Avalonia.Controls;

public class ResizeThumb : Thumb
{
	protected override Type StyleKeyOverride => typeof(Thumb);
	public DragDirection DragDirection { get; set; }

	public Control? TargetElement
	{
		get => GetValue(TargetElementProperty);
		set => SetValue(TargetElementProperty,value);
	}
	public static readonly StyledProperty<Control?> TargetElementProperty = AvaloniaProperty.Register<ResizeThumb, Control?>(nameof(TargetElement));

	public double Ratio
	{
		get => GetValue(RatioProperty);
		set => SetValue(RatioProperty, value);
	}
	public static readonly StyledProperty<double> RatioProperty = AvaloniaProperty.Register<ResizeThumb, double>(nameof(Ratio),1);

	private const double MINIMAL_SIZE = 20;
	protected override void OnDragDelta(VectorEventArgs e)
	{
		base.OnDragDelta(e);
		if (TargetElement is not { } target) return;
		if (target.Parent is not ContentPresenter parent) return;
		var originLeft = Canvas.GetLeft(parent);
		var originTop = Canvas.GetTop(parent);

		if (Ratio != 1 && !double.IsNaN(Ratio))
		{
			double deltaVertical = 0;
			double deltaHorizontal = 0;
			double height, width;
			switch (DragDirection)
			{
				case DragDirection.TopLeft:
					if (Math.Abs(e.Vector.X) <= Math.Abs(e.Vector.Y))
					{
						deltaHorizontal = Math.Min(e.Vector.X, target.Width - target.MinWidth);
						width = target.Width - deltaHorizontal;
						height = width / Ratio;
						Canvas.SetLeft(parent, originLeft + deltaHorizontal);
						Canvas.SetTop(parent, originTop + target.Height - height);
						target.Width = width;
						target.Height = height;
					}
					else
					{
						deltaVertical = Math.Min(e.Vector.Y, target.Height - target.MinHeight);

						height = target.Height - deltaVertical;
						width = height * Ratio;
						Canvas.SetTop(parent, originTop + deltaVertical);
						Canvas.SetLeft(parent, originLeft + target.Width - width);
						target.Height = height;
						target.Width = width;
					}
					break;

				case DragDirection.TopCenter:
					deltaVertical = Math.Min(e.Vector.Y, target.Height - target.MinHeight);

					height = target.Height - deltaVertical;
					width = height * Ratio;
					Canvas.SetTop(parent, originLeft + deltaVertical);
					Canvas.SetLeft(parent, originTop + target.Width - width);
					target.Height = height;
					target.Width = width;
					break;

				case DragDirection.TopRight:
					if (Math.Abs(e.Vector.X) <= Math.Abs(e.Vector.Y))
					{
						deltaHorizontal = Math.Min(-e.Vector.X, target.Width - target.MinWidth);
						width = target.Width - deltaHorizontal;
						height = width / Ratio;
						Canvas.SetTop(parent, originTop + target.Height - height);
						target.Width = width;
						target.Height = height;
					}
					else
					{
						deltaVertical = Math.Min(e.Vector.Y, target.Height - target.MinHeight);

						height = target.Height - deltaVertical;
						width = height * Ratio;

						Canvas.SetTop(parent, originTop + deltaVertical);
						target.Height = height;
						target.Width = width;
					}
					break;

				case DragDirection.MiddleLeft:
					deltaHorizontal = Math.Min(e.Vector.X, target.Width - target.MinWidth);
					width = target.Width - deltaHorizontal;
					height = (double)width / Ratio;
					Canvas.SetLeft(parent, originLeft + deltaHorizontal);
					target.Width = width;
					target.Height = height;
					break;

				case DragDirection.MiddleCenter:
					break;

				case DragDirection.MiddleRight:
					deltaHorizontal = Math.Min(-e.Vector.X, target.Width - target.MinWidth);
					width = target.Width - deltaHorizontal;
					height = width / Ratio;
					target.Width = width;
					target.Height = height;
					break;

				case DragDirection.BottomLeft:
					if (Math.Abs(e.Vector.X) <= Math.Abs(e.Vector.Y))
					{
						deltaHorizontal = Math.Min(e.Vector.X, target.Width - target.MinWidth);
						width = target.Width - deltaHorizontal;
						height = width / Ratio;
						Canvas.SetLeft(parent, originLeft + deltaHorizontal);
						target.Width = width;
						target.Height = height;
					}
					else
					{
						deltaVertical = Math.Min(-e.Vector.Y, target.Height - target.MinHeight);

						height = target.Height - deltaVertical;
						width = height * Ratio;
						Canvas.SetLeft(parent, originLeft + target.Width - width);
						target.Height = height;
						target.Width = width;
					}
					break;

				case DragDirection.BottomCenter:
					deltaVertical = Math.Min(-e.Vector.Y, target.Height - target.MinHeight);

					height = target.Height - deltaVertical;
					width = height * Ratio;
					target.Height = height;
					target.Width = width;
					break;

				case DragDirection.BottomRight:
					if (Math.Abs(e.Vector.X) <= Math.Abs(e.Vector.Y))
					{
						deltaHorizontal = Math.Min(-e.Vector.X, target.Width - target.MinWidth);
						width = target.Width - deltaHorizontal;
						height = (double)width / Ratio;
						target.Width = width;
						target.Height = height;
					}
					else
					{
						deltaVertical = Math.Min(-e.Vector.Y, target.Height - target.MinHeight);

						height = target.Height - deltaVertical;
						width = height * Ratio;
						target.Height = height;
						target.Width = width;
					}
					break;

				default:
					break;
			}
		}
		else
		{
			switch (this.VerticalAlignment)
			{
				case VerticalAlignment.Bottom:
					if (target.Height + e.Vector.Y <= MINIMAL_SIZE) break;
					target.Height += e.Vector.Y;
					break;
				case VerticalAlignment.Top:
					if (target.Height - e.Vector.Y > MINIMAL_SIZE)
					{
						target.Height -= e.Vector.Y;
						Canvas.SetTop(parent, originTop + e.Vector.Y);
					}
					break;
			}

			switch (this.HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
					if (target.Width - e.Vector.X > MINIMAL_SIZE)
					{
						target.Width -= e.Vector.X;
						Canvas.SetLeft(parent, originLeft + e.Vector.X);
					}

					break;
				case HorizontalAlignment.Right:
					if (target.Width + e.Vector.X > MINIMAL_SIZE)
						target.Width += e.Vector.X;
					break;
			}
		}
	}
}

public enum DragDirection
{
	TopLeft = 1,
	TopCenter = 2,
	TopRight = 4,
	MiddleLeft = 16,
	MiddleCenter = 32,
	MiddleRight = 64,
	BottomLeft = 256,
	BottomCenter = 512,
	BottomRight = 1024,
}
