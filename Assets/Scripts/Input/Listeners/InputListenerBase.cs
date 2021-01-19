using System;
using UnityEngine;

public abstract class InputListenerBase : MonoBehaviour, IInputListener
{
    public event EventHandler<ListenerStateEventArgs> ListenerStateEventHandler;

    void OnEnable()
    {
        if(ListenerStateEventHandler != null)
            ListenerStateEventHandler(this, new ListenerStateEventArgs(true));
    }

    void OnDisable()
    {
        if(ListenerStateEventHandler != null)
            ListenerStateEventHandler(this, new ListenerStateEventArgs(false));
    }
}
