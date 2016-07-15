using UnityEngine;
using System.Collections;

public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    protected BaseManager() { }
    protected virtual void Awake()
    {
        _instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }

    public static T instance
    {
        get
        {
            if (_instance == null) Debug.LogError(string.Format("Invalid manager({0}) access", typeof(T).Name));
            return _instance;
        }
    }
    private static T _instance;
}
