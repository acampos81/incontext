using System;

public class UIStateUpdateEventArgs : EventArgs
{
    public UIState state;

    public UIStateUpdateEventArgs(UIState state)
    {
        this.state = state;
    }
}
