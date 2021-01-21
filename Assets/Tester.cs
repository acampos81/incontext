using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public LineRenderer line;
    public Transform sphere;
    public Transform marker;

    private Quaternion _startRotation;
    private Vector3 _startPoint;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _startRotation = sphere.rotation;
            var ray = MathUtil.GetScreenInputRay(Camera.main);
            _startPoint = MathUtil.GetRaySphereIntersection(sphere.position, 1f, ray);
        }

        if (Input.GetMouseButton(0))
        {
            var rotation = MathUtil.GetScreenToWorldRotation(Camera.main, sphere.gameObject, _startRotation, _startPoint);
            sphere.rotation = rotation;
        }
    }
}
