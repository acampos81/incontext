using System;

public class CreateObjectEventArgs : EventArgs
{
    public WorldObjectType objectType;

    public CreateObjectEventArgs(WorldObjectType objectType)
    {
        this.objectType = objectType;
    }
}
