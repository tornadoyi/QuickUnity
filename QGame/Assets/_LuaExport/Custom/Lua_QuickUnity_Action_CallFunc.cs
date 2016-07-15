using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_CallFunc : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.Action.CallFunc o;
			QuickUnity.Action.CallFunc.Callback a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			o=new QuickUnity.Action.CallFunc(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.Action.CallFunc");
		createTypeMetatable(l,constructor, typeof(QuickUnity.Action.CallFunc),typeof(QuickUnity.Action.FiniteAction));
	}
}
