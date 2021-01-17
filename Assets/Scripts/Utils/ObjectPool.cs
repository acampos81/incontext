using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
	private Stack<GameObject> _objectStack;
	private GameObject _objectTemplate;
	private Transform _poolContainer;
	private string _instanceName;

	public ObjectPool(
		uint initialBufferSize,
		GameObject objectTemplate,
		Transform container,
		string instanceName=null)
	{
		_objectStack = new Stack<GameObject>();
		_objectTemplate = objectTemplate;
		_poolContainer = container;
		_instanceName = instanceName;
		for (uint i = 0; i < initialBufferSize; i++)
		{
			Store(NewInstance());
		}
	}

	public GameObject Next()
	{
		GameObject obj;
		if (_objectStack.Count > 0)
		{
			obj = _objectStack.Pop();
			obj.SetActive(true);
			obj.transform.SetParent(null);
		} else
		{
			obj = NewInstance();
		}
		
		return obj;
	}

	public void Store(GameObject obj)
	{
		if (!_objectStack.Contains(obj))
		{
			_objectStack.Push(obj);
		}
		obj.SetActive(false);
		obj.transform.SetParent(_poolContainer);
		obj.transform.position = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;
	}

	private GameObject NewInstance()
    {
		var obj = GameObject.Instantiate(this._objectTemplate);

		if(_instanceName != null)
			obj.name = _instanceName;

		return obj;
	}
}
