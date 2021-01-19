using System;

public class MouseClickEventArgs : EventArgs
{
    public MouseClickType clickType;

    public MouseClickEventArgs(MouseClickType clickType)
    {
        this.clickType = clickType;
    }
}