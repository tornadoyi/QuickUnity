using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_Blink : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.Action.Blink o;
			System.Single a1;
			checkType(l,2,out a1);
			System.UInt32 a2;
			checkType(l,3,out a2);
			o=new QuickUnity.Action.Blink(a1,a2);
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
			QuickUnity.Action.Blink self=(QuickUnity.Action.Blink)checkSelf(l);
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
	static public int Stop(IntPtr l) {
		try {
			QuickUnity.Action.Blink self=(QuickUnity.Action.Blink)checkSelf(l);
			self.Stop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.Action.Blink");
		addMember(l,Start);
		addMember(l,Stop);
		createTypeMetatable(l,constructor, typeof(QuickUnity.Action.Blink),typeof(QuickUnity.Action.IntervalAction));
	}
}
