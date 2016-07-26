using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_LoadGameObjectTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadGameObjectTask o;
			System.String a1;
			checkType(l,2,out a1);
			o=new QuickUnity.AssetManager.LoadGameObjectTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_gameObject(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadGameObjectTask self=(QuickUnity.AssetManager.LoadGameObjectTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.gameObject);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.LoadGameObjectTask");
		addMember(l,"gameObject",get_gameObject,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.LoadGameObjectTask),typeof(QuickUnity.AssetManager.LoadSpecifyAssetTask));
	}
}
