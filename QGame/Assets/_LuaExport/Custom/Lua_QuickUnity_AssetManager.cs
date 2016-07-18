using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AssetManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AssetManager o;
			o=new QuickUnity.AssetManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			System.String a4;
			checkType(l,4,out a4);
			System.String a5;
			checkType(l,5,out a5);
			var ret=QuickUnity.AssetManager.Start(a1,a2,a3,a4,a5);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DownloadAssetBundle_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.DownloadAssetBundle(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadAssetBundle_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadAssetBundle(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadAsset_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadAsset(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadSubAssets_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadSubAssets(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadAssetAsync_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadAssetAsync(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Instantiate_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.Instantiate(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnloadUnusedResources_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==0){
				var ret=QuickUnity.AssetManager.UnloadUnusedResources();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==1){
				QuickUnity.AssetUnloadLevel a1;
				checkEnum(l,1,out a1);
				var ret=QuickUnity.AssetManager.UnloadUnusedResources(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetAssetBundle_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.GetAssetBundle(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetAssetBundleKeepTag_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			QuickUnity.AssetManager.SetAssetBundleKeepTag(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetAssetKeepTag_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Boolean a2;
			checkType(l,2,out a2);
			QuickUnity.AssetManager.SetAssetKeepTag(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRuntimeInfo_s(IntPtr l) {
		try {
			Dictionary<System.String,QuickUnity.AssetBundleInfo> a1;
			Dictionary<System.String,QuickUnity.AssetInfo> a2;
			QuickUnity.AssetManager.GetRuntimeInfo(out a1,out a2);
			pushValue(l,true);
			pushValue(l,a1);
			pushValue(l,a2);
			return 3;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadAudioClip_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadAudioClip(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadAudioClipAsync_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadAudioClipAsync(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadFont_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadFont(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadFontAsync_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadFontAsync(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadSprite_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.String a1;
				checkType(l,1,out a1);
				var ret=QuickUnity.AssetManager.LoadSprite(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=QuickUnity.AssetManager.LoadSprite(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadSpriteAsync_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				System.String a1;
				checkType(l,1,out a1);
				var ret=QuickUnity.AssetManager.LoadSpriteAsync(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				System.String a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=QuickUnity.AssetManager.LoadSpriteAsync(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadTexture_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadTexture(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadTextureAsync_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadTextureAsync(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadText_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadText(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadTextAsync_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadTextAsync(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadBinary_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadBinary(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadBinaryAsync_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadBinaryAsync(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadGameObject_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadGameObject(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LoadGameObjectAsync_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.AssetManager.LoadGameObjectAsync(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_streamingAssetsPath(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,QuickUnity.AssetManager.streamingAssetsPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_serverAssetPath(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,QuickUnity.AssetManager.serverAssetPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_downloadUrl(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,QuickUnity.AssetManager.downloadUrl);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_init(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,QuickUnity.AssetManager.init);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AssetManager");
		addMember(l,Start_s);
		addMember(l,DownloadAssetBundle_s);
		addMember(l,LoadAssetBundle_s);
		addMember(l,LoadAsset_s);
		addMember(l,LoadSubAssets_s);
		addMember(l,LoadAssetAsync_s);
		addMember(l,Instantiate_s);
		addMember(l,UnloadUnusedResources_s);
		addMember(l,GetAssetBundle_s);
		addMember(l,SetAssetBundleKeepTag_s);
		addMember(l,SetAssetKeepTag_s);
		addMember(l,GetRuntimeInfo_s);
		addMember(l,LoadAudioClip_s);
		addMember(l,LoadAudioClipAsync_s);
		addMember(l,LoadFont_s);
		addMember(l,LoadFontAsync_s);
		addMember(l,LoadSprite_s);
		addMember(l,LoadSpriteAsync_s);
		addMember(l,LoadTexture_s);
		addMember(l,LoadTextureAsync_s);
		addMember(l,LoadText_s);
		addMember(l,LoadTextAsync_s);
		addMember(l,LoadBinary_s);
		addMember(l,LoadBinaryAsync_s);
		addMember(l,LoadGameObject_s);
		addMember(l,LoadGameObjectAsync_s);
		addMember(l,"streamingAssetsPath",get_streamingAssetsPath,null,false);
		addMember(l,"serverAssetPath",get_serverAssetPath,null,false);
		addMember(l,"downloadUrl",get_downloadUrl,null,false);
		addMember(l,"init",get_init,null,false);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AssetManager),typeof(BaseManager<QuickUnity.AssetManager>));
	}
}
