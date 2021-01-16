using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour, IInputDispatcher
{
    private EventHandler<InputContextEventArgs> _inputContextEvent;
    public event EventHandler<InputContextEventArgs> InputContextEventHandler
    {
        add
        {
            if (!IsDuplicate(_inputContextEvent, value))
            {
                _inputContextEvent += value;
            }
        }
        remove { _inputContextEvent -= value; }
    }

    private EventHandler<MouseButtonEventArgs> _mouseButtonEvent;
    public event EventHandler<MouseButtonEventArgs> MouseButtonEventHandler
    {
        add {
            if(!IsDuplicate(_mouseButtonEvent, value))
            {
                _mouseButtonEvent += value;
            }
        }
        remove { _mouseButtonEvent -= value; }
    }

    private EventHandler<ScrollEventArgs> _scrollEvent;
    public event EventHandler<ScrollEventArgs> ScrollEventHandler
    {
        add
        {
            if (!IsDuplicate(_scrollEvent, value))
            {
                _scrollEvent += value;
            }
        }
        remove { _scrollEvent -= value; }
    }

    private EventHandler<HotKeyEventArgs> _hotKeyEvent;
    public event EventHandler<HotKeyEventArgs> HotKeyEventHandler
    {
        add
        {
            if (!IsDuplicate(_hotKeyEvent, value))
            {
                _hotKeyEvent += value;
            }
        }
        remove { _hotKeyEvent -= value; }
    }

    private bool IsDuplicate<T>(EventHandler<T> handler, EventHandler<T> val)
    {
        if(handler == null)
        {
            return false;
        }else
        {
            return handler.GetInvocationList().Contains(val);
        }
    }

    private List<int> _mouseButtons;
    private List<KeyCode> _hotKeys;

    private void Awake()
    {
        List<IInputListener> inputListeners = FindObjectsOfType<MonoBehaviour>().OfType<IInputListener>().ToList();

        foreach(IInputListener listener in inputListeners)
        {
            listener.RegisterDispatcher(this);
        }

        _hotKeys = new List<KeyCode>();
        _hotKeys.Add(KeyCode.LeftAlt);
        _hotKeys.Add(KeyCode.RightAlt);
        _hotKeys.Add(KeyCode.LeftControl);
        _hotKeys.Add(KeyCode.RightControl);

        _mouseButtons = new List<int>();
        _mouseButtons.Add(0);
        _mouseButtons.Add(1);
        _mouseButtons.Add(2);
    }

    void Start()
    {
        DispatchInputContextEvent(InputContext.WORLD);
    }

    void Update()
    {
        //CheckInputType
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

    void DispatchInputContextEvent(InputContext context)
    {
        _inputContextEvent(this, new InputContextEventArgs(context));
    }

    void DispatchMouseButtonEvent(MouseButton button, ButtonState state)
    {
        _mouseButtonEvent(this, new MouseButtonEventArgs(button, state));
    }

    void DispatchMouseScrollEvent(Vector2 scrollDelta)
    {
        _scrollEvent(this, new ScrollEventArgs(scrollDelta));
    }

    void DispatchHotKeyEvent(HotKey hotKey, ButtonState state)
    {
        _hotKeyEvent(this, new HotKeyEventArgs(hotKey, state));
    }
}
