using System;
using UnityEngine;

public static class MathUtil
{
    /// <summary>
    /// Calculates the angle between two vectors.
    /// </summary>
    /// <param name="vectorA"></param>
    /// <param name="vectorB"></param>
    /// <returns>Angle in degrees</returns>
    public static float VectorAngle(Vector3 vectorA, Vector3 vectorB)
    {
        var dot = Vector3.Dot(vectorA.normalized, vectorB.normalized);
        return Mathf.Acos(dot) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Calculates a position on a plane defined by two orthogonal vectors.
    /// For best results use two vectors with the same starting point.
    /// </summary>
    /// <param name="angle">The angle or rotation from the horizontal axis</param>
    /// <param name="distance">The distance from origin of the plane</param>
    /// <param name="verticalAxis">The vector that represents the vertical plane axis</param>
    /// <param name="horizontalAxis">The vector that represents the horizontal plane axis</param>
    /// <returns>Position relative to the plane's origin</returns>
    public static Vector3 PositionFromAngle(float angle, float distance, Vector3 verticalAxis, Vector3 horizontalAxis)
    {
        var iSin = (int)(Mathf.Sin(angle * Mathf.Deg2Rad) * 1000f);
        var cSin = (int)(Mathf.Cos(angle * Mathf.Deg2Rad) * 1000f);
        var fSin = iSin / 1000f;
        var fCos = cSin / 1000f;
        var v = distance * fSin;
        var h = distance * fCos;
        return (verticalAxis.normalized * v) + (horizontalAxis.normalized * h);
    }

    /// <summary>
    /// Projects a given vector on a plane defined by two orthogonal vectors.
    /// For best results use two vectors with the same starting point.
    /// </summary>
    /// <param name="vector">The vector to project</param>
    /// <param name="verticalAxis">The vector that represents the vertical plane axis</param>
    /// <param name="horizontalAxis">The vector that represents the horizontal plane axis</param>
    /// <returns>The projected vector</returns>
    public static Vector3 ProjectVectorOnPlane(Vector3 vector, Vector3 verticalAxis, Vector3 horizontalAxis)
    {
        var vDot = Vector3.Dot(vector, verticalAxis.normalized);
        var hDot = Vector3.Dot(vector, horizontalAxis.normalized);
        var projectedVector = new Vector3(hDot, 0f, vDot);
        return projectedVector;
    }

    /// <summary>
    /// Calculates the intersection point of lines.
    /// </summary>
    /// <param name="m1">Slope of line 1</param>
    /// <param name="b1">Y-Intercept of line 1</param>
    /// <param name="m2">Slope of line 2</param>
    /// <param name="b2">Y-Intercept of line 2</param>
    /// <returns></returns>
    public static Vector3 IntersectionPoint(float m1, float b1, float m2, float b2)
    {
        // set two equations of a line equal to each other and solve for x
        // y = m1x + b1
        // y = m2x + b2
        // m1x + b1 = m2x + b2
        // m1x - m2x = b2 - b1
        // x(m1 - m2) = b2 - b1
        // x = b2 - b1 / m1 - m2
       
        var bDelta = b2 - b1;
        var mDelta = m1 - m2;

        // Parallel lines
        if (mDelta == 0)
        {
            return new Vector3(float.PositiveInfinity, float.PositiveInfinity);
        }

        var x = bDelta / mDelta;

        // Using line 1 parameters, but the intersection y coordinate will be the same in either line.
        var y = m1 * x + b1;
        return new Vector3(x, y);
    }
}
