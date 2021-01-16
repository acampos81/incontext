using System;
using UnityEngine;

public class MouseControl : TransformControlBase
{
    protected Transform _transform;
    protected Vector3 _mouseStart;

    public override void Init(Transform transform)
    {
        _transform = transform;
        _mouseStart = Input.mousePosition;
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
}
