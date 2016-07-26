using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_RotateAround : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.Action.RotateAround o;
			System.Single a1;
			checkType(l,2,out a1);
			UnityEngine.Vector3 a2;
			checkType(l,3,out a2);
			UnityEngine.Vector3 a3;
			checkType(l,4,out a3);
			System.Single a4;
			checkType(l,5,out a4);
			o=new QuickUnity.Action.RotateAround(a1,a2,a3,a4);
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
			QuickUnity.Action.RotateAround self=(QuickUnity.Action.RotateAround)checkSelf(l);
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
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.Action.RotateAround");
		addMember(l,Start);
		createTypeMetatable(l,constructor, typeof(QuickUnity.Action.RotateAround),typeof(QuickUnity.Action.IntervalAction));
	}
}
