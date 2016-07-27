using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_ISocket : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Dispose(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			self.Dispose();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetListener(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			QuickUnity.ISocketListener a1;
			checkType(l,2,out a1);
			self.SetListener(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Connect(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
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
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
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
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				self.Send(a1);
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.Send(a1,a2);
				pushValue(l,true);
				return 1;
			}
			else if(argc==4){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				self.Send(a1,a2,a3);
				pushValue(l,true);
				return 1;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Receive(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				var ret=self.Receive(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				var ret=self.Receive(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				var ret=self.Receive(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Seek(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				var ret=self.Seek(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				var ret=self.Seek(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
				System.Byte[] a1;
				checkArray(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				var ret=self.Seek(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			pushValue(l,false);
			LuaDLL.lua_pushstring(l,"No matched override function to call");
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Tick(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			self.Tick();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_listener(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.listener);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_protocol(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.protocol);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_state(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.state);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_disconnected(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
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
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
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
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
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
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.disconnecting);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_url(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.url);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_url(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			string v;
			checkType(l,2,out v);
			self.url=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_serverAddress(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.serverAddress);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_serverPort(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.serverPort);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_urlProtocol(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.urlProtocol);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_urlPath(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.urlPath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_receivedLength(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.receivedLength);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_error(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.error);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_lastSendTime(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.lastSendTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_lastReceiveTime(IntPtr l) {
		try {
			QuickUnity.ISocket self=(QuickUnity.ISocket)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.lastReceiveTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.ISocket");
		addMember(l,Dispose);
		addMember(l,SetListener);
		addMember(l,Connect);
		addMember(l,Disconnect);
		addMember(l,Send);
		addMember(l,Receive);
		addMember(l,Seek);
		addMember(l,Tick);
		addMember(l,"listener",get_listener,null,true);
		addMember(l,"protocol",get_protocol,null,true);
		addMember(l,"state",get_state,null,true);
		addMember(l,"disconnected",get_disconnected,null,true);
		addMember(l,"connecting",get_connecting,null,true);
		addMember(l,"connected",get_connected,null,true);
		addMember(l,"disconnecting",get_disconnecting,null,true);
		addMember(l,"url",get_url,set_url,true);
		addMember(l,"serverAddress",get_serverAddress,null,true);
		addMember(l,"serverPort",get_serverPort,null,true);
		addMember(l,"urlProtocol",get_urlProtocol,null,true);
		addMember(l,"urlPath",get_urlPath,null,true);
		addMember(l,"receivedLength",get_receivedLength,null,true);
		addMember(l,"error",get_error,null,true);
		addMember(l,"lastSendTime",get_lastSendTime,null,true);
		addMember(l,"lastReceiveTime",get_lastReceiveTime,null,true);
		createTypeMetatable(l,null, typeof(QuickUnity.ISocket));
	}
}
