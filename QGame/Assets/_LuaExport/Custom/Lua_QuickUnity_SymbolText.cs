using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_SymbolText : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.SymbolText o;
			o=new QuickUnity.SymbolText();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetSymbolText(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				QuickUnity.SymbolText self=(QuickUnity.SymbolText)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				self.SetSymbolText(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				QuickUnity.SymbolText self=(QuickUnity.SymbolText)checkSelf(l);
				System.String a1;
				checkType(l,2,out a1);
				System.String a2;
				checkType(l,3,out a2);
				self.SetSymbolText(a1,a2);
				pushValue(l,true);
				return 1;
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
	static public int SetText(IntPtr l) {
		try {
			QuickUnity.SymbolText self=(QuickUnity.SymbolText)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetText(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_text(IntPtr l) {
		try {
			QuickUnity.SymbolText self=(QuickUnity.SymbolText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.text);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_text(IntPtr l) {
		try {
			QuickUnity.SymbolText self=(QuickUnity.SymbolText)checkSelf(l);
			UnityEngine.UI.Text v;
			checkType(l,2,out v);
			self.text=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_symbolText(IntPtr l) {
		try {
			QuickUnity.SymbolText self=(QuickUnity.SymbolText)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.symbolText);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_symbolText(IntPtr l) {
		try {
			QuickUnity.SymbolText self=(QuickUnity.SymbolText)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.symbolText=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.SymbolText");
		addMember(l,SetSymbolText);
		addMember(l,SetText);
		addMember(l,"text",get_text,set_text,true);
		addMember(l,"symbolText",get_symbolText,set_symbolText,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.SymbolText),typeof(QuickUnity.SymbolWidget));
	}
}
