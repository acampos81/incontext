using UnityEngine;

public class PositionAxisControl : AxisControlBase
{
    private Vector3 _mouseStart;

    protected override void Init()
    {
        _mouseStart = Input.mousePosition;
    }

    protected override void UpdateAxis()
    {
        var mouseDelta = Input.mousePosition - _mouseStart;
        var modelPosition = _model.Position + worldAxis * mouseDelta.y * 0.001f;
        _model.Position = modelPosition;
    }
}
