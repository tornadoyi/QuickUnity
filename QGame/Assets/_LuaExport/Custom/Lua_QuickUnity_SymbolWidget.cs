using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_SymbolWidget : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WaitForDone(IntPtr l) {
		try {
			QuickUnity.SymbolWidget self=(QuickUnity.SymbolWidget)checkSelf(l);
			var ret=self.WaitForDone();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_libraryName(IntPtr l) {
		try {
			QuickUnity.SymbolWidget self=(QuickUnity.SymbolWidget)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.libraryName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_libraryName(IntPtr l) {
		try {
			QuickUnity.SymbolWidget self=(QuickUnity.SymbolWidget)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.libraryName=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.SymbolWidget");
		addMember(l,WaitForDone);
		addMember(l,"libraryName",get_libraryName,set_libraryName,true);
		createTypeMetatable(l,null, typeof(QuickUnity.SymbolWidget),typeof(UnityEngine.MonoBehaviour));
	}
}
