using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_QuickBehaviour : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Schedule(IntPtr l) {
		try {
			QuickUnity.QuickBehaviour self=(QuickUnity.QuickBehaviour)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			QuickUnity.Action.CallFunc.Callback a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.Schedule(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ScheduleOnce(IntPtr l) {
		try {
			QuickUnity.QuickBehaviour self=(QuickUnity.QuickBehaviour)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			QuickUnity.Action.CallFunc.Callback a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.ScheduleOnce(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnSchedule(IntPtr l) {
		try {
			QuickUnity.QuickBehaviour self=(QuickUnity.QuickBehaviour)checkSelf(l);
			QuickUnity.Action.ActionBase a1;
			checkType(l,2,out a1);
			self.UnSchedule(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_action(IntPtr l) {
		try {
			QuickUnity.QuickBehaviour self=(QuickUnity.QuickBehaviour)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.action);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.QuickBehaviour");
		addMember(l,Schedule);
		addMember(l,ScheduleOnce);
		addMember(l,UnSchedule);
		addMember(l,"action",get_action,null,true);
		createTypeMetatable(l,null, typeof(QuickUnity.QuickBehaviour),typeof(UnityEngine.MonoBehaviour));
	}
}
