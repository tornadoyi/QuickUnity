using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_SocketManager : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.SocketManager o;
			o=new QuickUnity.SocketManager();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateTCPSocket_s(IntPtr l) {
		try {
			var ret=QuickUnity.SocketManager.CreateTCPSocket();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveSocket_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			QuickUnity.SocketManager.RemoveSocket(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int FindSocket_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.SocketManager.FindSocket(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.SocketManager");
		addMember(l,CreateTCPSocket_s);
		addMember(l,RemoveSocket_s);
		addMember(l,FindSocket_s);
		createTypeMetatable(l,constructor, typeof(QuickUnity.SocketManager),typeof(QuickUnity.BaseManager<QuickUnity.SocketManager>));
	}
}
