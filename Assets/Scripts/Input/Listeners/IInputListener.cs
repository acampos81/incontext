using System;

public interface IInputListener
{
    event EventHandler<ListenerStateEventArgs> ListenerStateEventHandler;
}
