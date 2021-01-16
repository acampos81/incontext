using UnityEngine;

public class OrbitControl : MouseControl
{
    private Vector3 _orbitPoint;
    private float _startDistance;
    private float _startXAngle;
    private float _startYAngle;

    public override void Init(Transform transform)
    {
        base.Init(transform);
        CheckDistanceToOrbitPoint();
        SetStartParameters();
    }

    public override void Update()
    {
        // Mouse movement delta
        var mousePosition = Input.mousePosition;
        var delta = (mousePosition - _mouseStart) * .1f;

        // Vector from the orbit point to the this transform
        var currentOrbitVector = _transform.position - _orbitPoint;

        // Project the orbit vector onto the plane formed by x and z axis.
        var xzVector = MathUtil.ProjectVectorOnPlane(currentOrbitVector, Vector3.forward, Vector3.right);

        // Turn the y mouse movement into a y position based on the angle from the projected xz vector.
        var yPosition = MathUtil.PositionFromAngle(_startXAngle - delta.y, _startDistance, Vector3.up, xzVector);

        // Turn the x mouse movement into a position on the XZ plane.
        var xzPosition = MathUtil.PositionFromAngle(_startYAngle - delta.x, xzVector.magnitude, Vector3.forward, Vector3.right);

        // Apply the y coordinate to the xz vector
        xzPosition.y = yPosition.y;
        
        // Update the transform position
        _transform.position = xzPosition;

        // Update the transform rotation
        _transform.LookAt(_orbitPoint);
    }

    private void CheckDistanceToOrbitPoint()
    {
        // Check if the camera has moved past the orbit point.
        // This indicates zooming in past the point.
        var distance = (_orbitPoint - _transform.position).magnitude;

        if(distance < 1f)
            ResetOrbitPoint();
    }

    private void SetStartParameters()
    {
        var startVector = _transform.position - _orbitPoint;
        var xzVector = MathUtil.ProjectVectorOnPlane(startVector, Vector3.forward, Vector3.right);

        _startDistance = startVector.magnitude;
        _startYAngle = Mathf.Atan2(xzVector.z, xzVector.x) * Mathf.Rad2Deg;
        _startXAngle = MathUtil.VectorAngle(startVector, xzVector);
    }

    public void SetOrbitPoint(Vector3 point)
    {
        _orbitPoint = point;
    }

    public void ResetOrbitPoint()
    {
        // 1. Scale the camara's forward vector (look direction) by 5 units.
        var forward = _transform.forward * 5f;

        // 2. Take the dot product of the forward vector and the X and Z axes.
        var dotX = Vector3.Dot(forward, Vector3.right);
        var dotZ = Vector3.Dot(forward, Vector3.forward);

        // 3. The dot products produce the orbit point on the XZ plane.
        _orbitPoint = new Vector3(dotX, 0f, dotZ);
    }
}
