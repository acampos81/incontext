using UnityEngine;

public interface IMouseListener
{
    void HandleMouseButtonDown(MouseButton buttonn);
    void HandleMouseButtonUp(MouseButton button);
    void HandleScrolling(Vector2 scrollDelta);
}
