using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_WWWReadTextTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			QuickUnity.WWWReadTextTask o;
			if(argc==2){
				System.String a1;
				checkType(l,2,out a1);
				o=new QuickUnity.WWWReadTextTask(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==3){
				System.String a1;
				checkType(l,2,out a1);
				System.Text.Encoding a2;
				checkType(l,3,out a2);
				o=new QuickUnity.WWWReadTextTask(a1,a2);
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
	static public int get_text(IntPtr l) {
		try {
			QuickUnity.WWWReadTextTask self=(QuickUnity.WWWReadTextTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.text);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_encoding(IntPtr l) {
		try {
			QuickUnity.WWWReadTextTask self=(QuickUnity.WWWReadTextTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.encoding);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.WWWReadTextTask");
		addMember(l,"text",get_text,null,true);
		addMember(l,"encoding",get_encoding,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.WWWReadTextTask),typeof(QuickUnity.WWWTask));
	}
}
