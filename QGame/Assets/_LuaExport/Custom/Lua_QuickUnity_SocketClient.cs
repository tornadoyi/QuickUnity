using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_SocketClient : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.SocketClient o;
			o=new QuickUnity.SocketClient();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Connect(IntPtr l) {
		try {
			QuickUnity.SocketClient self=(QuickUnity.SocketClient)checkSelf(l);
			var ret=self.Connect();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ip(IntPtr l) {
		try {
			QuickUnity.SocketClient self=(QuickUnity.SocketClient)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ip);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ip(IntPtr l) {
		try {
			QuickUnity.SocketClient self=(QuickUnity.SocketClient)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self.ip=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_port(IntPtr l) {
		try {
			QuickUnity.SocketClient self=(QuickUnity.SocketClient)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.port);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_port(IntPtr l) {
		try {
			QuickUnity.SocketClient self=(QuickUnity.SocketClient)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.port=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_connectTimeout(IntPtr l) {
		try {
			QuickUnity.SocketClient self=(QuickUnity.SocketClient)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.connectTimeout);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_connectTimeout(IntPtr l) {
		try {
			QuickUnity.SocketClient self=(QuickUnity.SocketClient)checkSelf(l);
			System.Single v;
			checkType(l,2,out v);
			self.connectTimeout=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.SocketClient");
		addMember(l,Connect);
		addMember(l,"ip",get_ip,set_ip,true);
		addMember(l,"port",get_port,set_port,true);
		addMember(l,"connectTimeout",get_connectTimeout,set_connectTimeout,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.SocketClient),typeof(QuickUnity.SocketBase));
	}
}
