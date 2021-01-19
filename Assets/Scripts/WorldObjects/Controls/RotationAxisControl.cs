using UnityEngine;


public class RotationAxisControl : AxisControlBase
{
    private Vector3 _mouseStart;

    protected override void Init()
    {
        _mouseStart = Input.mousePosition;
    }

    protected override void UpdateAxis()
    {
        var mouseDelta = Input.mousePosition - _mouseStart;
        var modelRotation = _model.Rotation.eulerAngles + worldAxis * mouseDelta.y * 0.001f;
        _model.Rotation = Quaternion.Euler(modelRotation);
    }
}
