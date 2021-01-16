using System;
using UnityEngine;

public abstract class TransformControlBase : ITransformControl
{
    public abstract void Init(Transform transform);
    public abstract void Update();
}
