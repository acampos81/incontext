public interface IMouseListener
{
    void HandleMouseButtonDown(MouseButton buttonn);
    void HandleMouseButtonUp(MouseButton button);
    void HandleScrolling(float scrollDelta);
}
