using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_ISocketListener : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int BindSocket(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			QuickUnity.ISocket a1;
			checkType(l,2,out a1);
			self.BindSocket(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnConnect(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			self.OnConnect();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnDisconnect(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			self.OnDisconnect();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnTick(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			self.OnTick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_disconnected(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.disconnected);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_connecting(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.connecting);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_connected(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.connected);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_disconnecting(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.disconnecting);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_state(IntPtr l) {
		try {
			QuickUnity.ISocketListener self=(QuickUnity.ISocketListener)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.state);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.ISocketListener");
		addMember(l,BindSocket);
		addMember(l,OnConnect);
		addMember(l,OnDisconnect);
		addMember(l,OnTick);
		addMember(l,"disconnected",get_disconnected,null,true);
		addMember(l,"connecting",get_connecting,null,true);
		addMember(l,"connected",get_connected,null,true);
		addMember(l,"disconnecting",get_disconnecting,null,true);
		addMember(l,"state",get_state,null,true);
		createTypeMetatable(l,null, typeof(QuickUnity.ISocketListener));
	}
}
