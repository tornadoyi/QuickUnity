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
	static public int SetSuccess(IntPtr l) {
		try {
			QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
			self.SetSuccess();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetFail(IntPtr l) {
		try {
			QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetFail(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetCancel(IntPtr l) {
		try {
			QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetCancel(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetTimeout(IntPtr l) {
		try {
			QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.SetTimeout(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.CustomTask");
		addMember(l,SetSuccess);
		addMember(l,SetFail);
		addMember(l,SetCancel);
		addMember(l,SetTimeout);
		createTypeMetatable(l,constructor, typeof(QuickUnity.CustomTask),typeof(QuickUnity.Task));
	}
}
