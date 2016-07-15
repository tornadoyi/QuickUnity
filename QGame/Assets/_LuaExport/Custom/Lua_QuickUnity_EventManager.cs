using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_EventManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterReciver(IntPtr l) {
		try {
			QuickUnity.EventManager self=(QuickUnity.EventManager)checkSelf(l);
			UnityEngine.MonoBehaviour a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			var ret=self.RegisterReciver(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SendEvent(IntPtr l) {
		try {
			QuickUnity.EventManager self=(QuickUnity.EventManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.SendEvent(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SendEventAsync(IntPtr l) {
		try {
			QuickUnity.EventManager self=(QuickUnity.EventManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.SendEventAsync(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CheckReceiverValid_s(IntPtr l) {
		try {
			UnityEngine.MonoBehaviour a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.EventManager.CheckReceiverValid(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.EventManager");
		addMember(l,RegisterReciver);
		addMember(l,SendEvent);
		addMember(l,SendEventAsync);
		addMember(l,CheckReceiverValid_s);
		createTypeMetatable(l,null, typeof(QuickUnity.EventManager),typeof(UnityEngine.MonoBehaviour));
	}
}
