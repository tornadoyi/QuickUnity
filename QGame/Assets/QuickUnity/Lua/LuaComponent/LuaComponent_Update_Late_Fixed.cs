using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class LuaComponent_Update_Late_Fixed : LuaComponent
    {

        void Update()
        {
            if (onUpdate != null) onUpdate.call(_self);
        }

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


