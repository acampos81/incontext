using UnityEngine;

public class WorldObjectRotation : WorldObjectControl
{
    private Vector3 _startPoint;
    private Quaternion _startRotation;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startRotation = transform.rotation;
            var ray = MathUtil.GetScreenInputRay(Camera.main);
            _startPoint = MathUtil.GetRaySphereIntersection(transform.position, 1f, ray);
        }

        if (Input.GetMouseButton(0))
        {
            var rotation = MathUtil.GetScreenToWorldRotation(Camera.main, gameObject, _startRotation, _startPoint);
            _model.Rotation = rotation;
        }
    }
}
