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
        var iSin = (int)(Mathf.Sin(angle * Mathf.Deg2Rad) * 10000f);
        var cSin = (int)(Mathf.Cos(angle * Mathf.Deg2Rad) * 10000f);
        var fSin = iSin / 10000f;
        var fCos = cSin / 10000f;
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
    /// Calculates the slope of a line given it's angle of inclination.
    /// </summary>
    /// <param name="angle">Inclination angle</param>
    /// <returns>The slope of the line in radians. NaN implies a vertical line.</returns>
    public static float Slope(float angle)
    {
        // If an angle indicates a vertical line, return NaN
        if (angle % 180 == 90)
            return float.NaN;

        var iTan = (int)(Mathf.Tan(angle * Mathf.Deg2Rad) * 10000f);
        var fTan = iTan / 10000f;
        return fTan;
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

    public static Vector3 GetRaySphereIntersection(Vector3 sphereCenter, float sphereRadius, Ray ray)
    {
        //sphere center
        Vector3 c = sphereCenter;

        //sphere radius
        float r = sphereRadius;

        //world point on camera's near clip plane of screen point
        Vector3 p0 = ray.origin;

        //ray pointing towards camera's far plane along it's field of view)
        Vector3 d = ray.direction;

        // 1. compute e (the vector from the ray origin (p0) in the direction of the center of the sphere (c) ).
        Vector3 e = c - p0;

        // 2. compute dot product of e.d to obtain scalar a (vector magnitude equal to distance from p0 to plane perpedicular to d that contains c).
        float a = Vector3.Dot(e, d);

        // 3. solve for scalar b (length of remaining side of right triangle formed by p0, c, and point implied by a [aPoint] (b <= r implies intersection of d and sphere).
        //    b > r implies that ray is not pointing in a direction that intersects sphere.
        float b = Mathf.Sqrt(Mathf.Pow(e.magnitude, 2) - Mathf.Pow(a, 2));

        //not mathematically necessary, but done in order to keep f >= 0.  This ensures rotation will 
        //continue even if ray is no longer intersecting sphere (use only if needed).
        //		if(b > r) b = r;

        // 4. solve for scalar f (if positive, equals the length of remaining side of the 
        //    inner right triangle c, aPoint, point implied by t (if it exists). negative f =  no intersection)
        float f = Mathf.Sqrt(Mathf.Pow(r, 2) - Mathf.Pow(b, 2));

        //5. solve for t (distance from p0 to sphere intersection)
        float t = a - f;

        //		DrawIntersectionLines(a, b, c, d, e, f, t, r, p0);
        return GetPointOnRay(p0, d, t);
    }

    public static Quaternion GetScreenToWorldRotation(Camera mainCamera, GameObject target, Quaternion initialTargetRotation, Vector3 initialWorldPoint)
    {
        // This method finds the plane(pL) perpedicular to the normal formed by the target center to the initialWorldPoint.  Then the
        // the line drawn from the initial screen point to the current screen point is projected on a plane(pll) parallel to the camera near/far clip planes,
        // at the same distance from the camera to the initial world point.

        // In order to define an x, y and z value within pL, each componenet is solved with equation for plane perpedicular to a given direction:
        // a (x-x0)+ b(y-y0) + c(z-z0) = 0
        // (a, b, c) = direction of normal
        // (x, y, z) = first point on the plane (initialSpherePoint)
        // (x0, y0, z0) = second point on the plane;

        Vector3 radialVector = (initialWorldPoint - target.transform.position);
        Vector3 normalDir = radialVector.normalized;

        //x = (a*x0 + b*y0 + c*z0)/a;
        float perpX = (normalDir.x * initialWorldPoint.x + normalDir.y * initialWorldPoint.y + normalDir.z * initialWorldPoint.z) / normalDir.x;
        Vector3 perpXDir = Vector3.right * perpX;

        //y = (a*x0 + b*y0 + c*z0)/b;
        float perpY = (normalDir.x * initialWorldPoint.x + normalDir.y * initialWorldPoint.y + normalDir.z * initialWorldPoint.z) / normalDir.y;
        Vector3 perpYDir = Vector3.up * perpY;

        //z = (a*x0 + b*y0 + c*z0)/c;
        float perpZ = (normalDir.x * initialWorldPoint.x + normalDir.y * initialWorldPoint.y + normalDir.z * initialWorldPoint.z) / normalDir.z;
        Vector3 perpZDir = Vector3.forward * perpZ;


        //In order to avoid gimbal lock, the direction along pL with the lowest dot product in relation to camera's forward direction is the most perpendicular at the time,
        //therefore the best direction to use when projecting from pll to pL.
        float dotX = Vector3.Dot(mainCamera.transform.forward, perpXDir.normalized);
        float dotY = Vector3.Dot(mainCamera.transform.forward, perpYDir.normalized);
        float dotZ = Vector3.Dot(mainCamera.transform.forward, perpZDir.normalized);
        float lowestDot = Mathf.Min(new float[3] { Mathf.Abs(dotX), Mathf.Abs(dotY), Mathf.Abs(dotZ) });
        Vector3 hDir = Vector3.zero;

        if (lowestDot == Mathf.Abs(dotX))
            hDir = (perpXDir - initialWorldPoint).normalized;
        else if (lowestDot == Mathf.Abs(dotY))
            hDir = (perpYDir - initialWorldPoint).normalized;
        else if (lowestDot == Mathf.Abs(dotZ))
            hDir = (perpZDir - initialWorldPoint).normalized;

        Vector3 vDir = Vector3.Cross(hDir, normalDir).normalized;

        //distance from camera to pll, which contains initialWorldPoint
        float cameraPlaneDistance = Vector3.Dot(mainCamera.transform.forward, initialWorldPoint - mainCamera.transform.position);

        //point equivalent to current input x and y positions on pll
        Vector3 screenToWorldPoint = GetScreenToWorldPoint(mainCamera, Input.mousePosition.x, Input.mousePosition.y, cameraPlaneDistance);
        //		Debug.DrawLine(initialWorldPoint, screenToWorldPoint, Color.white);

        //vector from initialWorldPoint to screenToWorldPoint on pll (which will be used to project onto pL)
        Vector3 cameraPlaneVector = screenToWorldPoint - initialWorldPoint;

        //combined projections result in velocity vector that can be oriented in any direction on pL
        Vector3 velocityVector = Vector3.Dot(cameraPlaneVector, hDir) * hDir + Vector3.Dot(cameraPlaneVector, vDir) * vDir;
        //		Debug.DrawLine(initialWorldPoint, initialWorldPoint+velocityVector, Color.cyan);	

        //omega vector (perpendicular to radialVector, and velocityVector) acts as Exponential Map (e = theta*n);
        Vector3 omegaVector = Vector3.Cross(radialVector, velocityVector) / Mathf.Pow(radialVector.magnitude, 2);
        //		Debug.DrawLine(target.transform.position, omegaVector.normalized, Color.yellow);

        return Quaternion.AngleAxis(omegaVector.magnitude * Mathf.Rad2Deg, omegaVector.normalized) * initialTargetRotation;
    }

    public static Vector3 GetScreenToWorldPoint(Camera camera, float screenX, float screenY, float zDepth)
    {
        return camera.ScreenToWorldPoint(new Vector3(screenX, screenY, zDepth));
    }

    public static Ray GetScreenInputRay(Camera camera)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 nearPlanePoint = GetScreenToWorldPoint(camera, mousePosition.x, mousePosition.y, camera.nearClipPlane);
        Vector3 farPlanePoint = GetScreenToWorldPoint(camera, mousePosition.x, mousePosition.y, camera.farClipPlane);
        return new Ray(nearPlanePoint, (farPlanePoint - nearPlanePoint).normalized);
    }

    public static Vector3 GetPointOnRay(Vector3 origin, Vector3 direction, float distance)
    {
        Ray ray = new Ray(origin, direction);
        Vector3 point = ray.GetPoint(distance);
        return point;
    }
}
