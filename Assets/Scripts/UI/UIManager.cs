using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IWorldObjectSelectedListener
{
    public event EventHandler<UIStateUpdateEventArgs> UIStateUpdateEventHandler;

    public List<StatePanels> panelsByState;

    void Awake()
    {
        List<IUIStateListener> stateListeners = FindObjectsOfType<MonoBehaviour>().OfType<IUIStateListener>().ToList();

        foreach(var stateListener in stateListeners)
        {
            UIStateUpdateEventHandler += stateListener.HandleUIStateUpdate;
        }

        UpdateUIState(UIState.WORLD);
    }

    public void HandleWorldObjectSelected(object sender, WorldObjectSelectedEventArgs args)
    {
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

    private void UpdateUIState(UIState state)
    {
        if(UIStateUpdateEventHandler != null)
            UIStateUpdateEventHandler(this, new UIStateUpdateEventArgs(state));
    }
}
