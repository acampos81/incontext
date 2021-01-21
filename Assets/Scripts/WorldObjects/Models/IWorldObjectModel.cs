using System;
using SimpleJSON;
using UnityEngine;

public interface IWorldObjectModel
{
    WorldObjectType Type { get; set; }
    Vector3 Position { get; set; }
    Quaternion Rotation { get; set; }
    Vector3 LocalCenterPoint { get; set; }
    Action OnModelUpdate { get; set; }
    JSONNode ToJSON();
    void FromJSON(JSONNode json);
}
