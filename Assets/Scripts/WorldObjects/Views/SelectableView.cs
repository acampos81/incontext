using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SelectableView : MonoBehaviour
{
    private const float DOUBLE_CLICK_MS = 500f;

    private Stopwatch _stopWatch;
    private bool _forceHighlight;

    [SerializeField]
    private Material _defaultMaterial;
    public Material DefaultMaterial
    {
        get { return _defaultMaterial; }
        set
        {
            _defaultMaterial = value;
            UpdateView(value);
        }
    }

    [SerializeField]
    private Material _highlightMaterial;
    public Material HighlightMaterial
    {
        get { return _highlightMaterial; }
        set
        {
            _highlightMaterial = value;
            UpdateView(value);
        }
    }

    public Action<MouseClickType> OnViewClicked;
    public List<MeshRenderer> meshRenderers;


    void Awake()
    {
        _stopWatch = new Stopwatch();
    }

    void Start()
    {
        Reset();
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
        UpdateView(_highlightMaterial);
    }

    void OnMouseExit()
    {
        UpdateView(_defaultMaterial);
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

    public void ForceHighlight()
    {
        _forceHighlight = true;
        UpdateMaterial(_highlightMaterial);
    }

    public void Reset()
    {
        _forceHighlight = false;
        UpdateMaterial(_defaultMaterial);
    }

    private void UpdateView(Material mat)
    {
        if(_forceHighlight)
        {
            UpdateMaterial(_highlightMaterial);
        }else
        {
            UpdateMaterial(mat);
        }
    }

    private void UpdateMaterial(Material mat)
    {
        foreach (MeshRenderer renderer in meshRenderers)
            renderer.sharedMaterial = mat;
    }

    private void HandleClick(MouseClickType clickType)
    {
        if (OnViewClicked != null)
            OnViewClicked(clickType);
    }

    private void OnDisable()
    {
        Reset();
    }
}
