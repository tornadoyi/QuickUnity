using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_Repeat : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.Action.Repeat o;
			QuickUnity.Action.ActionBase a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			o=new QuickUnity.Action.Repeat(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start(IntPtr l) {
		try {
			QuickUnity.Action.Repeat self=(QuickUnity.Action.Repeat)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.Start(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Step(IntPtr l) {
		try {
			QuickUnity.Action.Repeat self=(QuickUnity.Action.Repeat)checkSelf(l);
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
			QuickUnity.Action.Repeat self=(QuickUnity.Action.Repeat)checkSelf(l);
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
		getTypeTable(l,"QuickUnity.Action.Repeat");
		addMember(l,Start);
		addMember(l,Step);
		addMember(l,IsDone);
		createTypeMetatable(l,constructor, typeof(QuickUnity.Action.Repeat),typeof(QuickUnity.Action.IntervalAction));
	}
}
