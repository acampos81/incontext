using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectManager : MonoBehaviour
{
    public uint initialObjectPoolSize;

    private List<ICreateObjectDispatcher> _creationDispatchers;

    void Awake()
    {
        _creationDispatchers = FindObjectsOfType<MonoBehaviour>().OfType<ICreateObjectDispatcher>().ToList();
        foreach(ICreateObjectDispatcher dispatcher in _creationDispatchers)
        {
            dispatcher.OnCreateObject += HandleCreateObject;
        }

        WorldObjectFactory.Instance.InitializePool(initialObjectPoolSize, WorldObjectType.SPHERE);
        WorldObjectFactory.Instance.InitializePool(initialObjectPoolSize, WorldObjectType.CUBE);
        WorldObjectFactory.Instance.InitializePool(initialObjectPoolSize, WorldObjectType.CYLINDER);
        WorldObjectFactory.Instance.InitializePool(initialObjectPoolSize, WorldObjectType.LIGHT);
    }

    private void Start()
    {

    }

    private void HandleCreateObject(object sender, CreateObjectEventArgs args)
    {
        var worldObject = WorldObjectFactory.Instance.GetWorldObject(args.objectType);
    }
}
