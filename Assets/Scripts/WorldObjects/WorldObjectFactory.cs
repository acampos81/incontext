using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectFactory : Singleton<WorldObjectFactory>
{
    private Dictionary<WorldObjectType, ObjectPool> _objectPools;

    private void Awake()
    {
        _objectPools = new Dictionary<WorldObjectType, ObjectPool>();
    }

    public void InitializePool(uint size, WorldObjectType objectType)
    {
        string resourcePath = null;
        switch(objectType)
        {
            case WorldObjectType.SPHERE:
                resourcePath = "Prefabs/WorldObjects/SphereWO";
                break;
            case WorldObjectType.CUBE:
                resourcePath = "Prefabs/WorldObjects/CubeWO";
                break;
            case WorldObjectType.CYLINDER:
                resourcePath = "Prefabs/WorldObjects/CylinderWO";
                break;
            case WorldObjectType.LIGHT:
                resourcePath = "Prefabs/WorldObjects/LightWO";
                break;
            default:
                throw new ArgumentException("Cannot initialize pool for unknown type:" + objectType);
        }

        var template = Resources.Load<GameObject>(resourcePath);
        var objectName = objectType.ToString();
        var container = new GameObject(string.Format("{0} Pool",objectName)).transform;
        container.transform.position = Vector3.down * 20f;
        container.transform.SetParent(this.transform);
        container.gameObject.SetActive(false);
        _objectPools[objectType] = new ObjectPool(size, template, container, objectName);
    }

    public GameObject GetWorldObject(WorldObjectType objectType)
    {
        return _objectPools[objectType].Next();
    }
}