using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour, IUIPanel, IUIStateListener
{
    public List<UIState> activeStates;

    protected UIState _currentState;

    public void HandleUIStateUpdate(object sender, UIStateUpdateEventArgs args)
    {
        _currentState = args.state;
        this.gameObject.SetActive(activeStates.Contains(args.state));
    }
}
