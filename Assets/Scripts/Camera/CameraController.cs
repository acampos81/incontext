using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraController : InputListenerBase,
    IWorldObjectSelectedListener,
    IMouseButtonStateListener,
    IScrollWheelListener,
    IHotKeyListener
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
        // can affect its orbit point.
        _orbitControl = new OrbitControl();
        _orbitControl.Init(transform);

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

    void UpdateCameraState(CameraState newState)
    {
        if (_currentState == newState) return;

        _activeControl = null;
        if (newState != CameraState.IDLE)
        {
            _activeControl = _transformControls[newState];
            _activeControl.Init(transform);
        }

        // Reset OrbitControl's orbit point if the previous camera state
        // was PAN or ROTATE. It can also be reset by zooming past the
        // orbit point but that's handled in OrbitControl's Init method.
        if (newState == CameraState.IDLE &&
          (_currentState == CameraState.PAN ||
           _currentState == CameraState.ROTATE))
        {
            _orbitControl.ResetOrbitPoint();
        }

        _currentState = newState;
    }

    public void HandleMouseButtonState(object sender, MouseButtonStateEventArgs args)
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
                    UpdateCameraState(CameraState.ORBIT);
                }
                break;
            case MouseButton.RIGHT:
                if(_altDown)
                {
                    UpdateCameraState(CameraState.MOUSE_ZOOM);
                } else
                {
                    UpdateCameraState(CameraState.ROTATE);
                }
                break;
            case MouseButton.MIDDLE:
                UpdateCameraState(CameraState.PAN);
                break;
            case MouseButton.NONE:
                UpdateCameraState(CameraState.IDLE);
                break;
        }    
    }

    public void HandleScrollWheel(object sender, ScrollWheelEventArgs args)
    {
        if (_currentState != CameraState.IDLE) return;

        _scrollZoomControl.ScrollDelta = args.scrollDelta;
        _scrollZoomControl.Update();
    }

    public void HandleHotKeyState(object sender, HotKeyEventArgs args)
    {
        if (args.hotKey == HotKey.ALT)
            _altDown = args.state == ButtonState.DOWN;
    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
        if(args.clickType == MouseClickType.DOUBLE)
        {
            var model = args.objectModel;
            var objectCenter = model.Position + model.LocalCenterPoint;
            var cameraOffset = (transform.position - objectCenter).normalized * 3f;
            var cameraPosition = objectCenter + cameraOffset;

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(cameraPosition, .3f).SetEase(Ease.OutQuad));
            seq.Insert(0f,transform.DOLookAt(objectCenter, .3f).SetEase(Ease.OutQuad));
            seq.OnComplete(() => _orbitControl.OrbitPoint = objectCenter);
        }
    }
}
