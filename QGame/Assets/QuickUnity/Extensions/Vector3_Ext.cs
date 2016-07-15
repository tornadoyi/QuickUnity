using UnityEngine;
using System.Collections;

public static class Vector3_Ext
{

    public static Vector3 Reverse(this Vector3 obj)
    {
        return new Vector3(-obj.x, -obj.y, -obj.z);
    }
}


