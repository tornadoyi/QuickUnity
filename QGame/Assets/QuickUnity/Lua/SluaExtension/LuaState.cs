using UnityEngine;
using System.Collections;


namespace SLua
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using LuaInterface;
    using System.IO;
    using System.Text;
    using System.Runtime.InteropServices;
#if !SLUA_STANDALONE
    using UnityEngine;
#endif

    public partial class LuaFunction : LuaVar
    {
        // [merge] public object call(object a1)
        public object callExpectTableReturn(object a1)
        {
            int error = LuaObject.pushTry(state.L);

            LuaObject.pushVar(state.L, a1);
            if (innerCall(1, error))
            {
                return state.topTable(error - 1);
            }

            return null;
        }
    }


    public partial class LuaState : IDisposable
    {
        // [merge] internal object topObjects(int from)
        internal object topTable(int from)
        {
            int top = LuaDLL.lua_gettop(L);
            int nArgs = top - from;
            if (nArgs == 0)
                return null;
            else if (nArgs == 1)
            {
                object o = LuaObject.checkLuaTable(L, top);
                LuaDLL.lua_pop(L, 1);
                return o;
            }
            else
            {
                LuaDLL.lua_settop(L, from);
                return null;
            }
        }
    }
}

