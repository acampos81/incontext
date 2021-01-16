using UnityEngine;

public class RotationControl : MouseControl
{
    private Vector3 _startRotation;

    public override void Init(Transform transform)
    {
        _startRotation = transform.rotation.eulerAngles;
        base.Init(transform);
    }

    public override void Update()
    {
        var mousePosition = Input.mousePosition;
        var delta = (mousePosition - _mouseStart) * .05f;
        var eulerAngles = new Vector3(-delta.y, delta.x, 0f);
        var rotation = _startRotation + eulerAngles;
        _transform.rotation = Quaternion.Euler(rotation);
    }
}
