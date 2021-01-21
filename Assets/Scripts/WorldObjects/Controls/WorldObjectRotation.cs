using UnityEngine;

public class WorldObjectRotation : MonoBehaviour, IWorldObjectSelectedListener
{

    private IWorldObjectModel _model;

    private Vector3 _startPoint;
    private Quaternion _startRotation;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startRotation = transform.rotation;
            var ray = MathUtil.GetScreenInputRay(Camera.main);
            _startPoint = MathUtil.GetRaySphereIntersection(transform.position, 1f, ray);
        }

        if (Input.GetMouseButton(0))
        {
            var rotation = MathUtil.GetScreenToWorldRotation(Camera.main, gameObject, _startRotation, _startPoint);
            _model.Rotation = rotation;
        }
    }

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
