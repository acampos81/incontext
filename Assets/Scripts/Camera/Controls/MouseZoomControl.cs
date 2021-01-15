using UnityEngine;

public class MouseZoomControl : MouseControl
{
    private float _lastYDelta;

    public override void Init()
    {
        _lastYDelta = 0f;
        base.Init();
    }

    public override void UpdateCamera(Transform cameraTransform)
    {
        var mousePosition = Input.mousePosition;
        var yDelta = (mousePosition - _mouseStart).y * .05f;
        var zoomDelta = yDelta - _lastYDelta;
        _lastYDelta = yDelta;

        var position = cameraTransform.position;
        var forward = cameraTransform.forward;
        var translation = forward * zoomDelta;
        cameraTransform.position = position + translation;
    }
}
