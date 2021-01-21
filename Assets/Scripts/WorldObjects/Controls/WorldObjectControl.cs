public class WorldObjectControl : InputListenerBase,
    IWorldObjectSelectedListener,
    IHotKeyListener
{
    private IWorldObjectModel _model;

    public PositionGizmo positionGizmo;
    public RotationGizmo rotationGizmo;


    private void UpdateView()
    {
        transform.position = _model.Position + _model.LocalCenterPoint;
        positionGizmo.transform.rotation = _model.Rotation;
        rotationGizmo.transform.rotation = _model.Rotation;
    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
        // Deregister from previous model's update action
        if (_model != null)
            _model.OnModelUpdate -= UpdateView;

        _model = args.objectModel;
        positionGizmo.Model = _model;
        rotationGizmo.Model = _model;

        // Register for this new model's update action.
        if (_model != null)
        {
            _model.OnModelUpdate += UpdateView;
            UpdateView();
        }

        // If the model is null, means no object is selected.  turn off.
        this.gameObject.SetActive(_model != null);
    }

    public void HandleHotKeyState(object sender, HotKeyEventArgs args)
    {
        if(args.hotKey == HotKey.KEY_1)
        {
            positionGizmo.gameObject.SetActive(true);
            rotationGizmo.gameObject.SetActive(false);
        }else if(args.hotKey == HotKey.KEY_2)
        {
            positionGizmo.gameObject.SetActive(false);
            rotationGizmo.gameObject.SetActive(true);
        }
    }
}
