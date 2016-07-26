using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_QBytes : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			QuickUnity.QBytes o;
			if(matchType(l,argc,2,typeof(string))){
				System.String a1;
				checkType(l,2,out a1);
				o=new QuickUnity.QBytes(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(System.Byte[]))){
				System.Byte[] a1;
				checkArray(l,2,out a1);
				o=new QuickUnity.QBytes(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_data(IntPtr l) {
		try {
			QuickUnity.QBytes self=(QuickUnity.QBytes)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.data);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_length(IntPtr l) {
		try {
			QuickUnity.QBytes self=(QuickUnity.QBytes)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.length);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.QBytes");
		addMember(l,"data",get_data,null,true);
		addMember(l,"length",get_length,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.QBytes));
	}
}
