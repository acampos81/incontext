using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class WorldObjectManager : UnitySingleton<WorldObjectManager>
{
    public event EventHandler<WorldObjectSelectedEventArgs> WorldObjectSelectedEventHandler;

    public uint initialObjectPoolSize;

    private Dictionary<WorldObjectType, ObjectPool> _objectPools;
    private Transform _worldObjectContainer;

    protected override void OnAwake()
    {
        MonoBehaviour[] sceneBehaviours = FindObjectsOfType<MonoBehaviour>(true);


        foreach (var behaviour in sceneBehaviours)
        {
            if(behaviour is ICreateObjectDispatcher createDispatcher)
                createDispatcher.CreateObjectEventHandler += HandleCreateObject;

            if (behaviour is IWorldObjectSelectedListener selectedListener)
                WorldObjectSelectedEventHandler += selectedListener.HandleWorldObjectSelected;

            if (behaviour is IIOProcessDispatcher ioDispatcher)
                ioDispatcher.IOProcessEventHandler += HandleIOProcess;
        }

        _objectPools = new Dictionary<WorldObjectType, ObjectPool>();
        InitializePool(initialObjectPoolSize, WorldObjectType.SPHERE);
        InitializePool(initialObjectPoolSize, WorldObjectType.CUBE);
        InitializePool(initialObjectPoolSize, WorldObjectType.CYLINDER);
        InitializePool(initialObjectPoolSize, WorldObjectType.LIGHT);

        _worldObjectContainer = new GameObject("WorldObjects").transform;
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

        var objectName = objectType.ToString();
        var template = Resources.Load<GameObject>(resourcePath);
        var container = new GameObject(string.Format("{0} Pool", objectName)).transform;
        container.transform.position = Vector3.down * 20f;
        container.transform.SetParent(this.transform);
        container.gameObject.SetActive(false);

        _objectPools[objectType] = new ObjectPool(size, template, container, objectName);
    }

    private void HandleCreateObject(object sender, CreateObjectEventArgs args)
    {
        var objView = AddViewInstance(args.objectType);
        if (objView.Model == null)
            SetViewModel(objView, args.objectType);
    }

    private IWorldObjectView AddViewInstance(WorldObjectType objectType)
    {
        var objInstance = _objectPools[objectType].Next();
        objInstance.transform.SetParent(_worldObjectContainer);
        return objInstance.GetComponent<IWorldObjectView>();
    }

    private void SetViewModel(IWorldObjectView objectView, WorldObjectType objectType)
    {
        if (objectType == WorldObjectType.LIGHT)
        {
            var lightView = (LightView)objectView;
            var lightModel = new LightModel();
            lightModel.Type = objectType;
            lightModel.LocalCenterPoint = lightView.localCenterPoint;
            lightModel.Intensity = lightView.lightComponent.intensity;
            lightModel.ConeAngle = lightView.lightComponent.spotAngle;
            lightView.Model = lightModel;
        } else
        {
            var shapeView = (ShapeView)objectView;
            var shapeModel = new ShapeModel();
            shapeModel.Type = objectType;
            shapeModel.LocalCenterPoint = shapeView.localCenterPoint;
            shapeModel.Color = WorldObjectMaterials.Instance.ShapeDefaultColor;
            shapeView.Model = shapeModel;
        }
    }

    public void RemoveViewInstance(WorldObjectType objectType, GameObject worldObject)
    {
        _objectPools[objectType].Store(worldObject);
    }

    public void WorldObjectClicked(IWorldObjectView wordlObject, MouseClickType clickType)
    {
        if (WorldObjectSelectedEventHandler != null)
            WorldObjectSelectedEventHandler(this, new WorldObjectSelectedEventArgs(wordlObject.Model, clickType));
    }

    public void HandleIOProcess(object sender, IOProcessEventArgs args)
    {
        if(args.processType == IOProcessType.SAVE)
        {
            Save(args.filePath);
        }else if(args.processType == IOProcessType.LOAD)
        {
            Load(args.filePath);
        }
    }

    private void Save(string filePath)
    {
        var views = _worldObjectContainer.GetComponentsInChildren<IWorldObjectView>();
        var models = new IWorldObjectModel[views.Length];

        for(int i=0; i<views.Length; i++)
        {
            models[i] = views[i].Model;
        }

        IOProcessor.Save(filePath, models);
    }

    private void Load(string filePath)
    {
        JSONArray jsonArray = IOProcessor.Load(filePath);
        for(int i=0; i<jsonArray.Count; i++)
        {
            JSONNode node = jsonArray[i];
            IWorldObjectModel model;
            if(node["type"].Value == WorldObjectType.LIGHT.ToString())
            {
                model = new LightModel();
                
            }else
            {
                model = new ShapeModel();
            }
            model.FromJSON(node);

            var objInstace = AddViewInstance(model.Type);
            objInstace.Model = model;
        }
    }
}
