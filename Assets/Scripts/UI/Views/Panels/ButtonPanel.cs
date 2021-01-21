using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class ButtonPanel : UIPanel
{
    protected List<Button> _buttons;

    void Awake()
    {
        _buttons = GetComponentsInChildren<Button>().ToList();
    }

    public abstract void HandleButtonClicked(Button button);
}
