using System;

public class ListenerStateEventArgs : EventArgs
{
    public bool isActive;

    public ListenerStateEventArgs(bool isActive)
    {
        this.isActive = isActive;
    }
}