using System;

public interface IUIStateDispatcher
{
    event EventHandler<UIStateUpdateEventArgs> UIStateUpdateEventHandler;
}
