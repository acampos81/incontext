using System;

public class WorldObjectSelectedEventArgs : EventArgs
{
    public IWorldObjectModel objectModel;
    public MouseClickType clickType;

    public WorldObjectSelectedEventArgs(IWorldObjectModel objectModel, MouseClickType clickType)
    {
        this.objectModel = objectModel;
        this.clickType = clickType;
    }
}
