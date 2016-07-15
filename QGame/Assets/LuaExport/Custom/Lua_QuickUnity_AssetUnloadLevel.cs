using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetUnloadLevel : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"QuickUnity");
		addMember(l,1,"AssetBundles");
		addMember(l,2,"Assets");
		addMember(l,3,"Default");
		addMember(l,3,"All");
		LuaDLL.lua_pop(l, 1);
	}
}
