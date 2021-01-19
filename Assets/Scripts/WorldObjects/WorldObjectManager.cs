using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectManager : UnitySingleton<WorldObjectManager>
{
    public event EventHandler<WorldObjectSelectedEventArgs> WorldObjectSelectedEventHandler;

    public uint initialObjectPoolSize;
    public List<WorldObjectMaterials> avaialbleMaterials;

    private Dictionary<WorldObjectType, ObjectPool> _objectPools;
    private Dictionary<Color, WorldObjectMaterials> _objectMaterials;

    protected override void OnAwake()
    {
        MonoBehaviour[] sceneBehaviours = FindObjectsOfType<MonoBehaviour>();


        foreach (var behaviour in sceneBehaviours)
        {
            if(behaviour is ICreateObjectDispatcher dispatcher)
                dispatcher.OnCreateObject += HandleCreateObject;

            if (behaviour is IWorldObjectSelectedListener listener)
                WorldObjectSelectedEventHandler += listener.HandleWorldObjectSelected;
        }

        InitializeMaterials();

        _objectPools = new Dictionary<WorldObjectType, ObjectPool>();
        InitializePool(initialObjectPoolSize, WorldObjectType.SPHERE);
        InitializePool(initialObjectPoolSize, WorldObjectType.CUBE);
        InitializePool(initialObjectPoolSize, WorldObjectType.CYLINDER);
        InitializePool(initialObjectPoolSize, WorldObjectType.LIGHT);
    }

    void InitializePool(uint size, WorldObjectType objectType)
    {
        string resourcePath = null;
        switch (objectType)
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
        var container = new GameObject(string.Format("{0} Pool", objectName)).transform;
        container.transform.position = Vector3.down * 20f;
        container.transform.SetParent(this.transform);
        container.gameObject.SetActive(false);
        _objectPools[objectType] = new ObjectPool(size, template, container, objectName);
    }

    void InitializeMaterials()
    {
        _objectMaterials = new Dictionary<Color, WorldObjectMaterials>();
        foreach (var objMat in avaialbleMaterials)
            _objectMaterials[objMat.color] = objMat;
    }

    private void HandleCreateObject(object sender, CreateObjectEventArgs args)
    {
        var worldObject = GetWorldObject(args.objectType);
    }

    public GameObject GetWorldObject(WorldObjectType objectType)
    {
        return _objectPools[objectType].Next();
    }

    public void RemoveWorldObject(WorldObjectType objectType, GameObject worldObject)
    {
        _objectPools[objectType].Store(worldObject);
    }

    public void WorldObjectClicked(IWorldObjectView wordlObject, MouseClickType clickType)
    {
        if (WorldObjectSelectedEventHandler != null)
            WorldObjectSelectedEventHandler(this, new WorldObjectSelectedEventArgs(wordlObject.GetModel(), clickType));
    }

    public Material GetIdleMaterial(Color color)
    {
        return _objectMaterials[color].diffuseMaterial;
    }

    public Material GetHighlightMaterial(Color color)
    {
        return _objectMaterials[color].highlightMaterial;
    }
}
