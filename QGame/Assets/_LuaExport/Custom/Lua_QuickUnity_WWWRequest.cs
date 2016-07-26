using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_WWWRequest : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			QuickUnity.WWWRequest o;
			if(argc==2){
				System.String a1;
				checkType(l,2,out a1);
				o=new QuickUnity.WWWRequest(a1);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(UnityEngine.WWWForm))){
				System.String a1;
				checkType(l,2,out a1);
				UnityEngine.WWWForm a2;
				checkType(l,3,out a2);
				o=new QuickUnity.WWWRequest(a1,a2);
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(matchType(l,argc,2,typeof(string),typeof(System.Byte[]))){
				System.String a1;
				checkType(l,2,out a1);
				System.Byte[] a2;
				checkArray(l,3,out a2);
				o=new QuickUnity.WWWRequest(a1,a2);
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
				o=new QuickUnity.WWWRequest(a1,a2,a3);
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
	static public int get_bytes(IntPtr l) {
		try {
			QuickUnity.WWWRequest self=(QuickUnity.WWWRequest)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.bytes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.WWWRequest");
		addMember(l,"bytes",get_bytes,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.WWWRequest),typeof(QuickUnity.WWWTask));
	}
}
