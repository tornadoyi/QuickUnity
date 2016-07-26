using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_LoadSpriteTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadSpriteTask o;
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			o=new QuickUnity.AssetManager.LoadSpriteTask(a1,a2);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_sprite(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadSpriteTask self=(QuickUnity.AssetManager.LoadSpriteTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.sprite);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_sprite(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadSpriteTask self=(QuickUnity.AssetManager.LoadSpriteTask)checkSelf(l);
			UnityEngine.Sprite v;
			checkType(l,2,out v);
			self.sprite=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.LoadSpriteTask");
		addMember(l,"sprite",get_sprite,set_sprite,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.LoadSpriteTask),typeof(QuickUnity.AssetManager.LoadSpecifyAssetTask));
	}
}
