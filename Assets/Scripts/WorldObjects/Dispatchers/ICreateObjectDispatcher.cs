using System;

public interface ICreateObjectDispatcher
{
    event EventHandler<CreateObjectEventArgs> CreateObjectEventHandler;
}
