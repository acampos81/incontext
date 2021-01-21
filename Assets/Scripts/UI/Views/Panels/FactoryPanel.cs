using System;
using UnityEngine.UI;

public class FactoryPanel : ButtonPanel, ICreateObjectDispatcher
{
    public event EventHandler<CreateObjectEventArgs> CreateObjectEventHandler;

    public override void HandleButtonClicked(Button button)
    {
        var siblingIndex = button.transform.GetSiblingIndex();
        var objectType = (WorldObjectType)siblingIndex;

        if(CreateObjectEventHandler != null)
            CreateObjectEventHandler(this, new CreateObjectEventArgs(objectType));
    }
}
