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
			System.Boolean a1;
			checkType(l,2,out a1);
			o=new QuickUnity.CustomTask(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
				var ret=self.Start();
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==2){
				QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
				QuickUnity.CustomTask.OnStartCallback a1;
				LuaDelegation.checkDelegate(l,2,out a1);
				var ret=self.Start(a1);
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
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_finishAfterResult(IntPtr l) {
		try {
			QuickUnity.CustomTask self=(QuickUnity.CustomTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.finishAfterResult);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.CustomTask");
		addMember(l,Start);
		addMember(l,SetSuccess);
		addMember(l,SetFail);
		addMember(l,SetCancel);
		addMember(l,SetTimeout);
		addMember(l,"finishAfterResult",get_finishAfterResult,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.CustomTask),typeof(QuickUnity.Task));
	}
}
