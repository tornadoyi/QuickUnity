using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_CustomTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.CustomTask o;
			o=new QuickUnity.CustomTask();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetResultFailed(IntPtr l) {
		try {
			QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetResultFailed(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Done(IntPtr l) {
		try {
			QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
			self.Done();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Error(IntPtr l) {
		try {
			QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Error(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		addMember(l,SetResultFailed);
		addMember(l,Done);
		addMember(l,Error);
		createTypeMetatable(l,constructor, typeof(QuickUnity.CustomTask),typeof(QuickUnity.Task));
	}
}
