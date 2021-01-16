using System;

public class HotKeyEventArgs : EventArgs
{
    public HotKey hotKey;
    public ButtonState state;

    public HotKeyEventArgs(HotKey hotKey, ButtonState state)
    {
        this.hotKey = hotKey;
        this.state = state;
    }
}
