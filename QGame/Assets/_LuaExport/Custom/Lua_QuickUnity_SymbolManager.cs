using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_SymbolManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.SymbolManager o;
			o=new QuickUnity.SymbolManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateLibrary_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.SymbolManager.CreateLibrary(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int DeleteLibrary_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			QuickUnity.SymbolManager.DeleteLibrary(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ContainLibrary_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.SymbolManager.ContainLibrary(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ContainSymbol_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			var ret=QuickUnity.SymbolManager.ContainSymbol(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddSymbol_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			QuickUnity.SymbolManager.AddSymbol(a1,a2,a3);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddSymbols_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.Collections.Generic.Dictionary<System.String,System.String> a2;
			checkType(l,2,out a2);
			QuickUnity.SymbolManager.AddSymbols(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveSymbol_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			QuickUnity.SymbolManager.RemoveSymbol(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Translate_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			var ret=QuickUnity.SymbolManager.Translate(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Translates_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			System.String a2;
			checkType(l,2,out a2);
			var ret=QuickUnity.SymbolManager.Translates(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLibrary_s(IntPtr l) {
		try {
			System.String a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.SymbolManager.GetLibrary(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetLibraryNames_s(IntPtr l) {
		try {
			var ret=QuickUnity.SymbolManager.GetLibraryNames();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NotifyUpdateSymbols_s(IntPtr l) {
		try {
			QuickUnity.SymbolManager.NotifyUpdateSymbols();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.SymbolManager");
		addMember(l,CreateLibrary_s);
		addMember(l,DeleteLibrary_s);
		addMember(l,ContainLibrary_s);
		addMember(l,ContainSymbol_s);
		addMember(l,AddSymbol_s);
		addMember(l,AddSymbols_s);
		addMember(l,RemoveSymbol_s);
		addMember(l,Translate_s);
		addMember(l,Translates_s);
		addMember(l,GetLibrary_s);
		addMember(l,GetLibraryNames_s);
		addMember(l,NotifyUpdateSymbols_s);
		createTypeMetatable(l,constructor, typeof(QuickUnity.SymbolManager),typeof(QuickUnity.Singleton<QuickUnity.SymbolManager>));
	}
}
