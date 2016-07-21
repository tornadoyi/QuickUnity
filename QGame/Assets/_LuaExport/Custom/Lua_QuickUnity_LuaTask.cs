using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_LuaTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.LuaTask o;
			o=new QuickUnity.LuaTask();
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
			QuickUnity.LuaTask self=(QuickUnity.LuaTask)checkSelf(l);
			QuickUnity.CustomTask.OnStartCallback a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.Start(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetFinish(IntPtr l) {
		try {
			QuickUnity.LuaTask self=(QuickUnity.LuaTask)checkSelf(l);
			self.SetFinish();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.LuaTask");
		addMember(l,Start);
		addMember(l,SetFinish);
		createTypeMetatable(l,constructor, typeof(QuickUnity.LuaTask),typeof(QuickUnity.CustomTask));
	}
}
