using UnityEngine;

public class ScrollZoomControl : TransformControlBase
{
    private Transform _transform;
    public Vector2 ScrollDelta { get; set; }

    public override void Init(Transform transform)
    {
        _transform = transform;
    }

    public override void Update()
    {
        Vector3 position = _transform.position;
        Vector3 translation = _transform.forward * ScrollDelta.y;
        _transform.position = position + translation;
    }
}
