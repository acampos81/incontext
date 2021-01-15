using UnityEngine;

public class ScrollZoomControl : CameraControlBase
{
    private Vector2 _scrollDelta;

    public override void Init()
    {
        _scrollDelta = Input.mouseScrollDelta;
    }

    public override void UpdateCamera(Transform cameraTransform)
    {
        Vector3 position = cameraTransform.position;
        Vector3 translation = cameraTransform.forward * _scrollDelta.y;
        cameraTransform.position = position + translation;
    }
}
