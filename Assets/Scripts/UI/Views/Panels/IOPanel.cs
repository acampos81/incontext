using System;
using UnityEngine;
using UnityEngine.UI;

public class IOPanel : ButtonPanel, IIOProcessDispatcher
{
    public event EventHandler<IOProcessEventArgs> IOProcessEventHandler;

    public override void HandleButtonClicked(Button button)
    {
        var siblingIndex = button.transform.GetSiblingIndex();
        var processType = (IOProcessType)siblingIndex;
        var filePath = string.Format("{0}/{1}.json", Application.persistentDataPath, "yodawg");

        if (IOProcessEventHandler != null)
            IOProcessEventHandler(this, new IOProcessEventArgs(filePath, processType));
    }
}
