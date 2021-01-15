using System;
using UnityEngine;

public class MouseControl : CameraControlBase
{
    protected Vector3 _mouseStart;

    public override void Init()
    {
        _mouseStart = Input.mousePosition;
    }

    public override void UpdateCamera(Transform cameraTransform)
    {
        throw new NotImplementedException();
    }
}
