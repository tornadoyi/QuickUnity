using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager_LoadAudioClipTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadAudioClipTask o;
			System.String a1;
			checkType(l,2,out a1);
			o=new QuickUnity.AssetManager.LoadAudioClipTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_clip(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadAudioClipTask self=(QuickUnity.AssetManager.LoadAudioClipTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.clip);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_clip(IntPtr l) {
		try {
			QuickUnity.AssetManager.LoadAudioClipTask self=(QuickUnity.AssetManager.LoadAudioClipTask)checkSelf(l);
			UnityEngine.AudioClip v;
			checkType(l,2,out v);
			self.clip=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager.LoadAudioClipTask");
		addMember(l,"clip",get_clip,set_clip,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager.LoadAudioClipTask),typeof(QuickUnity.AssetManager.LoadSpecifyAssetTask));
	}
}
