using System;
using SimpleJSON;
using UnityEngine;

public class WorldObjectModel : IWorldObjectModel
{
    public Action OnModelUpdate { get; set; }

    public string Name { get; set; }
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

    public virtual JSONNode ToJSON()
    {
        JSONNode modelNode = new JSONObject();
        modelNode["name"] = Name;
        modelNode["type"] = Type.ToString();
        modelNode["localCenterPoint"] = new JSONObject().WriteVector3(LocalCenterPoint);
        modelNode["position"] = new JSONObject().WriteVector3(_position);
        modelNode["rotation"] = new JSONObject().WriteQuaternion(_rotation);
        return modelNode;
    }

    public virtual void FromJSON(JSONNode modelNode)
    {
        Name = modelNode["name"];
        Type = (WorldObjectType)Enum.Parse(typeof(WorldObjectType), modelNode["type"].Value);
        LocalCenterPoint = modelNode["localCenterPoint"].ReadVector3();
        _position = modelNode["position"].ReadVector3();
        _rotation = modelNode["rotation"].ReadQuaternion();
    }
}
