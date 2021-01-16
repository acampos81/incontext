using UnityEngine;

public class OrbitControl : MouseControl
{
    public Transform OrbitTarget { get; set; }

    private Vector3 _orbitPoint;
    private Quaternion _startRotation;
    private Vector3 _startPosition;
    private float _startDistance;
    private float _startXZAngle;
    private float _startYAngle;

    public override void Init(Transform transform)
    {
        base.Init(transform);
        SetOrbitPoint();
        SetStartParameters();
    }

    public override void Update()
    {
        // Mouse movement delta
        var mousePosition = Input.mousePosition;
        var delta = (mousePosition - _mouseStart) * .1f;

        // Vector from the orbit point to the this transform
        var currentOrbitVector = _transform.position - _orbitPoint;

        // Project currentOrbitVector onto the XZ plane.
        // Represents a distance and direction, starting at the world origin.
        var xzVector = MathUtil.ProjectVectorOnPlane(currentOrbitVector, Vector3.forward, Vector3.right);

        // Add the horizontal mouse movement to the transform's starting Y-axis rotation.
        // Use the new Y-axis angle to determine a XZ position relative to _orbitPoint
        var xzPosition = MathUtil.PositionFromAngle(_startYAngle - delta.x, xzVector.magnitude, Vector3.forward, Vector3.right);

        // Add the vertical mouse movement to the transform's starting angle from the xzVector.
        // Use the angle to determine the y position.
        var yPosition = MathUtil.PositionFromAngle(_startXZAngle - delta.y, _startDistance, Vector3.up, xzPosition);

        // Update the transform position relative to the orbit point.
        _transform.position = _orbitPoint + yPosition;

        // Determine the LookAt rotations
        if (OrbitTarget != null)
        {
            _transform.LookAt(_orbitPoint);
        }else
        {
            var xzOrtho = Vector3.Cross(Vector3.up, xzVector);
            var vRotation = Quaternion.AngleAxis(delta.x, Vector3.up);
            var hRotation = Quaternion.AngleAxis(delta.y, xzOrtho);
            var offsetRotation = hRotation * vRotation;
            _transform.rotation = offsetRotation * _startRotation;
        }
            
    }

    private void SetOrbitPoint()
    {
        if (OrbitTarget == null)
        {
            ResetOrbitPoint();
            return;
        }

        _orbitPoint = OrbitTarget.position;

        // Check if the distance to the target is less than 1.
        var distance = (_orbitPoint - _transform.position).magnitude;

        if(distance < 1f)
            ResetOrbitPoint();

    }

    private void SetStartParameters()
    {
        var orbitVector = _transform.position - _orbitPoint;
        var ySign = Mathf.Sign(orbitVector.y);
        var xzVector = MathUtil.ProjectVectorOnPlane(orbitVector, Vector3.forward, Vector3.right);

        _startDistance = orbitVector.magnitude;
        _startXZAngle = MathUtil.VectorAngle(orbitVector, xzVector) * ySign;
        _startYAngle = Mathf.Atan2(xzVector.z, xzVector.x) * Mathf.Rad2Deg;
        _startPosition = _transform.position;
        _startRotation = _transform.rotation;
    }

    public void ResetOrbitPoint()
    {
        var position = _transform.position;
        var distance = 10f;

        // 1. Scale the camara's forward vector by the specified distance.
        var forward = _transform.forward * distance;

        // 2. Project the forward vector, onto the XZ plane.
        var xzVector = MathUtil.ProjectVectorOnPlane(forward, Vector3.forward, Vector3.right);

        // 3. If the forward position crosses the ground plane, adjust the distance
        //    in order to set it on the ground plane.
        var forwardPosition = position + forward;
        if ( (position.y > 0f && forwardPosition.y < 0f ) ||
             (position.y < 0f && forwardPosition.y > 0f ))
        {
            var angle = MathUtil.VectorAngle(forward, xzVector);
            distance = Mathf.Abs(position.y) / Mathf.Sin(angle * Mathf.Deg2Rad);
            forward = _transform.forward * distance;
            forwardPosition = position + forward;
        }

        // 4. The forwardPosition becomes the orbit point.
        _orbitPoint = forwardPosition;
    }
}