using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_LuaComponent_FixedUpdate : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.LuaComponent_FixedUpdate o;
			o=new QuickUnity.LuaComponent_FixedUpdate();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.LuaComponent_FixedUpdate");
		createTypeMetatable(l,constructor, typeof(QuickUnity.LuaComponent_FixedUpdate),typeof(QuickUnity.LuaComponent));
	}
}
