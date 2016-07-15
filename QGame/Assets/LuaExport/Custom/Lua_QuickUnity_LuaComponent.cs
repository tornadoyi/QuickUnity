using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_LuaComponent : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.LuaComponent o;
			o=new QuickUnity.LuaComponent();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Invoke(IntPtr l) {
		try {
			QuickUnity.LuaComponent self=(QuickUnity.LuaComponent)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.Invoke(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ConnectLuaClass(IntPtr l) {
		try {
			QuickUnity.LuaComponent self=(QuickUnity.LuaComponent)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			self.ConnectLuaClass(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int OnProcessEvent(IntPtr l) {
		try {
			QuickUnity.LuaComponent self=(QuickUnity.LuaComponent)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			QuickUnity.EventManager a4;
			checkType(l,5,out a4);
			self.OnProcessEvent(a1,a2,a3,a4);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_luaClassName(IntPtr l) {
		try {
			QuickUnity.LuaComponent self=(QuickUnity.LuaComponent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.luaClassName);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_luaClassObject(IntPtr l) {
		try {
			QuickUnity.LuaComponent self=(QuickUnity.LuaComponent)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.luaClassObject);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		addMember(l,Invoke);
		addMember(l,ConnectLuaClass);
		addMember(l,OnProcessEvent);
		addMember(l,"luaClassName",get_luaClassName,null,true);
		addMember(l,"luaClassObject",get_luaClassObject,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.LuaComponent),typeof(QuickUnity.QuickBehaviour));
	}
}
