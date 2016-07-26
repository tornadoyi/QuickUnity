using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_DownloadAssetBundleTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.DownloadAssetBundleTask o;
			System.String a1;
			checkType(l,2,out a1);
			o=new QuickUnity.AssetManager.DownloadAssetBundleTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_name(IntPtr l) {
		try {
			QuickUnity.AssetManager.DownloadAssetBundleTask self=(QuickUnity.AssetManager.DownloadAssetBundleTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.name);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.DownloadAssetBundleTask");
		addMember(l,"name",get_name,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.DownloadAssetBundleTask),typeof(QuickUnity.CoroutineTask));
	}
}
