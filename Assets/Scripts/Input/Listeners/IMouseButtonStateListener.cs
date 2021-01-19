public interface IMouseButtonStateListener : IInputListener
{
    void HandleMouseButtonState(object sender, MouseButtonStateEventArgs args);
}
