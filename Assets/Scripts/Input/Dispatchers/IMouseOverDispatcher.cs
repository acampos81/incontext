using System;

public interface IMouseOverDispatcher
{
    event EventHandler<MouseOverEventArgs> MouseOverEventHandler;
}
