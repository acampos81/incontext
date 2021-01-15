public interface IKeyListener : IInputTypeListener
{
    void HandleKeyDown(AppKey appKey);
    void HandleKeyUp(AppKey appKey);
}
