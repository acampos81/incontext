using UnityEngine;

public class ShapeView : WorldObjectViewBase
{
    private IShape _model;

    protected override void OnAwake()
    {
        var model = new ShapeModel();
        model.Type = objectType;
        model.Color = Color.gray;
        model.Positon = transform.position;
        model.LocalCenterPoint = localCenterPoint;
        model.OnModelUpdate = HandleModelUpdate;
        _model = model;
    }

    public override IWorldObjectModel GetModel()
    {
        return _model;
    }

    private void HandleModelUpdate()
    {
        transform.position = _model.Positon;
        UpdateMaterial(WorldObjectManager.Instance.GetHighlightMaterial(_model.Color));
    }

    private void OnMouseEnter()
    {
        UpdateMaterial(WorldObjectManager.Instance.GetHighlightMaterial(_model.Color));
    }

    private void OnMouseExit()
    {
        UpdateMaterial(WorldObjectManager.Instance.GetIdleMaterial(_model.Color));
    }
}
