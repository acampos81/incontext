using UnityEngine;

public class RotationGizmo : MonoBehaviour
{
    public IWorldObjectModel Model { get; set; }

    private Vector3 _startPoint;
    private Quaternion _startRotation;

    void Update()
    {
        if (Model == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            _startRotation = transform.rotation;
            var ray = MathUtil.GetScreenInputRay(Camera.main);
            _startPoint = MathUtil.GetRaySphereIntersection(transform.position, 1f, ray);
        }

        if (Input.GetMouseButton(0))
        {
            var rotation = MathUtil.GetScreenToWorldRotation(Camera.main, gameObject, _startRotation, _startPoint);
            Model.Rotation = rotation;
        }
    }
}
