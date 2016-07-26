using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_SafeLuaCoroutine : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.SafeLuaCoroutine o;
			o=new QuickUnity.SafeLuaCoroutine();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StartCoroutine_s(IntPtr l) {
		try {
			UnityEngine.MonoBehaviour a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			QuickUnity.SafeLuaCoroutine.CoroutineCallback a3;
			LuaDelegation.checkDelegate(l,3,out a3);
			var ret=QuickUnity.SafeLuaCoroutine.StartCoroutine(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int NextFrame_s(IntPtr l) {
		try {
			UnityEngine.MonoBehaviour a1;
			checkType(l,1,out a1);
			QuickUnity.SafeLuaCoroutine.CoroutineCallback a2;
			LuaDelegation.checkDelegate(l,2,out a2);
			var ret=QuickUnity.SafeLuaCoroutine.NextFrame(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.SafeLuaCoroutine");
		addMember(l,StartCoroutine_s);
		addMember(l,NextFrame_s);
		createTypeMetatable(l,constructor, typeof(QuickUnity.SafeLuaCoroutine));
	}
}
