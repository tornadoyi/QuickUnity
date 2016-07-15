using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_LoadTextureTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadTextureTask o;
			System.String a1;
			checkType(l,2,out a1);
			o=new QuickUnity.AssetManager.LoadTextureTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_texture(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadTextureTask self=(QuickUnity.AssetManager.LoadTextureTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.texture);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_texture(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadTextureTask self=(QuickUnity.AssetManager.LoadTextureTask)checkSelf(l);
			UnityEngine.Texture2D v;
			checkType(l,2,out v);
			self.texture=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.LoadTextureTask");
		addMember(l,"texture",get_texture,set_texture,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.LoadTextureTask),typeof(QuickUnity.AssetManager.LoadSpecifyAssetTask));
	}
}
