using System;
using UnityEngine;

public class LightModel : ILight
{
    public Action OnModelUpdate { get; set; }

    public WorldObjectType Type { get; set; }

    public Vector3 LocalCenterPoint { get; set; }

    private Vector3 _position;
    public Vector3 Position
    {
        get { return _position; }
        set
        {
            _position = value;
            if (OnModelUpdate != null)
                OnModelUpdate();
        }
    }

    private Quaternion _rotation;
    public Quaternion Rotation
    {
        get { return _rotation; }
        set
        {
            _rotation = value;
            if (OnModelUpdate != null)
                OnModelUpdate();
        }
    }

    private float _intensity;
    public float Intensity
    {
        get { return _intensity; }
        set
        {
            _intensity = value;
            if (OnModelUpdate != null)
                OnModelUpdate();
        }
    }
}
