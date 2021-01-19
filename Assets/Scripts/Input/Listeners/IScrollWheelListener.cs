public interface IScrollWheelListener : IInputListener
{
    void HandleScrollWheel(object sender, ScrollWheelEventArgs args);
}
