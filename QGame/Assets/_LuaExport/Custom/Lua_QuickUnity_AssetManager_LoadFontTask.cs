using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_LoadFontTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadFontTask o;
			System.String a1;
			checkType(l,2,out a1);
			o=new QuickUnity.AssetManager.LoadFontTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_font(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadFontTask self=(QuickUnity.AssetManager.LoadFontTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.font);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_font(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadFontTask self=(QuickUnity.AssetManager.LoadFontTask)checkSelf(l);
			UnityEngine.Font v;
			checkType(l,2,out v);
			self.font=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.LoadFontTask");
		addMember(l,"font",get_font,set_font,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.LoadFontTask),typeof(QuickUnity.AssetManager.LoadSpecifyAssetTask));
	}
}
