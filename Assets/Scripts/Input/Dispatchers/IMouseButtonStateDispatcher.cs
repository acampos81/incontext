using System;

public interface IMouseButtonStateDispatcher
{
    event EventHandler<MouseButtonStateEventArgs> MouseButtonStateEventHandler;
}
