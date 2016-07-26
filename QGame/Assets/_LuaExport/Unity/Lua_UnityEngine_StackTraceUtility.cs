﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_StackTraceUtility : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.StackTraceUtility o;
			o=new UnityEngine.StackTraceUtility();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExtractStackTrace_s(IntPtr l) {
		try {
			var ret=UnityEngine.StackTraceUtility.ExtractStackTrace();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ExtractStringFromException_s(IntPtr l) {
		try {
			System.Object a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.StackTraceUtility.ExtractStringFromException(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.StackTraceUtility");
		addMember(l,ExtractStackTrace_s);
		addMember(l,ExtractStringFromException_s);
		createTypeMetatable(l,constructor, typeof(UnityEngine.StackTraceUtility));
	}
}
