using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_LoadBinaryTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadBinaryTask o;
			System.String a1;
			checkType(l,2,out a1);
			o=new QuickUnity.AssetManager.LoadBinaryTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_bytes(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadBinaryTask self=(QuickUnity.AssetManager.LoadBinaryTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bytes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_bytes(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadBinaryTask self=(QuickUnity.AssetManager.LoadBinaryTask)checkSelf(l);
			System.Byte[] v;
			checkArray(l,2,out v);
			self.bytes=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.LoadBinaryTask");
		addMember(l,"bytes",get_bytes,set_bytes,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.LoadBinaryTask),typeof(QuickUnity.AssetManager.LoadSpecifyAssetTask));
	}
}
