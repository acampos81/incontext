using System;
using UnityEngine.UI;

public class IOPanel : ButtonPanel, IUIStateDispatcher
{
    public event EventHandler<UIStateUpdateEventArgs> UIStateUpdateEventHandler;

    public override void HandleButtonClicked(Button button)
    {
        var sibIndex = button.transform.GetSiblingIndex();
        var state = sibIndex == 0 ? UIState.SAVE_PROMPT : UIState.LOAD_PROMPT;

        if (UIStateUpdateEventHandler != null)
            UIStateUpdateEventHandler(this, new UIStateUpdateEventArgs(state));
    }
}
