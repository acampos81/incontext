using UnityEngine;

public class MouseZoomControl : MouseControl
{
    private float _lastYDelta;

    public override void Init(Transform transform)
    {
        _lastYDelta = 0f;
        base.Init(transform);
    }

    public override void Update()
    {
        var mousePosition = Input.mousePosition;
        var yDelta = (mousePosition - _mouseStart).y * .05f;
        var zoomDelta = yDelta - _lastYDelta;
        _lastYDelta = yDelta;

        var position = _transform.position;
        var forward = _transform.forward;
        var translation = forward * zoomDelta;
        _transform.position = position + translation;
    }
}
