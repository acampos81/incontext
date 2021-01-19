using System;
using UnityEngine;

public class ShapeModel : IShape
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
