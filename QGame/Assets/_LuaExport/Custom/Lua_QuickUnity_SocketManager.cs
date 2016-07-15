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
	static public int _RemoveSocket(IntPtr l) {
		try {
			QuickUnity.SocketManager self=(QuickUnity.SocketManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self._RemoveSocket(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int _GetSocket(IntPtr l) {
		try {
			QuickUnity.SocketManager self=(QuickUnity.SocketManager)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			var ret=self._GetSocket(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CreateClient_s(IntPtr l) {
		try {
			QuickUnity.SocketClient a1;
			var ret=QuickUnity.SocketManager.CreateClient(out a1);
			pushValue(l,true);
			pushValue(l,ret);
			pushValue(l,a1);
			return 3;
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
	static public int GetSocket_s(IntPtr l) {
		try {
			System.Int32 a1;
			checkType(l,1,out a1);
			var ret=QuickUnity.SocketManager.GetSocket(a1);
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
		addMember(l,_RemoveSocket);
		addMember(l,_GetSocket);
		addMember(l,CreateClient_s);
		addMember(l,RemoveSocket_s);
		addMember(l,GetSocket_s);
		createTypeMetatable(l,constructor, typeof(QuickUnity.SocketManager),typeof(BaseManager<QuickUnity.SocketManager>));
	}
}
