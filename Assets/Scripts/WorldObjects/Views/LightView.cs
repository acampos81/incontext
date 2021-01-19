using UnityEngine;

public class LightView : WorldObjectViewBase
{
    public Light lightComponent;

    private ILight _model;

    private Color _color;

    protected override void OnAwake()
    {
        var model = new LightModel();
        model.Type = WorldObjectType.LIGHT;
        model.Position = transform.position;
        model.LocalCenterPoint = localCenterPoint;
        model.Rotation = transform.rotation;
        model.Intensity = lightComponent.intensity;
        model.OnModelUpdate += HandleModelUpdate;
        _model = model;

        _color = WorldObjectManager.Instance.avaialbleMaterials[4].color;
        UpdateMaterial(WorldObjectManager.Instance.GetIdleMaterial(_color));
    }

    public override IWorldObjectModel GetModel()
    {
        return _model;
    }

    private void HandleModelUpdate()
    {
        transform.position = _model.Position;
        transform.rotation = _model.Rotation;
        lightComponent.intensity = _model.Intensity;
    }

    private void OnMouseEnter()
    {
        UpdateMaterial(WorldObjectManager.Instance.GetHighlightMaterial(_color));
    }

    private void OnMouseExit()
    {
        UpdateMaterial(WorldObjectManager.Instance.GetIdleMaterial(_color));
    }
}
