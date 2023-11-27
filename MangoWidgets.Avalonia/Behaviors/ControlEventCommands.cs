using Avalonia.Controls;

namespace MangoWidgets.Avalonia.Behaviors;

public class SizeChangedEventCommand : NativeEvent2CommandBehavior<Control>
{
    protected override void RegistEvent(Control obj) => obj.SizeChanged += ProxyMethod;
    protected override void UnRegistEvent(Control obj) => obj.SizeChanged -= ProxyMethod;
}

