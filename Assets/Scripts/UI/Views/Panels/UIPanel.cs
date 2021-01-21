using System.Collections.Generic;
using UnityEngine;

public abstract class UIPanel : MonoBehaviour, IUIStateListener
{
    public List<UIState> activeStates;

    public void HandleUIStateUpdate(object sender, UIStateUpdateEventArgs args)
    {
        this.gameObject.SetActive(activeStates.Contains(args.state));
    }
}
