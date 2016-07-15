using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_System_Enum : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetTypeCode(IntPtr l) {
		try {
			System.Enum self=(System.Enum)checkSelf(l);
			var ret=self.GetTypeCode();
			pushValue(l,true);
			pushEnum(l,(int)ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int CompareTo(IntPtr l) {
		try {
			System.Enum self=(System.Enum)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.CompareTo(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetValues_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			var ret=System.Enum.GetValues(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetNames_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			var ret=System.Enum.GetNames(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetName_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=System.Enum.GetName(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsDefined_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			var ret=System.Enum.IsDefined(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetUnderlyingType_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			var ret=System.Enum.GetUnderlyingType(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Parse_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				System.Type a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				var ret=System.Enum.Parse(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				System.Type a1;
				checkType(l,1,out a1);
				System.String a2;
				checkType(l,2,out a2);
				System.Boolean a3;
				checkType(l,3,out a3);
				var ret=System.Enum.Parse(a1,a2,a3);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int ToObject_s(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(matchType(l,argc,1,typeof(System.Type),typeof(System.UInt16))){
				System.Type a1;
				checkType(l,1,out a1);
				System.UInt16 a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Type),typeof(System.SByte))){
				System.Type a1;
				checkType(l,1,out a1);
				System.SByte a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Type),typeof(System.UInt64))){
				System.Type a1;
				checkType(l,1,out a1);
				System.UInt64 a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Type),typeof(System.UInt32))){
				System.Type a1;
				checkType(l,1,out a1);
				System.UInt32 a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Type),typeof(System.Object))){
				System.Type a1;
				checkType(l,1,out a1);
				System.Object a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Type),typeof(System.Int16))){
				System.Type a1;
				checkType(l,1,out a1);
				System.Int16 a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Type),typeof(System.Byte))){
				System.Type a1;
				checkType(l,1,out a1);
				System.Byte a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Type),typeof(System.Int64))){
				System.Type a1;
				checkType(l,1,out a1);
				System.Int64 a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(matchType(l,argc,1,typeof(System.Type),typeof(int))){
				System.Type a1;
				checkType(l,1,out a1);
				System.Int32 a2;
				checkType(l,2,out a2);
				var ret=System.Enum.ToObject(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
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
	static public int Format_s(IntPtr l) {
		try {
			System.Type a1;
			checkType(l,1,out a1);
			System.Object a2;
			checkType(l,2,out a2);
			System.String a3;
			checkType(l,3,out a3);
			var ret=System.Enum.Format(a1,a2,a3);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"System.Enum");
		addMember(l,GetTypeCode);
		addMember(l,CompareTo);
		addMember(l,GetValues_s);
		addMember(l,GetNames_s);
		addMember(l,GetName_s);
		addMember(l,IsDefined_s);
		addMember(l,GetUnderlyingType_s);
		addMember(l,Parse_s);
		addMember(l,ToObject_s);
		addMember(l,Format_s);
		createTypeMetatable(l,null, typeof(System.Enum),typeof(System.ValueType));
	}
}
