using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PositionAxis : MonoBehaviour
{
    public Action<PositionAxis> OnAxisMouseDown;
    public Action<PositionAxis> OnAxisMouseUp;

    [SerializeField]
    private PositionAxis _orthoAxis;
    public Vector3 OrthoAxis { get { return _orthoAxis.transform.forward; } }

    public Vector3 Axis { get { return transform.forward; } }

    private void OnMouseDown()
    {
        if (OnAxisMouseDown != null)
            OnAxisMouseDown(this);
    }

    private void OnMouseUp()
    {
        if (OnAxisMouseUp != null)
            OnAxisMouseUp(this);
    }

}
