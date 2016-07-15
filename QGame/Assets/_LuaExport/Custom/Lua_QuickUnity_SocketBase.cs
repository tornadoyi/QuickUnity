using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_SocketBase : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.SocketBase o;
			QuickUnity.SocketBase.Type a1;
			checkEnum(l,2,out a1);
			o=new QuickUnity.SocketBase(a1);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Disconnect(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			self.Disconnect();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Send(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			System.Byte[] a1;
			checkArray(l,2,out a1);
			var ret=self.Send(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Tick(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			self.Tick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_HeadLength(IntPtr l) {
		try {
			pushValue(l,true);
			pushValue(l,QuickUnity.SocketBase.HeadLength);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_socketType(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.socketType);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_socketType(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			QuickUnity.SocketBase.Type v;
			checkEnum(l,2,out v);
			self.socketType=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_maxMessageSize(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.maxMessageSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_maxMessageSize(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			System.Int32 v;
			checkType(l,2,out v);
			self.maxMessageSize=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_debug(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.debug);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_debug(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			System.Boolean v;
			checkType(l,2,out v);
			self.debug=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get__lastError(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self._lastError);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set__lastError(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			System.String v;
			checkType(l,2,out v);
			self._lastError=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_lastError(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.lastError);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_lastLog(IntPtr l) {
		try {
			QuickUnity.SocketBase self=(QuickUnity.SocketBase)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.lastLog=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.SocketBase");
		addMember(l,Disconnect);
		addMember(l,Send);
		addMember(l,Tick);
		addMember(l,"HeadLength",get_HeadLength,null,false);
		addMember(l,"socketType",get_socketType,set_socketType,true);
		addMember(l,"maxMessageSize",get_maxMessageSize,set_maxMessageSize,true);
		addMember(l,"debug",get_debug,set_debug,true);
		addMember(l,"_lastError",get__lastError,set__lastError,true);
		addMember(l,"lastError",get_lastError,null,true);
		addMember(l,"lastLog",null,set_lastLog,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.SocketBase));
	}
}
