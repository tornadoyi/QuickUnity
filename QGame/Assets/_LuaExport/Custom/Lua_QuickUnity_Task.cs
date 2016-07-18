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
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			var ret=self.Start();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Finish(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.Task.FinishCallback a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.Finish(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Success(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.Task.FinishCallback a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.Success(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Fail(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.Task.FinishCallback a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.Fail(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Cancel(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.Task.FinishCallback a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.Cancel(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Timeout(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.Task.FinishCallback a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.Timeout(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Progress(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			QuickUnity.Task.ProgressCallback a1;
			LuaDelegation.checkDelegate(l,2,out a1);
			var ret=self.Progress(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int WaitForFinish(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			var ret=self.WaitForFinish();
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_sleep(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.sleep);
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
	static public int get_finish(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.finish);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_success(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.success);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_fail(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.fail);
			return 2;
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
	static public int get_retryCount(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.retryCount);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_retryCount(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.retryCount=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_curRetryCount(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.curRetryCount);
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
	static public int get_costTime(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.costTime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_hasTimeout(IntPtr l) {
		try {
			QuickUnity.Task self=(QuickUnity.Task)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.hasTimeout);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.Task");
		addMember(l,Start);
		addMember(l,Finish);
		addMember(l,Success);
		addMember(l,Fail);
		addMember(l,Cancel);
		addMember(l,Timeout);
		addMember(l,Progress);
		addMember(l,WaitForFinish);
		addMember(l,"sleep",get_sleep,null,true);
		addMember(l,"running",get_running,null,true);
		addMember(l,"finish",get_finish,null,true);
		addMember(l,"success",get_success,null,true);
		addMember(l,"fail",get_fail,null,true);
		addMember(l,"progress",get_progress,null,true);
		addMember(l,"error",get_error,null,true);
		addMember(l,"retryCount",get_retryCount,set_retryCount,true);
		addMember(l,"curRetryCount",get_curRetryCount,null,true);
		addMember(l,"startTime",get_startTime,null,true);
		addMember(l,"endTime",get_endTime,null,true);
		addMember(l,"timeout",get_timeout,set_timeout,true);
		addMember(l,"costTime",get_costTime,null,true);
		addMember(l,"hasTimeout",get_hasTimeout,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.Task));
	}
}
