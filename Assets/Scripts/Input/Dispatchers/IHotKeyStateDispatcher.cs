using System;

public interface IHotKeyStateDispatcher
{
    event EventHandler<HotKeyEventArgs> HotKeyStateEventHandler;
}
