using System;
using UnityEngine;

public static class MathUtil
{
    public static float VectorAngle(Vector3 vectorA, Vector3 vectorB)
    {
        var dot = Vector3.Dot(vectorA.normalized, vectorB.normalized);
        return Mathf.Acos(dot) * Mathf.Rad2Deg;
    }

    public static Vector3 PositionFromAngle(float angle, float distance, Vector3 verticalAxis, Vector3 horizontalAxis)
    {
        var v = distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        var h = distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        return (verticalAxis.normalized * v) + (horizontalAxis.normalized * h);
    }

    public static Vector3 ProjectVectorOnPlane(Vector3 vector, Vector3 verticalAxis, Vector3 horizontalAxis)
    {
        var vDot = Vector3.Dot(vector, verticalAxis.normalized);
        var hDot = Vector3.Dot(vector, horizontalAxis.normalized);
        var projectedVector = new Vector3(hDot, 0f, vDot);
        return projectedVector;
    }
}
