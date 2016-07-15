using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_LuaComponentType : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"QuickUnity.LuaComponentType");
		addMember(l,0,"Common");
		addMember(l,1,"Update");
		addMember(l,2,"LateUpdate");
		addMember(l,3,"FixedUpdate");
		addMember(l,4,"Update_Fixed");
		addMember(l,5,"Update_Late");
		addMember(l,6,"Late_Fixed");
		addMember(l,7,"Update_Late_Fixed");
		LuaDLL.lua_pop(l, 1);
	}
}
