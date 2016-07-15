using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_EventTrigger : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.EventTrigger o;
			o=new QuickUnity.EventTrigger();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_events(IntPtr l) {
		try {
			QuickUnity.EventTrigger self=(QuickUnity.EventTrigger)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.events);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_events(IntPtr l) {
		try {
			QuickUnity.EventTrigger self=(QuickUnity.EventTrigger)checkSelf(l);
			System.Collections.Generic.List<QuickUnity.EventTrigger.EventInfo> v;
			checkType(l,2,out v);
			self.events=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		addMember(l,"events",get_events,set_events,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.EventTrigger),typeof(QuickUnity.EventManager));
	}
}
