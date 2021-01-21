using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SelectableView : MonoBehaviour
{
    private const float DOUBLE_CLICK_MS = 500f;

    private Stopwatch _stopWatch;

    public Action<MouseClickType> OnViewSelected;
    public Material defaultMaterial;
    public Material highlightMaterial;
    public List<MeshRenderer> meshRenderers;

    void Awake()
    {
        _stopWatch = new Stopwatch();
    }

    void Start()
    {
        UpdateMaterial(defaultMaterial);
    }

    void Update()
    {
        if (_stopWatch.IsRunning &&
            _stopWatch.ElapsedMilliseconds > DOUBLE_CLICK_MS)
        {
            _stopWatch.Stop();
        }
    }

    void OnMouseEnter()
    {
        UpdateMaterial(highlightMaterial);
    }

    void OnMouseExit()
    {
        UpdateMaterial(defaultMaterial);
    }

    void OnMouseUp()
    {
        var clickType = MouseClickType.SINGLE;

        if (_stopWatch.IsRunning)
        {
            _stopWatch.Stop();
            if (_stopWatch.ElapsedMilliseconds <= DOUBLE_CLICK_MS)
                clickType = MouseClickType.DOUBLE;
        }
        else
        {
            _stopWatch.Reset();
            _stopWatch.Start();
        }

        HandleClick(clickType);
    }

    private void UpdateMaterial(Material mat)
    {
        foreach (MeshRenderer renderer in meshRenderers)
            renderer.sharedMaterial = mat;
    }

    private void HandleClick(MouseClickType clickType)
    {
        if (OnViewSelected != null)
            OnViewSelected(clickType);
    }
}
