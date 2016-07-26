using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_LoadTextTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadTextTask o;
			System.String a1;
			checkType(l,2,out a1);
			o=new QuickUnity.AssetManager.LoadTextTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_text(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadTextTask self=(QuickUnity.AssetManager.LoadTextTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.text);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_text(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadTextTask self=(QuickUnity.AssetManager.LoadTextTask)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.text=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.LoadTextTask");
		addMember(l,"text",get_text,set_text,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.LoadTextTask),typeof(QuickUnity.AssetManager.LoadSpecifyAssetTask));
	}
}
