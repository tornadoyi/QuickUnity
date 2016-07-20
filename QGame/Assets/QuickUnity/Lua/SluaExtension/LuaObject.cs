using UnityEngine;
using System.Collections;

namespace SLua
{

    #if !SLUA_STANDALONE
    using UnityEngine;
    #endif
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using LuaInterface;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public partial class LuaObject
    {
        protected static void addMember(IntPtr l, string name, LuaCSFunction func, bool instance)
        {
            checkMethodValid(func);

            pushValue(l, func);
            LuaDLL.lua_setfield(l, instance ? -2 : -3, name);
        }
    }
}