using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_TCPSocket : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.TCPSocket o;
			o=new QuickUnity.TCPSocket();
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
			QuickUnity.TCPSocket self=(QuickUnity.TCPSocket)checkSelf(l);
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
	static public int Disconnect(IntPtr l) {
		try {
			QuickUnity.TCPSocket self=(QuickUnity.TCPSocket)checkSelf(l);
			self.Disconnect();
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
			QuickUnity.TCPSocket self=(QuickUnity.TCPSocket)checkSelf(l);
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
			QuickUnity.TCPSocket self=(QuickUnity.TCPSocket)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.connectTimeout=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_sendThreadSleepTime(IntPtr l) {
		try {
			QuickUnity.TCPSocket self=(QuickUnity.TCPSocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.sendThreadSleepTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_sendThreadSleepTime(IntPtr l) {
		try {
			QuickUnity.TCPSocket self=(QuickUnity.TCPSocket)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.sendThreadSleepTime=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.TCPSocket");
		addMember(l,Connect);
		addMember(l,Disconnect);
		addMember(l,"connectTimeout",get_connectTimeout,set_connectTimeout,true);
		addMember(l,"sendThreadSleepTime",get_sendThreadSleepTime,set_sendThreadSleepTime,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.TCPSocket),typeof(QuickUnity.ISocket));
	}
}
