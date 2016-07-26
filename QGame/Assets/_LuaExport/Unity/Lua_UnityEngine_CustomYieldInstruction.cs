﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_CustomYieldInstruction : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MoveNext(IntPtr l) {
		try {
			UnityEngine.CustomYieldInstruction self=(UnityEngine.CustomYieldInstruction)checkSelf(l);
			var ret=self.MoveNext();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reset(IntPtr l) {
		try {
			UnityEngine.CustomYieldInstruction self=(UnityEngine.CustomYieldInstruction)checkSelf(l);
			self.Reset();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_keepWaiting(IntPtr l) {
		try {
			UnityEngine.CustomYieldInstruction self=(UnityEngine.CustomYieldInstruction)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.keepWaiting);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Current(IntPtr l) {
		try {
			UnityEngine.CustomYieldInstruction self=(UnityEngine.CustomYieldInstruction)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Current);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.CustomYieldInstruction");
		addMember(l,MoveNext);
		addMember(l,Reset);
		addMember(l,"keepWaiting",get_keepWaiting,null,true);
		addMember(l,"Current",get_Current,null,true);
		createTypeMetatable(l,null, typeof(UnityEngine.CustomYieldInstruction));
	}
}
