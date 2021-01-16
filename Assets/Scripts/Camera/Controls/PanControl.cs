using UnityEngine;

public class PanControl : MouseControl
{
    public Vector3 _startPosition;

    public override void Init(Transform transform)
    {
        _startPosition = transform.position;
        base.Init(transform);
    }

    public override void Update()
    {
        var mousePosition = Input.mousePosition;
        var delta = (_mouseStart - mousePosition) * .01f;
        var translationX = _transform.right * delta.x;
        var translationY = _transform.up * delta.y;
        var position = _startPosition + translationX + translationY;
        _transform.position = position;
    }
}
