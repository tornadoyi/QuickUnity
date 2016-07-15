﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_EventSystems_UIBehaviour : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsActive(IntPtr l) {
		try {
			UnityEngine.EventSystems.UIBehaviour self=(UnityEngine.EventSystems.UIBehaviour)checkSelf(l);
			var ret=self.IsActive();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsDestroyed(IntPtr l) {
		try {
			UnityEngine.EventSystems.UIBehaviour self=(UnityEngine.EventSystems.UIBehaviour)checkSelf(l);
			var ret=self.IsDestroyed();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.EventSystems.UIBehaviour");
		addMember(l,IsActive);
		addMember(l,IsDestroyed);
		createTypeMetatable(l,null, typeof(UnityEngine.EventSystems.UIBehaviour),typeof(UnityEngine.MonoBehaviour));
	}
}
