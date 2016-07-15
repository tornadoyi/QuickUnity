using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class LuaComponent_FixedUpdate : LuaComponent
    {
        void FixedUpdate()
        {
            if (onFixedUpdate != null) onFixedUpdate.call(_self);
        }
    }
}


