using UnityEngine;
using System.Collections;
using System;

public static class GameObject_Ext
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        var com = go.GetComponent<T>();
        if (com != null) return com;
        return go.AddComponent<T>();
    }

    public static Component GetOrAddComponent(this GameObject go, Type type)
    {
        var com = go.GetComponent(type);
        if (com != null) return com;
        return go.AddComponent(type);
    }

}
