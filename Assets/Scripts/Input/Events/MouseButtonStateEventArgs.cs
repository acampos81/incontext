using System;

public class MouseButtonStateEventArgs : EventArgs
{
    public MouseButton button;
    public ButtonState state;

    public MouseButtonStateEventArgs(MouseButton button, ButtonState state)
    {
        this.button = button;
        this.state = state;
    }
}
