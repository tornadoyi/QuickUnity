using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class LuaComponent_LateUpdate : LuaComponent
    {
        void LateUpdate()
        {
            if (onLateUpdate != null) onLateUpdate.call(_self);
        }
    }
}


