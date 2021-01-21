using UnityEngine;

[RequireComponent(typeof(SelectableView))]
public abstract class WorldObjectViewBase : MonoBehaviour, IWorldObjectView
{
    private SelectableView _selectableView;

    public abstract IWorldObjectModel Model { get; set; }

    public WorldObjectType objectType;
    public Vector3 localCenterPoint;

    void Awake()
    {
        _selectableView = GetComponent<SelectableView>();
        _selectableView.OnViewClicked = HandleViewClicked;
        OnAwake();
    }

    public void SetSelected(bool isSelected)
    {
        if(isSelected)
        {
            _selectableView.ForceHighlight();
        }else
        {
            _selectableView.Reset();
        }
    }

    private void HandleViewClicked(MouseClickType clickType)
    {
        WorldObjectManager.Instance.WorldObjectClicked(this, clickType);
    }

    protected void UpdateViewColor(Color color)
    {
        _selectableView.DefaultMaterial = WorldObjectMaterials.Instance.GetDiffuseMaterial(color);
        _selectableView.HighlightMaterial = WorldObjectMaterials.Instance.GetHighlightMaterial(color);
    }

    protected abstract void OnAwake();
    protected abstract void HandleModelUpdate();
}
