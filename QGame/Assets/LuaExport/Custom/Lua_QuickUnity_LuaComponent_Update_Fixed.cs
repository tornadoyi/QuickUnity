﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_LuaComponent_Update_Fixed : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.LuaComponent_Update_Fixed o;
			o=new QuickUnity.LuaComponent_Update_Fixed();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		createTypeMetatable(l,constructor, typeof(QuickUnity.LuaComponent_Update_Fixed),typeof(QuickUnity.LuaComponent));
	}
}
