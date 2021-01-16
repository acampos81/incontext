using System;

public class InputContextEventArgs : EventArgs
{
    public InputContext context;

    public InputContextEventArgs(InputContext context)
    {
        this.context = context;
    }
}
