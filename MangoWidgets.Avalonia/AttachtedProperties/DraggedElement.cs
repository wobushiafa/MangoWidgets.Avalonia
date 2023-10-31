using Avalonia;

namespace MangoWidgets.Avalonia.AttachtedProperties;

public class DraggedElement : AvaloniaObject
{
	public static bool GetCanDragged(AvaloniaObject obj) => obj.GetValue(CanDraggedProperty);
	public static void SetCanDragged(AvaloniaObject obj,bool value) => obj.SetValue(CanDraggedProperty, value);

	public static readonly AttachedProperty<bool> CanDraggedProperty = AvaloniaProperty.RegisterAttached<DraggedElement, bool>("CanDragged", typeof(DraggedElement), false);
}
