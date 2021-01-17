using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : MonoBehaviour
{
    protected List<Button> _buttons;

    void Awake()
    {
        _buttons = GetComponentsInChildren<Button>().ToList();
    }

    public virtual void HandleButtonClicked(Button button)
    {
        Debug.Log(string.Format("Button {0} Clicked", button.name));
    }
}
