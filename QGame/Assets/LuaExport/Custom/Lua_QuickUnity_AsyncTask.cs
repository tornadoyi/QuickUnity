using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_AsyncTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.AsyncTask o;
			o=new QuickUnity.AsyncTask();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_threadID(IntPtr l) {
		try {
			QuickUnity.AsyncTask self=(QuickUnity.AsyncTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.threadID);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_asyncResult(IntPtr l) {
		try {
			QuickUnity.AsyncTask self=(QuickUnity.AsyncTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.asyncResult);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		addMember(l,"threadID",get_threadID,null,true);
		addMember(l,"asyncResult",get_asyncResult,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.AsyncTask),typeof(QuickUnity.Task));
	}
}
