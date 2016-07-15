using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_FiniteAction : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Step(IntPtr l) {
		try {
			QuickUnity.Action.FiniteAction self=(QuickUnity.Action.FiniteAction)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			var ret=self.Step(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsDone(IntPtr l) {
		try {
			QuickUnity.Action.FiniteAction self=(QuickUnity.Action.FiniteAction)checkSelf(l);
			var ret=self.IsDone();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		addMember(l,Step);
		addMember(l,IsDone);
		createTypeMetatable(l,null, typeof(QuickUnity.Action.FiniteAction),typeof(QuickUnity.Action.ActionBase));
	}
}
