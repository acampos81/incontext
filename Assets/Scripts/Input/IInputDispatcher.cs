using System;

public interface IInputDispatcher
{
    event EventHandler<InputContextEventArgs> InputContextEventHandler;
    event EventHandler<MouseButtonEventArgs> MouseButtonEventHandler;
    event EventHandler<ScrollEventArgs> ScrollEventHandler;
    event EventHandler<HotKeyEventArgs> HotKeyEventHandler;
}
