using UnityEngine;

public interface IKeyListener : IInputTypeListener
{
    void HandleKeyDown(KeyCode keyCode);
    void HandleKeyUp(KeyCode keyCode);
}
