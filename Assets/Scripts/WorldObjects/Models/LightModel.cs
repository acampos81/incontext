using SimpleJSON;

public class LightModel : WorldObjectModel, ILightModel
{
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

    public override JSONNode ToJSON()
    {
        JSONNode modelNode = base.ToJSON();
        modelNode["intensity"] = _intensity;
        modelNode["coneAngle"] = _coneAngle;
        return modelNode;
    }

    public override void FromJSON(JSONNode modelNode)
    {
        base.FromJSON(modelNode);
        _intensity = modelNode["intensity"].AsFloat;
        _coneAngle = modelNode["coneAngle"].AsFloat;
    }
}
