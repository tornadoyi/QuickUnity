namespace SLua
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System;
	using LuaInterface;
	using System.Reflection;
	using System.Runtime.InteropServices;
    using QuickUnity;

	public partial class LuaObject
    {
		public static void pushValue(IntPtr l, QBytes bytes)
		{
			LuaDLL.lua_pushlstring(l, bytes.data, bytes.length);
		}
    }
}
