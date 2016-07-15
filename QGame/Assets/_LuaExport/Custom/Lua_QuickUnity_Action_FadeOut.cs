using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_FadeOut : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.Action.FadeOut o;
			System.Single a1;
			checkType(l,2,out a1);
			o=new QuickUnity.Action.FadeOut(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.Action.FadeOut");
		createTypeMetatable(l,constructor, typeof(QuickUnity.Action.FadeOut),typeof(QuickUnity.Action.FadeTo));
	}
}
