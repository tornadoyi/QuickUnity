using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    public class LuaComponent_Update : LuaComponent
    {
        void Update()
        {
            if (onUpdate != null) onUpdate.call(_self);
        }
    }
}


