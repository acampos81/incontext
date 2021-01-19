using System;

public class MouseOverEventArgs : EventArgs
{
    public MouseOverState state;

    public MouseOverEventArgs(MouseOverState state)
    {
        this.state = state;
    }
}
