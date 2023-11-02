using Avalonia;

namespace MangoWidgets.Avalonia;

public class BindingProxy<T> : AvaloniaObject where T : notnull
{
    public T Data { get; set; }

    public static readonly StyledProperty<T> DataProperty = AvaloniaProperty.Register<BindingProxy<T>, T>(nameof(Data),default(T));
}
