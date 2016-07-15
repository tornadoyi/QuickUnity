using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_ActionBase : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start(IntPtr l) {
		try {
			QuickUnity.Action.ActionBase self=(QuickUnity.Action.ActionBase)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			self.Start(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Stop(IntPtr l) {
		try {
			QuickUnity.Action.ActionBase self=(QuickUnity.Action.ActionBase)checkSelf(l);
			self.Stop();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Step(IntPtr l) {
		try {
			QuickUnity.Action.ActionBase self=(QuickUnity.Action.ActionBase)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			var ret=self.Step(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsDone(IntPtr l) {
		try {
			QuickUnity.Action.ActionBase self=(QuickUnity.Action.ActionBase)checkSelf(l);
			var ret=self.IsDone();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetDuration(IntPtr l) {
		try {
			QuickUnity.Action.ActionBase self=(QuickUnity.Action.ActionBase)checkSelf(l);
			var ret=self.GetDuration();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WaitForDone(IntPtr l) {
		try {
			QuickUnity.Action.ActionBase self=(QuickUnity.Action.ActionBase)checkSelf(l);
			var ret=self.WaitForDone();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		addMember(l,Start);
		addMember(l,Stop);
		addMember(l,Step);
		addMember(l,IsDone);
		addMember(l,GetDuration);
		addMember(l,WaitForDone);
		createTypeMetatable(l,null, typeof(QuickUnity.Action.ActionBase));
	}
}
