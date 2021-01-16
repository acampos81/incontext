using System;

public class MouseButtonEventArgs : EventArgs
{
    public MouseButton button;
    public ButtonState state;

    public MouseButtonEventArgs(MouseButton button, ButtonState state)
    {
        this.button = button;
        this.state = state;
    }
}
