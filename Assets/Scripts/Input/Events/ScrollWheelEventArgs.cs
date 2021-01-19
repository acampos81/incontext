using System;
using UnityEngine;

public class ScrollWheelEventArgs : EventArgs
{
    public Vector2 scrollDelta;

    public ScrollWheelEventArgs(Vector2 scrollDelta)
    {
        this.scrollDelta = scrollDelta;
    }
}
