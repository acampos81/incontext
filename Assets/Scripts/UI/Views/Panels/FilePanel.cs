using System;
using UnityEngine;
using UnityEngine.UI;

public class FilePanel : ButtonPanel,
    IUIStateDispatcher,
    IIOProcessDispatcher
{
    public event EventHandler<UIStateUpdateEventArgs> UIStateUpdateEventHandler;
    public event EventHandler<IOProcessEventArgs> IOProcessEventHandler;

    public InputField inputField;
    public Text confirmButtonText;

    private IOProcessType _processType;

    void OnEnable()
    {
        if(_currentState == UIState.SAVE_PROMPT)
        {
            confirmButtonText.text = "Save";
            _processType = IOProcessType.SAVE;
        }else if(_currentState == UIState.LOAD_PROMPT)
        {
            confirmButtonText.text = "Load";
            _processType = IOProcessType.LOAD;
        }
    }

    public override void HandleButtonClicked(Button button)
    {
        if(button.transform.GetSiblingIndex() == 0)
        {
            var filePath = string.Format("{0}/{1}.json", Application.persistentDataPath, inputField.text);

            if (IOProcessEventHandler != null)
                IOProcessEventHandler(this, new IOProcessEventArgs(filePath, _processType));
        }

        if (UIStateUpdateEventHandler != null)
            UIStateUpdateEventHandler(this, new UIStateUpdateEventArgs(UIState.WORLD));
    }
}
