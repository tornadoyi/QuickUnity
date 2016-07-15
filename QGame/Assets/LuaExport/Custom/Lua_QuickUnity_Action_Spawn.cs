using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Action_Spawn : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			QuickUnity.Action.Spawn o;
			if(argc==1){
				o=new QuickUnity.Action.Spawn();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(List<QuickUnity.Action.ActionBase>))){
				System.Collections.Generic.List<QuickUnity.Action.ActionBase> a1;
				checkType(l,2,out a1);
				o=new QuickUnity.Action.Spawn(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(QuickUnity.Action.ActionBase[]))){
				QuickUnity.Action.ActionBase[] a1;
				checkParams(l,2,out a1);
				o=new QuickUnity.Action.Spawn(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Add(IntPtr l) {
		try {
			QuickUnity.Action.Spawn self=(QuickUnity.Action.Spawn)checkSelf(l);
			QuickUnity.Action.ActionBase a1;
			checkType(l,2,out a1);
			self.Add(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start(IntPtr l) {
		try {
			QuickUnity.Action.Spawn self=(QuickUnity.Action.Spawn)checkSelf(l);
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
	static public int Step(IntPtr l) {
		try {
			QuickUnity.Action.Spawn self=(QuickUnity.Action.Spawn)checkSelf(l);
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
			QuickUnity.Action.Spawn self=(QuickUnity.Action.Spawn)checkSelf(l);
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
	static public int CreateWithActions_s(IntPtr l) {
		try {
			QuickUnity.Action.ActionBase[] a1;
			checkParams(l,1,out a1);
			var ret=QuickUnity.Action.Spawn.CreateWithActions(a1);
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
		addMember(l,Add);
		addMember(l,Start);
		addMember(l,Step);
		addMember(l,IsDone);
		addMember(l,CreateWithActions_s);
		createTypeMetatable(l,constructor, typeof(QuickUnity.Action.Spawn),typeof(QuickUnity.Action.IntervalAction));
	}
}
