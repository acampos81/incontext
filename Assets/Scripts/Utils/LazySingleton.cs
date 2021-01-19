using System;
using UnityEngine;

public abstract class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);

    public static T Instance { get { return LazyInstance.Value; } }

    private static T CreateSingleton()
    {
        var ownerObject = new GameObject(string.Format("{0} (Singleton)", typeof(T).Name));
        var instance = ownerObject.AddComponent<T>();
        return instance;
    }
}
