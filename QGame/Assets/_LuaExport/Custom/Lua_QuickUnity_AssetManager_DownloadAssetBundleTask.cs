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
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.DownloadAssetBundleTask");
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.DownloadAssetBundleTask),typeof(QuickUnity.CoroutineTask));
	}
}
