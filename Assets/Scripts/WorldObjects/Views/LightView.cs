using UnityEngine;

public class LightView : WorldObjectViewBase
{
    public Light lightComponent;

    private ILightModel _model;
    public override IWorldObjectModel Model {
        get { return _model; }
        set {
            if (_model != null)
                _model.OnModelUpdate -= HandleModelUpdate;

            _model = (ILightModel)value;
            _model.OnModelUpdate += HandleModelUpdate;

            HandleModelUpdate();
        }
    }

    protected override void OnAwake()
    {
        UpdateViewColor(WorldObjectMaterials.Instance.LightDefaultColor);
    }


    protected override void HandleModelUpdate()
    {
        transform.position = _model.Position;
        transform.rotation = _model.Rotation;
        lightComponent.intensity = _model.Intensity;
        lightComponent.spotAngle = _model.ConeAngle;
    }
}
