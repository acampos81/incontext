using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour,
    IUIStateDispatcher,
    IUIStateListener,
    IWorldObjectSelectedListener
{
    public event EventHandler<UIStateUpdateEventArgs> UIStateUpdateEventHandler;

    void Awake()
    {
        List<IUIPanel> uiPanels = FindObjectsOfType<MonoBehaviour>().OfType<IUIPanel>().ToList();

        foreach(var panel in uiPanels)
        {
            if(panel is IUIStateListener stateListener)
                UIStateUpdateEventHandler += stateListener.HandleUIStateUpdate;

            if (panel is IUIStateDispatcher stateDispatcher)
                stateDispatcher.UIStateUpdateEventHandler += HandleUIStateUpdate;
        }

        UpdateUIState(UIState.WORLD);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.Delete))
        {
            WorldObjectManager.Instance.WorldObjectClicked(null, MouseClickType.SINGLE);
            UpdateUIState(UIState.WORLD);
        }


    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
        if (args.objectModel == null) return;

        UIState state;
        if (args.objectModel.Type == WorldObjectType.LIGHT)
        {
            state = UIState.LIGHT_DATA;
        } else
        {
            state = UIState.SHAPE_DATA;
        }
        UpdateUIState(state);
    }

    public void HandleUIStateUpdate(object sender, UIStateUpdateEventArgs args)
    {
        UpdateUIState(args.state);
    }

    private void UpdateUIState(UIState state)
    {
        if(UIStateUpdateEventHandler != null)
            UIStateUpdateEventHandler(this, new UIStateUpdateEventArgs(state));
    }
}
