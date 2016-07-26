using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_LoadAssetBundleTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadAssetBundleTask o;
			System.String a1;
			checkType(l,2,out a1);
			o=new QuickUnity.AssetManager.LoadAssetBundleTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_asset(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadAssetBundleTask self=(QuickUnity.AssetManager.LoadAssetBundleTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.asset);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_subAssets(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadAssetBundleTask self=(QuickUnity.AssetManager.LoadAssetBundleTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.subAssets);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_name(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadAssetBundleTask self=(QuickUnity.AssetManager.LoadAssetBundleTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.LoadAssetBundleTask");
		addMember(l,"asset",get_asset,null,true);
		addMember(l,"subAssets",get_subAssets,null,true);
		addMember(l,"name",get_name,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.LoadAssetBundleTask),typeof(QuickUnity.CoroutineTask));
	}
}
