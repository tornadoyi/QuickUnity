using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_QuickManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.QuickManager o;
			o=new QuickUnity.QuickManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start_s(IntPtr l) {
		try {
			QuickUnity.QuickManager.Start();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Destory_s(IntPtr l) {
		try {
			QuickUnity.QuickManager.Destory();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_isInit(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,QuickUnity.QuickManager.isInit);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_gameObject(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,QuickUnity.QuickManager.gameObject);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.QuickManager");
		addMember(l,Start_s);
		addMember(l,Destory_s);
		addMember(l,"isInit",get_isInit,null,false);
		addMember(l,"gameObject",get_gameObject,null,false);
		createTypeMetatable(l,constructor, typeof(QuickUnity.QuickManager));
	}
}
