﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_FadeTo : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.Action.FadeTo o;
			System.Single a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			o=new QuickUnity.Action.FadeTo(a1,a2);
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
			QuickUnity.Action.FadeTo self=(QuickUnity.Action.FadeTo)checkSelf(l);
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
		getTypeTable(l,"QuickUnity.Action.FadeTo");
		addMember(l,Start);
		createTypeMetatable(l,constructor, typeof(QuickUnity.Action.FadeTo),typeof(QuickUnity.Action.IntervalAction));
	}
}
