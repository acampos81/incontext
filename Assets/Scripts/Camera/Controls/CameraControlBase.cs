using UnityEngine;

public abstract class CameraControlBase : ICameraControl
{
    public abstract void Init();
    public abstract void UpdateCamera(Transform cameraTransform);
}
