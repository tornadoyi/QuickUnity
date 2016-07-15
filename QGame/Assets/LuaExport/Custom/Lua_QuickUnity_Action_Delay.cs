using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_Delay : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.Action.Delay o;
			System.Single a1;
			checkType(l,2,out a1);
			o=new QuickUnity.Action.Delay(a1);
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
		createTypeMetatable(l,constructor, typeof(QuickUnity.Action.Delay),typeof(QuickUnity.Action.IntervalAction));
	}
}
