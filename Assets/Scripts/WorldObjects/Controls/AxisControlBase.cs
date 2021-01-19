using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public abstract class AxisControlBase : MonoBehaviour,
    IWorldObjectSelectedListener
{
    public Material idleMaterial;
    public Material highLightMaterial;
    public Transform container;
    public Vector3 worldAxis;

    private MeshRenderer _meshRenderer;

    protected bool _controlActive;
    protected Vector3 _controlAxis;
    protected IWorldObjectModel _model;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnMouseEnter()
    {
        _meshRenderer.sharedMaterial = highLightMaterial;
    }

    private void OnMouseExit()
    {
        _meshRenderer.sharedMaterial = idleMaterial;
    }

    private void OnMouseDown()
    {
        _controlActive = true;
        Init();
    }

    private void OnMouseDrag()
    {
        UpdateAxis();
    }

    private void OnMouseUp()
    {
        _controlActive = false;
    }

    private void HandleModelUpdate()
    {
        container.position = _model.Position;
        container.rotation = _model.Rotation;
    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
        if (_model != null)
            _model.OnModelUpdate -= HandleModelUpdate;

        _model = args.objectModel;
        _model.OnModelUpdate += HandleModelUpdate;

        HandleModelUpdate();
    }

    void OnDisable()
    {
        _model.OnModelUpdate -= HandleModelUpdate;
        _model = null;
    }

    protected abstract void Init();
    protected abstract void UpdateAxis();
}
