using System;

public interface IIOProcessDispatcher
{
    event EventHandler<IOProcessEventArgs> IOProcessEventHandler;
}
