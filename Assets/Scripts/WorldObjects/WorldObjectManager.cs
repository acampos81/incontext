using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class WorldObjectManager : UnitySingleton<WorldObjectManager>
{
    private static Vector3 WORLD_OBJECT_START_POSITION = new Vector3(0f, 1f, 0f);

    public event EventHandler<WorldObjectSelectedEventArgs> WorldObjectSelectedEventHandler;

    public uint initialObjectPoolSize;

    private IWorldObjectView _selectedView;
    private Dictionary<WorldObjectType, ObjectPool> _objectPools;
    private Transform _worldObjectContainer;

    protected override void OnAwake()
    {
        MonoBehaviour[] sceneBehaviours = FindObjectsOfType<MonoBehaviour>(true);


        foreach (var behaviour in sceneBehaviours)
        {
            if(behaviour is ICreateObjectDispatcher createDispatcher)
                createDispatcher.CreateObjectEventHandler += HandleCreateObject;

            if (behaviour is IRemoveObjectDispatcher removeDispatcher)
                removeDispatcher.RemoveObjectEventHandler += HandleRemoveObject;

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

        objView.Model.Position = WORLD_OBJECT_START_POSITION;
    }

    private void HandleRemoveObject(object sender, EventArgs args)
    {
        if (_selectedView == null) return;

        RemoveViewInstance(_selectedView.Model.Type, ((WorldObjectViewBase)_selectedView).gameObject);
    }

    private IWorldObjectView AddViewInstance(WorldObjectType objectType)
    {
        var objInstance = _objectPools[objectType].Next();
        objInstance.transform.SetParent(_worldObjectContainer);
        return objInstance.GetComponent<IWorldObjectView>();
    }

    private void SetViewModel(IWorldObjectView objectView, WorldObjectType objectType)
    {
        IWorldObjectModel objectModel;

        if (objectType == WorldObjectType.LIGHT)
        {
            var lightView = (LightView)objectView;
            objectModel = new LightModel();
            objectModel.LocalCenterPoint = lightView.localCenterPoint;
            ((ILightModel)objectModel).Intensity = lightView.lightComponent.intensity;
            ((ILightModel)objectModel).ConeAngle = lightView.lightComponent.spotAngle;
        } else
        {
            var shapeView = (ShapeView)objectView;
            objectModel = new ShapeModel();
            objectModel.LocalCenterPoint = shapeView.localCenterPoint;
            ((IShapeModel)objectModel).Color = WorldObjectMaterials.Instance.ShapeDefaultColor;
        }

        objectModel.Type = objectType;
        objectModel.Name = objectType.ToString();
        objectModel.Position = Vector3.zero;
        objectModel.Rotation = Quaternion.identity;
        objectView.Model = objectModel;
    }

    public void RemoveViewInstance(WorldObjectType objectType, GameObject worldObject)
    {
        _objectPools[objectType].Store(worldObject);
    }

    public void WorldObjectClicked(IWorldObjectView worldObject, MouseClickType clickType)
    {
        if (_selectedView != null)
        {
            _selectedView.SetSelected(false);
            _selectedView = null;
        }

        IWorldObjectModel model = null;
        if(worldObject != null)
        {
            model = worldObject.Model;
            _selectedView = worldObject;
            _selectedView.SetSelected(true);
        }

        if (WorldObjectSelectedEventHandler != null)
            WorldObjectSelectedEventHandler(this, new WorldObjectSelectedEventArgs(model, clickType));
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
