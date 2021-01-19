using System;

public interface IMouseClickDispatcher
{
    event EventHandler<MouseClickEventArgs> MouseClickEventHandler;
}
