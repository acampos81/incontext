using UnityEngine;

public interface ILight : IWorldObjectModel
{
    float Intensity { get; set; }
    Quaternion Rotation { get; set; }
}
