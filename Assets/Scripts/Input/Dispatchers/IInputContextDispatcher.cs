using System;

public interface IInputContextDispatcher
{
    event EventHandler<InputContextEventArgs> InputContextEventHandler;
}
