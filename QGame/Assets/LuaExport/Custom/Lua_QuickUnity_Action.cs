using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Schedule(IntPtr l) {
		try {
			QuickUnity.Action self=(QuickUnity.Action)checkSelf(l);
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
			QuickUnity.Action self=(QuickUnity.Action)checkSelf(l);
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
			QuickUnity.Action self=(QuickUnity.Action)checkSelf(l);
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
	static public int RunAction(IntPtr l) {
		try {
			QuickUnity.Action self=(QuickUnity.Action)checkSelf(l);
			QuickUnity.Action.ActionBase a1;
			checkType(l,2,out a1);
			UnityEngine.GameObject a2;
			checkType(l,3,out a2);
			self.RunAction(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int StopAction(IntPtr l) {
		try {
			QuickUnity.Action self=(QuickUnity.Action)checkSelf(l);
			QuickUnity.Action.ActionBase a1;
			checkType(l,2,out a1);
			self.StopAction(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearAllActions(IntPtr l) {
		try {
			QuickUnity.Action self=(QuickUnity.Action)checkSelf(l);
			self.ClearAllActions();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		addMember(l,Schedule);
		addMember(l,ScheduleOnce);
		addMember(l,UnSchedule);
		addMember(l,RunAction);
		addMember(l,StopAction);
		addMember(l,ClearAllActions);
		createTypeMetatable(l,null, typeof(QuickUnity.Action),typeof(UnityEngine.MonoBehaviour));
	}
}
