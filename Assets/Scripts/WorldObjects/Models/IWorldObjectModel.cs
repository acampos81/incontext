using System;
using UnityEngine;

public interface IWorldObjectModel
{
    WorldObjectType Type { get; set; }
    Vector3 Positon { get; set; }
}
