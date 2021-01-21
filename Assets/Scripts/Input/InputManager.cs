using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : InputListenerBase,
    IMouseButtonStateDispatcher,
    IScrollWheelDispatcher,
    IHotKeyStateDispatcher,
    IRemoveObjectDispatcher
{
    public event EventHandler<MouseButtonStateEventArgs> MouseButtonStateEventHandler;
    public event EventHandler<ScrollWheelEventArgs> ScrollWheelEventHandler;
    public event EventHandler<HotKeyEventArgs> HotKeyStateEventHandler;
    public event EventHandler RemoveObjectEventHandler;

    private List<int> _mouseButtons;
    private List<KeyCode> _hotKeys;

    private void Awake()
    {
        List<IInputListener> inputListeners = FindObjectsOfType<MonoBehaviour>(true).OfType<IInputListener>().ToList();

        foreach(IInputListener listener in inputListeners)
        {
            listener.ListenerStateEventHandler += HandleListenerState;
            RegisterListenerEvents(listener);
        }

        _hotKeys = new List<KeyCode>();
        _hotKeys.Add(KeyCode.Alpha1);
        _hotKeys.Add(KeyCode.Alpha2);
        _hotKeys.Add(KeyCode.LeftAlt);
        _hotKeys.Add(KeyCode.RightAlt);
        _hotKeys.Add(KeyCode.LeftControl);
        _hotKeys.Add(KeyCode.RightControl);

        _mouseButtons = new List<int>();
        _mouseButtons.Add(0);
        _mouseButtons.Add(1);
        _mouseButtons.Add(2);
    }

    void Update()
    {
        CheckMouseInput();
        CheckKeyboardInput();
    }

    void CheckMouseInput()
    {
        foreach (var button in _mouseButtons)
        {
            if (Input.GetMouseButtonDown(button))
            {
                DispatchMouseButtonEvent((MouseButton)button, ButtonState.DOWN);
            }

            if (Input.GetMouseButtonUp(button))
            {
                DispatchMouseButtonEvent((MouseButton)button, ButtonState.UP);
            }
        }

        if(Input.mouseScrollDelta.y != 0)
        {
            DispatchMouseScrollEvent(Input.mouseScrollDelta);
        }
    }

    void CheckKeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            if (RemoveObjectEventHandler != null)
                RemoveObjectEventHandler(this, EventArgs.Empty);
        }

        foreach (var keyCode in _hotKeys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                DispatchHotKeyEvent(GetAppKey(keyCode), ButtonState.DOWN);
            }

            if (Input.GetKeyUp(keyCode))
            {
                DispatchHotKeyEvent(GetAppKey(keyCode), ButtonState.UP);
            }
        }
    }

    HotKey GetAppKey(KeyCode keyCode)
    {
        switch(keyCode)
        {
            case KeyCode.Alpha1:
                return HotKey.KEY_1;
            case KeyCode.Alpha2:
                return HotKey.KEY_2;
            case KeyCode.LeftAlt:
            case KeyCode.RightAlt:
                return HotKey.ALT;
            case KeyCode.LeftControl:
            case KeyCode.RightControl:
                return HotKey.CTRL;
            default:
                return HotKey.UNKNOWN;
        }
    }

    public void RegisterListenerEvents(IInputListener listener)
    {
        if (listener is IMouseButtonStateListener mouseListener)
            MouseButtonStateEventHandler += mouseListener.HandleMouseButtonState;

        if (listener is IScrollWheelListener scrollListener)
            ScrollWheelEventHandler += scrollListener.HandleScrollWheel;

        if (listener is IHotKeyListener hotKeyListener)
            HotKeyStateEventHandler += hotKeyListener.HandleHotKeyState;
    }

    public void DeregisterListenerEvents(IInputListener listener)
    {
        if (listener is IMouseButtonStateListener mouseListener)
            MouseButtonStateEventHandler -= mouseListener.HandleMouseButtonState;

        if (listener is IScrollWheelListener scrollListener)
            ScrollWheelEventHandler -= scrollListener.HandleScrollWheel;

        if (listener is IHotKeyListener hotKeyListener)
            HotKeyStateEventHandler -= hotKeyListener.HandleHotKeyState;
    }

    void HandleListenerState(object sender, ListenerStateEventArgs args)
    {
        var listener = (IInputListener)sender;

        if (args.isActive)
        {
            RegisterListenerEvents(listener);
        } else
        {
            DeregisterListenerEvents(listener);
        }
    }

    void DispatchMouseButtonEvent(MouseButton button, ButtonState state)
    {
        MouseButtonStateEventHandler(this, new MouseButtonStateEventArgs(button, state));
    }

    void DispatchMouseScrollEvent(Vector2 scrollDelta)
    {
        ScrollWheelEventHandler(this, new ScrollWheelEventArgs(scrollDelta));
    }

    void DispatchHotKeyEvent(HotKey hotKey, ButtonState state)
    {
        HotKeyStateEventHandler(this, new HotKeyEventArgs(hotKey, state));
    }
}
