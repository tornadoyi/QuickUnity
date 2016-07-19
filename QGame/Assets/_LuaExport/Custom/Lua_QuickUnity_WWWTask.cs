using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_WWWTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			QuickUnity.WWWTask o;
			if(argc==2){
				System.String a1;
				checkType(l,2,out a1);
				o=new QuickUnity.WWWTask(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.WWWForm))){
				System.String a1;
				checkType(l,2,out a1);
				UnityEngine.WWWForm a2;
				checkType(l,3,out a2);
				o=new QuickUnity.WWWTask(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(System.Byte[]))){
				System.String a1;
				checkType(l,2,out a1);
				System.Byte[] a2;
				checkArray(l,3,out a2);
				o=new QuickUnity.WWWTask(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==4){
				System.String a1;
				checkType(l,2,out a1);
				System.Byte[] a2;
				checkArray(l,3,out a2);
				System.Collections.Generic.Dictionary<System.String,System.String> a3;
				checkType(l,4,out a3);
				o=new QuickUnity.WWWTask(a1,a2,a3);
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
	static public int get_url(IntPtr l) {
		try {
			QuickUnity.WWWTask self=(QuickUnity.WWWTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.url);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_form(IntPtr l) {
		try {
			QuickUnity.WWWTask self=(QuickUnity.WWWTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.form);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_postData(IntPtr l) {
		try {
			QuickUnity.WWWTask self=(QuickUnity.WWWTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.postData);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_responseHeaders(IntPtr l) {
		try {
			QuickUnity.WWWTask self=(QuickUnity.WWWTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.responseHeaders);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.WWWTask");
		addMember(l,"url",get_url,null,true);
		addMember(l,"form",get_form,null,true);
		addMember(l,"postData",get_postData,null,true);
		addMember(l,"processor",null,null,true);
		addMember(l,"responseHeaders",get_responseHeaders,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.WWWTask),typeof(QuickUnity.CoroutineTask));
	}
}
