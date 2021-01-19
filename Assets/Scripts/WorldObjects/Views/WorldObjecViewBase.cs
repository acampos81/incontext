using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class WorldObjectViewBase : MonoBehaviour, IWorldObjectView
{
    private const float DOUBLE_CLICK_MS = 500f;

    private Stopwatch _stopWatch;

    public WorldObjectType objectType;
    public Vector3 localCenterPoint;
    public List<MeshRenderer> meshRenderers;

    private void Awake()
    {
        OnAwake();
        _stopWatch = new Stopwatch();
    }

    private void Update()
    {
        if(_stopWatch.IsRunning &&
            _stopWatch.ElapsedMilliseconds > DOUBLE_CLICK_MS)
        {
            _stopWatch.Stop();
        }
    }

    private void OnMouseUp()
    {
        if(_stopWatch.IsRunning)
        {
            _stopWatch.Stop();
            HandleClick();
        }else
        {
            _stopWatch.Reset();
            _stopWatch.Start();
        }
    }

    private void HandleClick()
    {
        var clickType = MouseClickType.SINGLE;

        if (_stopWatch.ElapsedMilliseconds <= DOUBLE_CLICK_MS)
            clickType = MouseClickType.DOUBLE;

        WorldObjectManager.Instance.WorldObjectClicked(this, clickType);
    }

    protected void UpdateMaterial(Material material)
    {
        foreach (var renderer in meshRenderers)
        {
            renderer.sharedMaterial = material;
        }
    }

    protected abstract void OnAwake();
    public abstract IWorldObjectModel GetModel();
}
