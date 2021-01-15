using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour,
    IInputTypeListener,
    IMouseListener,
    IKeyListener
{
    private CameraState _currentState;
    private ICameraControl _activeControl;
    private Dictionary<CameraState, ICameraControl> _stateControls;

    private void Awake()
    {
        _stateControls = new Dictionary<CameraState, ICameraControl>
        {
            {CameraState.IDLE, null },
            {CameraState.ORBIT, new OrbitControl()},
            {CameraState.PAN,   new PanControl()},
            {CameraState.ROTATE, new RotationControl()},
            {CameraState.ZOOM, new ZoomControl()}
        };

        _currentState = CameraState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        if (_activeControl != null)
            _activeControl.UpdateCamera(transform);
    }

    void UpdateState(CameraState newState)
    {
        if( _currentState == newState) return;

        _currentState = newState;
        _activeControl = _stateControls[_currentState];

        Logger.Camera(string.Format("Camera State Update: {0}", _currentState));
        Logger.Camera(string.Format("Active Controller: {0}", _activeControl));
    }

    public void HandleInputType(InputType inputType)
    {
        this.enabled = inputType == InputType.WORLD;
        Logger.Camera(string.Format("Input Type Update: {0}",inputType));
    }

    public void HandleMouseButtonDown(MouseButton button)
    {
        Logger.Camera("Mouse Button Down "+button);
    }

    public void HandleMouseButtonUp(MouseButton button)
    {
        Logger.Camera("Mouse Button Up "+button);
    }

    public void HandleScrolling(float scrollDelta)
    {
        Logger.Camera("Scroll Delta "+scrollDelta);
    }

    public void HandleKeyDown(KeyCode keyCode)
    {
        Logger.Camera(keyCode.ToString());
    }

    public void HandleKeyUp(KeyCode keyCode)
    {
        Logger.Camera(keyCode.ToString());
    }

}
