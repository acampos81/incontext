public interface IKeyListener : IInputTypeListener
{
    void HandleKeyDown(HotKey appKey);
    void HandleKeyUp(HotKey appKey);
}
