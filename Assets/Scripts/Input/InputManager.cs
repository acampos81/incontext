using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private List<IInputTypeListener> _inputTypeListeners;
    private List<IMouseListener> _mouseListeners;
    private List<IKeyListener> _keyListeners;
    private List<int> _mouseButtons;
    private List<KeyCode> _appKeys;

    private void Awake()
    {
        List<MonoBehaviour> sceneBehaviours = FindObjectsOfType<MonoBehaviour>().ToList();

        _inputTypeListeners = new List<IInputTypeListener>();
        _mouseListeners = new List<IMouseListener>();
        _keyListeners = new List<IKeyListener>();

        foreach (var behaviour in sceneBehaviours)
        {
            if (behaviour is IInputTypeListener)
                _inputTypeListeners.Add(behaviour as IInputTypeListener);

            if (behaviour is IMouseListener)
                _mouseListeners.Add(behaviour as IMouseListener);

            if (behaviour is IKeyListener)
                _keyListeners.Add(behaviour as IKeyListener);
        }

        _appKeys = new List<KeyCode>();
        _appKeys.Add(KeyCode.LeftAlt);
        _appKeys.Add(KeyCode.RightAlt);
        _appKeys.Add(KeyCode.LeftControl);
        _appKeys.Add(KeyCode.RightControl);

        _mouseButtons = new List<int>();
        _mouseButtons.Add(0);
        _mouseButtons.Add(1);
        _mouseButtons.Add(2);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _inputTypeListeners.Count; i++)
        {
            _inputTypeListeners[i].HandleInputType(InputType.WORLD);
        }
    }

    // Update is called once per frame
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
                foreach (var mouseListener in _mouseListeners)
                {
                    mouseListener.HandleMouseButtonDown((MouseButton)button);
                }
            }

            if (Input.GetMouseButtonUp(button))
            {
                foreach (var mouseListener in _mouseListeners)
                {
                    mouseListener.HandleMouseButtonUp((MouseButton)button);
                }
            }
        }

        if(Input.mouseScrollDelta.y != 0)
        {
            foreach (var mouseListener in _mouseListeners)
            {
                mouseListener.HandleScrolling(Input.mouseScrollDelta);
            }
        }
    }

    void CheckKeyboardInput()
    {
        foreach (var keyCode in _appKeys)
        {
            if (Input.GetKeyDown(keyCode))
            {
                foreach (var keyListener in _keyListeners)
                {
                    keyListener.HandleKeyDown(GetAppKey(keyCode));
                }
            }

            if (Input.GetKeyUp(keyCode))
            {
                foreach (var keyListener in _keyListeners)
                {
                    keyListener.HandleKeyUp(GetAppKey(keyCode));
                }
            }
        }
    }

    AppKey GetAppKey(KeyCode keyCode)
    {
        switch(keyCode)
        {
            case KeyCode.LeftAlt:
            case KeyCode.RightAlt:
                return AppKey.ALT;
            case KeyCode.LeftControl:
            case KeyCode.RightControl:
                return AppKey.CTRL;
            default:
                return AppKey.UNKNOWN;
        }
    }

}
