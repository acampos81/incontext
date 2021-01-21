﻿public class ShapeView : WorldObjectViewBase
{
    private IShapeModel _model;
    public override IWorldObjectModel Model
    {
        get { return _model; }
        set
        {
            if (_model != null)
                _model.OnModelUpdate -= HandleModelUpdate;

            _model = (IShapeModel)value;
            _model.OnModelUpdate += HandleModelUpdate;

            HandleModelUpdate();
        }
    }

    protected override void OnAwake()
    {
        UpdateViewColor(WorldObjectMaterials.Instance.ShapeDefaultColor);
    }

    protected override void HandleModelUpdate()
    {
        transform.position = _model.Position;
        transform.rotation = _model.Rotation;
        UpdateViewColor(_model.Color);
    }
}
