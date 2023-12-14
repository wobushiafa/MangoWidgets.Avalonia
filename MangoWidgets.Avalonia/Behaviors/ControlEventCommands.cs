using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace MangoWidgets.Avalonia.Behaviors;

public class SizeChangedEventCommand : NativeEvent2CommandBehavior<Control>
{
    protected override void RegistEvent(Control obj) => obj.SizeChanged += ProxyMethod;
    protected override void UnRegistEvent(Control obj) => obj.SizeChanged -= ProxyMethod;
}

public class SelectionChangedEventCommand : NativeEvent2CommandBehavior<SelectingItemsControl>
{
    protected override void RegistEvent(SelectingItemsControl obj) => obj.SelectionChanged += ProxyMethod;
    protected override void UnRegistEvent(SelectingItemsControl obj) => obj.SelectionChanged -= ProxyMethod;
}

