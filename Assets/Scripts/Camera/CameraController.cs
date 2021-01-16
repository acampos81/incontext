using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : InputListenerBase, IInputListener
{
    private Dictionary<CameraState, ITransformControl> _transformControls;
    private CameraState _currentState;
    private ITransformControl _activeControl;
    private ScrollZoomControl _scrollZoomControl;
    private OrbitControl _orbitControl;
    private MouseButton _activeMouseButton;
    private bool _altDown;

    private void Awake()
    {
        

        // Scrollwheel zooming is stateless.  It's handled
        // whenever a delta greater than 0 is detected.
        _scrollZoomControl = new ScrollZoomControl();
        _scrollZoomControl.Init(transform);

        // Need to maintain a refernce to _orbitControl since other states
        // can affect its focus target.
        _orbitControl = new OrbitControl();

        _transformControls = new Dictionary<CameraState, ITransformControl>
        {
            {CameraState.ORBIT, _orbitControl},
            {CameraState.PAN,   new PanControl()},
            {CameraState.ROTATE, new RotationControl()},
            {CameraState.MOUSE_ZOOM, new MouseZoomControl()}
        };

        Reset();
    }

    void Reset()
    {
        _currentState = CameraState.IDLE;
        _activeMouseButton = MouseButton.NONE;
        _activeControl = null;
    }

    void Update()
    {
        if (_activeControl == null) return;

        _activeControl.Update();
    }

    void UpdateState(CameraState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        _activeControl = null;

        if (_currentState != CameraState.IDLE)
        {
            _activeControl = _transformControls[_currentState];
            _activeControl.Init(transform);
        }

        // Set OrbitControl's rotation point if the camera pans, or rotates
        // It can also be reset by zooming past the rotation point, but that's
        // handled in OrbitControl's Init method.
        if(_currentState == CameraState.PAN || _currentState == CameraState.ROTATE)
        {
            _orbitControl.ResetOrbitPoint();
        }
    }

    protected override void RegisterEvents()
    {
        Dispatcher.InputContextEventHandler += HandleInputContext;
        Dispatcher.MouseButtonEventHandler += HandleMouseButton;
        Dispatcher.ScrollEventHandler += HandleScrolling;
        Dispatcher.HotKeyEventHandler += HandleHotKey;
    }

    protected override void DeregisterEvents()
    {
        Dispatcher.InputContextEventHandler -= HandleInputContext;
        Dispatcher.MouseButtonEventHandler -= HandleMouseButton;
        Dispatcher.ScrollEventHandler -= HandleScrolling;
        Dispatcher.HotKeyEventHandler -= HandleHotKey;
    }

    void HandleInputContext(object sender, InputContextEventArgs args)
    {
        this.enabled = args.context == InputContext.WORLD;
        if(!this.enabled)
        {
            Reset();
        }
    }

    void HandleMouseButton(object sender, MouseButtonEventArgs args)
    {
        if(args.state == ButtonState.DOWN && _activeMouseButton == MouseButton.NONE)
        {
            _activeMouseButton = args.button;
        }else if(args.state == ButtonState.UP && _activeMouseButton == args.button)
        {
            _activeMouseButton = MouseButton.NONE;
        }else
        {
            return;
        }

        switch(_activeMouseButton)
        {
            case MouseButton.LEFT:
                if (_altDown)
                {
                    UpdateState(CameraState.ORBIT);
                }
                break;
            case MouseButton.RIGHT:
                if(_altDown)
                {
                    UpdateState(CameraState.MOUSE_ZOOM);
                } else
                {
                    UpdateState(CameraState.ROTATE);
                }
                break;
            case MouseButton.MIDDLE:
                UpdateState(CameraState.PAN);
                break;
            case MouseButton.NONE:
                UpdateState(CameraState.IDLE);
                break;
        }    
    }

    void HandleScrolling(object sender, ScrollEventArgs args)
    {
        if (_currentState != CameraState.IDLE) return;

        _scrollZoomControl.ScrollDelta = args.scrollDelta;
        _scrollZoomControl.Update();
    }

    void HandleHotKey(object sender, HotKeyEventArgs args)
    {
        if (args.hotKey == HotKey.ALT)
            _altDown = args.state == ButtonState.DOWN;
    }

    void HandleWorldObjectSelected(object sender, EventArgs args)
    {

    }
}
