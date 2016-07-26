using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_LuaComponent_Update_Late : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.LuaComponent_Update_Late o;
			o=new QuickUnity.LuaComponent_Update_Late();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.LuaComponent_Update_Late");
		createTypeMetatable(l,constructor, typeof(QuickUnity.LuaComponent_Update_Late),typeof(QuickUnity.LuaComponent));
	}
}
