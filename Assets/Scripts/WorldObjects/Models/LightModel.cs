using System;
using SimpleJSON;
using UnityEngine;

public class LightModel : ILightModel
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

    private float _coneAngle;
    public float ConeAngle
    {
        get { return _coneAngle; }
        set
        {
            _coneAngle = value;
            if (OnModelUpdate != null)
                OnModelUpdate();
        }
    }

    public JSONNode ToJSON()
    {
        JSONNode modelNode = new JSONObject();
        modelNode["type"] = Type.ToString();
        modelNode["localCenterPoint"] = new JSONObject().WriteVector3(LocalCenterPoint);
        modelNode["position"] = new JSONObject().WriteVector3(_position);
        modelNode["rotation"] = new JSONObject().WriteQuaternion(_rotation);
        modelNode["intensity"] = _intensity;
        modelNode["coneAngle"] = _coneAngle;
        return modelNode;
    }

    public void FromJSON(JSONNode modelNode)
    {
        Type = (WorldObjectType)Enum.Parse(typeof(WorldObjectType), modelNode["type"].Value);
        LocalCenterPoint = modelNode["localCenterPoint"].ReadVector3();
        _position = modelNode["position"].ReadVector3();
        _rotation = modelNode["rotation"].ReadQuaternion();
        _intensity = modelNode["intensity"].AsFloat;
        _coneAngle = modelNode["coneAngle"].AsFloat;
    }
}
