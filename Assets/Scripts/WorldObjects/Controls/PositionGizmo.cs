using System.Collections.Generic;
using UnityEngine;

public class PositionGizmo : MonoBehaviour
{
    public IWorldObjectModel Model { get; set; }

    public List<PositionAxis> positionAxes;

    private Vector3 _axis;
    private Vector3 _startModelPosition;
    private Vector3 _startClipPosition;
    private Vector3 _initialInputDirection;
    private bool _axisActive;

    private void Awake()
    {
        foreach(PositionAxis axis in positionAxes)
        {
            axis.OnAxisMouseDown += HandleAxisMouseDown;
            axis.OnAxisMouseUp += HandleAxisMouseUp;
        }
    }

    void HandleAxisMouseDown(PositionAxis positionAxis)
    {
        _axis = positionAxis.Axis;
        _axisActive = true;
    }

    void HandleAxisMouseUp(PositionAxis positionAxis)
    {
        _axisActive = false;
    }

    void Update()
    {
        if (!_axisActive || Model == null) return;

        if(Input.GetMouseButtonDown(0))
        {
            _startModelPosition = Model.Position;
            var ray = MathUtil.GetScreenInputRay(Camera.main);
            _initialInputDirection = ray.direction;
            _startClipPosition = ray.origin;
        }

        if(Input.GetMouseButton(0))
        {
            var ray = MathUtil.GetScreenInputRay(Camera.main);
            var farVector = ray.GetPoint(Camera.main.farClipPlane);
            var inputDirection = (farVector - _startClipPosition).normalized;
            var currentDirection = inputDirection - _initialInputDirection;
            var dot = Vector3.Dot(currentDirection, _axis) ;
            var projectedDirection = _axis * dot;

            Model.Position = _startModelPosition + projectedDirection * 10f;
        }
    }
}
