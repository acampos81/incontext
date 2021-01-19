public interface IInputContextListener : IInputListener
{
    void HandleInputContext(object sender, InputContextEventArgs args);
}
