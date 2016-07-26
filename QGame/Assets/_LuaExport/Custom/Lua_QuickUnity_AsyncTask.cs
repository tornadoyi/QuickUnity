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
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.AsyncTask");
		createTypeMetatable(l,constructor, typeof(QuickUnity.AsyncTask),typeof(QuickUnity.Task));
	}
}
