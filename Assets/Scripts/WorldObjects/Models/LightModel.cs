using System;
using UnityEngine;

public class LightModel : ILight
{
    public Action OnModelUpdate;

    public WorldObjectType Type { get; set; }

    private Vector3 _position;
    public Vector3 Positon
    {
        get { return _position; }
        set
        {
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
            if (OnModelUpdate != null)
                OnModelUpdate();
        }
    }

    public LightModel()
    {
        Type = WorldObjectType.LIGHT;
    }
}
