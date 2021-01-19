using System;
using UnityEngine;

public class ShapeModel : IShape
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

    private Color _color;
    public Color Color
    {
        get { return _color; }
        set
        {
            _color = value;
            if (OnModelUpdate != null)
                OnModelUpdate();
        }
    }
}
