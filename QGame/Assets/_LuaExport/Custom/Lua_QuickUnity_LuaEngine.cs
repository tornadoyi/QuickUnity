using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_LuaEngine : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.LuaEngine o;
			o=new QuickUnity.LuaEngine();
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
			var ret=QuickUnity.LuaEngine.Start();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DoFile_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			QuickUnity.LuaEngine.DoFile(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetGlobalObject_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.LuaEngine.GetGlobalObject(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PushLuaLoader_s(IntPtr l) {
		try {
			QuickUnity.LuaEngine.LuaLoaderDelegate a1;
			LuaDelegation.checkDelegate(l,1,out a1);
			QuickUnity.LuaEngine.PushLuaLoader(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int PopLuaLoader_s(IntPtr l) {
		try {
			QuickUnity.LuaEngine.PopLuaLoader();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_inited(IntPtr l) {
		try {
			QuickUnity.LuaEngine self=(QuickUnity.LuaEngine)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.inited);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.LuaEngine");
		addMember(l,Start_s);
		addMember(l,DoFile_s);
		addMember(l,GetGlobalObject_s);
		addMember(l,PushLuaLoader_s);
		addMember(l,PopLuaLoader_s);
		addMember(l,"inited",get_inited,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.LuaEngine),typeof(QuickUnity.BaseManager<QuickUnity.LuaEngine>));
	}
}
