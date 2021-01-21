using SimpleJSON;
using UnityEngine;

public class ShapeModel : WorldObjectModel, IShapeModel
{
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

    public override JSONNode ToJSON()
    {
        JSONNode modelNode = base.ToJSON();
        modelNode["color"] = new JSONObject().WriteColor(_color);
        return modelNode;
    }

    public override void FromJSON(JSONNode modelNode)
    {
        base.FromJSON(modelNode);
        _color = modelNode["color"].ReadColor();
    }
}
