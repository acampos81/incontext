using UnityEngine;

public class OrbitControl : MouseControl
{
    public Transform OrbitTarget { get; set; }

    private Vector3 _orbitPoint;
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
        var yAngle = _startYAngle - delta.x;
        var xzPosition = _orbitPoint + MathUtil.PositionFromAngle(yAngle, xzVector.magnitude, Vector3.forward, Vector3.right);

        // Add the vertical mouse movement to the transform's starting angle from the xzVector.
        // Use the angle to determine the y position.
        var yPosition = MathUtil.PositionFromAngle(_startXZAngle - delta.y, _startDistance, Vector3.up, xzVector);

        // Add the y coordinate to xzPosition.
        xzPosition.y = yPosition.y;

        // Update the transform position
        _transform.position = xzPosition;

        // Determine the LookAt rotation
        if (OrbitTarget != null)
        {
            _transform.LookAt(_orbitPoint);
        }else
        {
            _transform.rotation = Quaternion.AngleAxis(90f+yAngle, Vector3.down);
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
        var xzVector = MathUtil.ProjectVectorOnPlane(orbitVector, Vector3.forward, Vector3.right);

        _startDistance = orbitVector.magnitude;
        _startXZAngle = MathUtil.VectorAngle(orbitVector, xzVector);
        _startYAngle = Mathf.Atan2(xzVector.z, xzVector.x) * Mathf.Rad2Deg;

        Debug.Log(string.Format("startDistance:{0}, startX:{1}, startY{2}", _startDistance, _startXZAngle, _startYAngle));
    }

    public void ResetOrbitPoint()
    {
        // 1. Scale the camara's forward vector (look direction) by 10 units.
        var forward = _transform.forward * 10f;
        
        // 2. Project the forward vector on the XZ plane.
        var xzVector = MathUtil.ProjectVectorOnPlane(forward, Vector3.forward, Vector3.right);

        // 3. Add the xzVector to the transform's xz position to determine the orbit point.
        var position = _transform.position;
        _orbitPoint = new Vector3(position.x + xzVector.x, 0f, position.z + xzVector.z);

        Debug.Log("OrbitPoint: "+_orbitPoint);
    }
}