using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_Task : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.Task o;
			o=new QuickUnity.Task();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Start(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
				self.Start();
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
				QuickUnity.TaskDoneHandler a1;
				LuaDelegation.checkDelegate(l,2,out a1);
				QuickUnity.TaskProcessHandler a2;
				LuaDelegation.checkDelegate(l,3,out a2);
				self.Start(a1,a2);
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
	static public int StartAndWaitForDone(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.TaskDoneHandler a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			QuickUnity.TaskProcessHandler a2;
			LuaDelegation.checkDelegate(l,3,out a2);
			var ret=self.StartAndWaitForDone(a1,a2);
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
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			var ret=self.WaitForDone();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddDoneCallback(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.TaskDoneHandler a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.AddDoneCallback(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddProgressCallback(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.TaskProcessHandler a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			self.AddProgressCallback(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_progress(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.progress);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_result(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.result);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_error(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.error);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ready(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.ready);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_running(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.running);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_done(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.done);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_lastTime(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.lastTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_startTime(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.startTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_endTime(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.endTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_timeout(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.timeout);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_timeout(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.timeout=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_state(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushEnum(l,(int)self.state);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity");
		addMember(l,Start);
		addMember(l,StartAndWaitForDone);
		addMember(l,WaitForDone);
		addMember(l,AddDoneCallback);
		addMember(l,AddProgressCallback);
		addMember(l,"progress",get_progress,null,true);
		addMember(l,"result",get_result,null,true);
		addMember(l,"error",get_error,null,true);
		addMember(l,"ready",get_ready,null,true);
		addMember(l,"running",get_running,null,true);
		addMember(l,"done",get_done,null,true);
		addMember(l,"lastTime",get_lastTime,null,true);
		addMember(l,"startTime",get_startTime,null,true);
		addMember(l,"endTime",get_endTime,null,true);
		addMember(l,"timeout",get_timeout,set_timeout,true);
		addMember(l,"state",get_state,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.Task));
	}
}
