using UnityEngine;

public abstract class WorldObjectControl : MonoBehaviour, IWorldObjectSelectedListener
{
    protected IWorldObjectModel _model;

    private void UpdateView()
    {
        transform.position = _model.Position + _model.LocalCenterPoint;
        transform.rotation = _model.Rotation;
    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
        // Deregister from previous model's update action
        if (_model != null)
            _model.OnModelUpdate -= UpdateView;

        _model = args.objectModel;

        // Register for this new model's update action.
        if (_model != null)
        {
            _model.OnModelUpdate += UpdateView;
            UpdateView();
        }

        // If the model is null, means no object is selected.  turn off.
        this.gameObject.SetActive(_model != null);
    }
}
