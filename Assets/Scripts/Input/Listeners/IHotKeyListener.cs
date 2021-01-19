public interface IHotKeyListener : IInputListener
{
    void HandleHotKeyState(object sender, HotKeyEventArgs args);
}
