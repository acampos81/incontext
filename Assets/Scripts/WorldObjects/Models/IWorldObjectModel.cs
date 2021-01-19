using System;
using UnityEngine;

public interface IWorldObjectModel
{
    Action OnModelUpdate { get; set; }
    WorldObjectType Type { get; set; }
    Vector3 Positon { get; set; }
    Quaternion Rotation { get; set; }
    Vector3 LocalCenterPoint { get; set; }
}
