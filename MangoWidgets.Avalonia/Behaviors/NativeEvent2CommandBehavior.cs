using System.Windows.Input;
using Avalonia;
using Avalonia.Xaml.Interactivity;

namespace MangoWidgets.Avalonia.Behaviors;

public abstract class NativeEvent2CommandBehavior<T> : Behavior<T> where T :AvaloniaObject 
{
     #region StyledProperty
     public static readonly StyledProperty<T?> SourceObjectProperty =
          AvaloniaProperty.Register<NativeEvent2CommandBehavior<T>, T?>(nameof(SourceObject));
     
     public static readonly StyledProperty<ICommand?> CommandProperty =
          AvaloniaProperty.Register<NativeEvent2CommandBehavior<T>, ICommand?>(nameof(Command));
    
     public static readonly StyledProperty<object?> CommandParameterProperty =
          AvaloniaProperty.Register<NativeEvent2CommandBehavior<T>, object?>(nameof(CommandParameter));

     public static readonly StyledProperty<bool> PassEventArgsToCommandProperty =
          AvaloniaProperty.Register<NativeEvent2CommandBehavior<T>, bool>(nameof(PassEventArgsToCommand));
     #endregion
     
     #region Properties

     public T? SourceObject
     {
          get => GetValue(SourceObjectProperty);
          set => SetValue(SourceObjectProperty, value);
     }
    
     public ICommand? Command
     {
          get => GetValue(CommandProperty);
          set => SetValue(CommandProperty, value);
     }
     public object? CommandParameter
     {
          get => GetValue(CommandParameterProperty);
          set => SetValue(CommandParameterProperty, value);
     }
    
     public bool PassEventArgsToCommand
     {
          get => GetValue(PassEventArgsToCommandProperty);
          set => SetValue(PassEventArgsToCommandProperty, value);
     }

     #endregion
     
     //当前订阅事件的那个对象(AssociatedObject or SourceObject)
     private T? _resolvedSource;
     
     protected NativeEvent2CommandBehavior()
     {
          SourceObjectProperty.Changed.AddClassHandler<NativeEvent2CommandBehavior<T>>(OnSourceObjectChanged);
     }
    
     private static void OnSourceObjectChanged(NativeEvent2CommandBehavior<T> sender,AvaloniaPropertyChangedEventArgs e)
     {
          sender.SetResolvedSource(sender.ComputeResolvedSource());
     }
     
     #region Override Methods

     protected override void OnAttached()
     {
          SetResolvedSource(ComputeResolvedSource());
     }
    
     protected override void OnDetaching()
     {
          SetResolvedSource(null);
     }
     #endregion
     
     #region Private Methods
     
     /// <summary>
     /// 获取当前要订阅事件的对象,默认是SourceObject,为空则是AssociatedObject
     /// </summary>
     /// <returns></returns>
     private T? ComputeResolvedSource()
     {
          return GetValue(SourceObjectProperty) ?? AssociatedObject;
     }
     
     /// <summary>
     /// 取消当前SourceObject的事件订阅,添加新的SourceObject事件订阅
     /// </summary>
     /// <param name="newSource"></param>
     private void SetResolvedSource(T? newSource)
     {
          //排除当前订阅事件的sourceObject与新变更的sourceObject为同一对象的情况
          if (Equals(_resolvedSource, newSource)) 
               return;
        
          if (_resolvedSource is not null)
               UnRegistEvent(_resolvedSource);

          _resolvedSource = newSource;
        
          //新的sourceObject可能是null
          if(_resolvedSource is not null)
               RegistEvent(_resolvedSource);
     }
     
     #endregion
     
     #region Virtual Methods
     protected virtual void RegistEvent(T obj)
     {

     }

     protected virtual void UnRegistEvent(T obj)
     {
        
     }

     protected virtual void ProxyMethod(object? sender, EventArgs e)
     {
          Command?.Execute(PassEventArgsToCommand ? e : CommandParameter);
     }
     
     #endregion
}