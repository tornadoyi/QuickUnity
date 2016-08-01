using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_SymbolImage : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.SymbolImage o;
			o=new QuickUnity.SymbolImage();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetSymbolImage(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.SetSymbolImage(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				var ret=self.SetSymbolImage(a1,a2);
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
	static public int SetImage(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				var ret=self.SetImage(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				var ret=self.SetImage(a1,a2);
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
	static public int get_image(IntPtr l) {
		try {
			QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.image);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_image(IntPtr l) {
		try {
			QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
			UnityEngine.UI.Image v;
			checkType(l,2,out v);
			self.image=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_symbolImage(IntPtr l) {
		try {
			QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.symbolImage);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_symbolImage(IntPtr l) {
		try {
			QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.symbolImage=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_loadFinishEnable(IntPtr l) {
		try {
			QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.loadFinishEnable);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_loadFinishEnable(IntPtr l) {
		try {
			QuickUnity.SymbolImage self=(QuickUnity.SymbolImage)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.loadFinishEnable=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.SymbolImage");
		addMember(l,SetSymbolImage);
		addMember(l,SetImage);
		addMember(l,"image",get_image,set_image,true);
		addMember(l,"symbolImage",get_symbolImage,set_symbolImage,true);
		addMember(l,"loadFinishEnable",get_loadFinishEnable,set_loadFinishEnable,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.SymbolImage),typeof(QuickUnity.SymbolWidget));
	}
}
