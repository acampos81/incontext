using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour,
    IInputTypeListener,
    IMouseListener,
    IKeyListener
{
    private Dictionary<CameraState, ICameraControl> _stateControls;
    private CameraState _currentState;
    private ICameraControl _currentStateControl;
    private ScrollZoomControl _scrollZoomControl;
    private MouseButton _currentMouseButton;
    private bool _ctrlDown;
    private bool _altDown;

    private void Awake()
    {
        _stateControls = new Dictionary<CameraState, ICameraControl>
        {
            {CameraState.ORBIT, new OrbitControl()},
            {CameraState.PAN,   new PanControl()},
            {CameraState.ROTATE, new RotationControl()},
            {CameraState.MOUSE_ZOOM, new MouseZoomControl()}
        };

        // Scrollwheel zooming is stateless.  It's handled
        // whenever a delta greater than 0 is detected.
        _scrollZoomControl = new ScrollZoomControl();

        _currentState = CameraState.IDLE;
    }

    void Update()
    {
        if (_currentStateControl == null) return;

        _currentStateControl.UpdateCamera(transform);
    }

    void UpdateState(CameraState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        _currentStateControl = null;

        if (_currentState != CameraState.IDLE)
        {
            _currentStateControl = _stateControls[_currentState];
            _currentStateControl.Init();
        }
    }

    public void HandleInputType(InputType inputType)
    {
        this.enabled = inputType == InputType.WORLD;
    }

    public void HandleMouseButtonDown(MouseButton button)
    {
        if (_currentState != CameraState.IDLE) return;

        _currentMouseButton = button;

        switch(_currentMouseButton)
        {
            case MouseButton.LEFT:
                if (_ctrlDown)
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
        }    
    }

    public void HandleMouseButtonUp(MouseButton button)
    {
        if (_currentMouseButton != button) return;
        
        _currentMouseButton = MouseButton.NONE;
        UpdateState(CameraState.IDLE);
    }

    public void HandleScrolling(Vector2 scrollDelta)
    {
        if (_currentState != CameraState.IDLE) return;

        _scrollZoomControl.Init();
        _scrollZoomControl.UpdateCamera(transform);
    }

    public void HandleKeyDown(AppKey appKey)
    {
        // If either key is not down, set it's flag to true if the keycode matches

        if(!_altDown)
            _altDown = appKey == AppKey.ALT;

        if(!_ctrlDown)
            _ctrlDown = appKey == AppKey.CTRL;
    }

    public void HandleKeyUp(AppKey appKey)
    {
        // If either key is down, set it's flag to false if the keycode matches
        if(_altDown)
            _altDown = !(appKey == AppKey.ALT);

        if(_ctrlDown)
            _ctrlDown = !(appKey == AppKey.CTRL);
    }


    private void OnDisable()
    {
        UpdateState(CameraState.IDLE);
    }

}
