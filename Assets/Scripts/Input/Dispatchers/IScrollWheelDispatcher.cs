using System;

public interface IScrollWheelDispatcher
{
    event EventHandler<ScrollWheelEventArgs> ScrollWheelEventHandler;
}
