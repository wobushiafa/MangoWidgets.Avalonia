using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using MangoWidgets.Avalonia.AttachtedProperties;

namespace MangoWidgets.Avalonia.Controls;

public class ItemCanDraggedCanvas : Canvas
{
	private ContentPresenter? _draggedElement;
	private bool _isDragging;
	private Point _originPos;
	private double _origHorizOffset, _origVertOffset;
	private bool _modifyLeftOffset, _modifyTopOffset;

	public bool AllowDragOutOfView
	{
		get => GetValue(AllowDragOutOfViewProperty);
		set => SetValue(AllowDragOutOfViewProperty, value);
	}
	public static readonly StyledProperty<bool> AllowDragOutOfViewProperty = AvaloniaProperty.Register<ItemCanDraggedCanvas, bool>(nameof(AllowDragOutOfView),false);

	protected override void OnPointerPressed(PointerPressedEventArgs e)
	{
		_isDragging = false;
		base.OnPointerPressed(e);
		foreach (ContentPresenter item in Children.Where(x => x != (e.Source as Control)?.Parent))
		{
			DraggedElement.SetCanDragged(item.Child!,false);
		}
		
		if (e.Source == this) return;
		var element = GetContentPresenter(e.Source);
		if (element is null) return;
		_draggedElement = element;
		if (!DraggedElement.GetCanDragged(_draggedElement.Child!)) return;

		_isDragging = true;
		_originPos = e.GetPosition(this);
		var left = GetLeft(_draggedElement);
		var right = GetRight(_draggedElement);
		var top = GetTop(_draggedElement);
		var bottom = GetBottom(_draggedElement);

		_origHorizOffset = ResolveOffset(left, right, out _modifyLeftOffset);
		_origVertOffset = ResolveOffset(top, bottom, out _modifyTopOffset);
		e.Handled = true;
	}

	protected override void OnPointerMoved(PointerEventArgs e)
	{
		base.OnPointerMoved(e);
		if (_draggedElement == null || !_isDragging)
			return;

		var cursorLocation = e.GetPosition(this);

		// Determine the horizontal offset.
		var newHorizontalOffset = _modifyLeftOffset
			? _origHorizOffset + (cursorLocation.X - _originPos.X)
			: _origHorizOffset - (cursorLocation.X - _originPos.X);

		// Determine the vertical offset.
		var newVerticalOffset = _modifyTopOffset
			? _origVertOffset + (cursorLocation.Y - _originPos.Y)
			: _origVertOffset - (cursorLocation.Y - _originPos.Y);

		if (!AllowDragOutOfView)
		{
			var elemRect = CalculateDragElementRect(newHorizontalOffset, newVerticalOffset);
			#region Verify Drag Element Location
			// Get the bounding rect of the drag element.
			//
			// If the element is being dragged out of the viewable area,
			// determine the ideal rect location, so that the element is
			// within the edge(s) of the canvas.
			//
			var leftAlign = elemRect.Left < 0;
			var rightAlign = elemRect.Right > Bounds.Width;

			if (leftAlign)
			{
				newHorizontalOffset = _modifyLeftOffset ? 0 : Bounds.Width - elemRect.Width;
			}
			else if (rightAlign)
			{
				newHorizontalOffset = _modifyLeftOffset ? Bounds.Width - elemRect.Width : 0;
			}

			var topAlign = elemRect.Top < 0;
			var bottomAlign = elemRect.Bottom > Bounds.Height;

			if (topAlign)
			{
				newVerticalOffset = _modifyTopOffset ? 0 : Bounds.Height - elemRect.Height;
			}
			else if (bottomAlign)
			{
				newVerticalOffset = _modifyTopOffset ? Bounds.Height - elemRect.Height : 0;
			}

			#endregion Verify Drag Element Location
		}

		if (_modifyLeftOffset)
			SetLeft(_draggedElement, newHorizontalOffset);
		else
			SetRight(_draggedElement, newHorizontalOffset);

		if (_modifyTopOffset)
			SetTop(_draggedElement, newVerticalOffset);
		else
			SetBottom(_draggedElement, newVerticalOffset);

	}

	protected override void OnPointerReleased(PointerReleasedEventArgs e)
	{
		base.OnPointerReleased(e);
		_draggedElement = null;
		_isDragging = false;
	}

	private Rect CalculateDragElementRect(double newHorizOffset, double newVertOffset)
	{
		if (_draggedElement == null)
			throw new InvalidOperationException("draggedElement is null.");
		var elemSize = _draggedElement.Bounds;
		double x, y;
		x = _modifyLeftOffset ? newHorizOffset : elemSize.Width - newHorizOffset - elemSize.Width;
		y = _modifyTopOffset ? newVertOffset : elemSize.Height - newVertOffset - elemSize.Height;
		var elemLoc = new Point(x, y);
		return new Rect(elemLoc, elemSize.Size);
	}

	private static ContentPresenter? GetContentPresenter(object? o)
	{
		while(o != null)
		{
			if (o is ContentPresenter content) 
				return content;
			o = (o as Control)?.Parent;
		}
		return null;
	}

	private static double ResolveOffset(double side1, double side2, out bool useSide1)
	{
		// If the Canvas.Left and Canvas.Right attached properties
		// are specified for an element, the 'Left' value is honored.
		// The 'Top' value is honored if both Canvas.Top and
		// Canvas.Bottom are set on the same element.  If one
		// of those attached properties is not set on an element,
		// the default value is Double.NaN.
		useSide1 = true;
		double result;
		if (double.IsNaN(side1))
		{
			if (double.IsNaN(side2))
			{
				// Both sides have no value, so set the
				// first side to a value of zero.
				result = 0;
			}
			else
			{
				result = side2;
				useSide1 = false;
			}
		}
		else
		{
			result = side1;
		}
		return result;
	}

}
