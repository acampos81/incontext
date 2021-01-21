using System.Collections.Generic;
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
        _selectableView.OnViewSelected = HandleViewSelected;
        OnAwake();
    }

    private void HandleViewSelected(MouseClickType clickType)
    {
        WorldObjectManager.Instance.WorldObjectClicked(this, clickType);
    }

    protected void UpdateViewColor(Color color)
    {
        _selectableView.defaultMaterial = WorldObjectMaterials.Instance.GetDiffuseMaterial(color);
        _selectableView.highlightMaterial = WorldObjectMaterials.Instance.GetHighlightMaterial(color);
    }

    protected abstract void OnAwake();
    protected abstract void HandleModelUpdate();
}
