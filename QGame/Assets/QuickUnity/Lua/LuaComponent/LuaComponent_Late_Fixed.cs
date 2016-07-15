using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class LuaComponent_Late_Fixed : LuaComponent
    {
        void LateUpdate()
        {
            if (onLateUpdate != null) onLateUpdate.call(_self);
        }

        void FixedUpdate()
        {
            if (onFixedUpdate != null) onFixedUpdate.call(_self);
        }
    }
}


