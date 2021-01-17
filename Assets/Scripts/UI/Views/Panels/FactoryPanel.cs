using System;
using UnityEngine.UI;

public class FactoryPanel : ButtonPanel, ICreateObjectDispatcher
{
    public event EventHandler<CreateObjectEventArgs> OnCreateObject;

    public override void HandleButtonClicked(Button button)
    {
        var siblingIndex = button.transform.GetSiblingIndex();
        var objectType = (WorldObjectType)siblingIndex;

        if(OnCreateObject != null)
            OnCreateObject(this, new CreateObjectEventArgs(objectType));
    }
}
