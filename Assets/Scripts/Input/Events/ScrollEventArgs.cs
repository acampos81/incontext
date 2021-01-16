using System;
using UnityEngine;

public class ScrollEventArgs : EventArgs
{
    public Vector2 scrollDelta;

    public ScrollEventArgs(Vector2 scrollDelta)
    {
        this.scrollDelta = scrollDelta;
    }
}
