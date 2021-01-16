using UnityEngine;

public abstract class InputListenerBase : MonoBehaviour, IInputListener
{
    private IInputDispatcher _dispatcher;
    protected IInputDispatcher Dispatcher
    {
        get { return _dispatcher; }
    }

    public void RegisterDispatcher(IInputDispatcher dispatcher)
    {
        if (_dispatcher != null)
            DeregisterEvents();

        _dispatcher = dispatcher;
        RegisterEvents();
    }

    protected abstract void RegisterEvents();
    protected abstract void DeregisterEvents();

    void OnEnable()
    {
        RegisterEvents();
    }

    void OnDisable()
    {
        DeregisterEvents();
    }
}
