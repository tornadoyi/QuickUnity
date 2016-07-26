using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_QList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			QuickUnity.QList o;
			if(argc==1){
				o=new QuickUnity.QList();
				pushValue(l,true);
				pushValue(l,o);
				return 2;
			}
			else if(argc==2){
				System.Int32 a1;
				checkType(l,2,out a1);
				o=new QuickUnity.QList(a1);
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
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			System.Object a1;
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
	static public int BinarySearch(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.BinarySearch(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Clear(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			self.Clear();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Contains(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Contains(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int GetRange(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			var ret=self.GetRange(a1,a2);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IndexOf(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
				System.Object a1;
				checkType(l,2,out a1);
				var ret=self.IndexOf(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
				System.Object a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				var ret=self.IndexOf(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
				System.Object a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				var ret=self.IndexOf(a1,a2,a3);
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
	static public int Insert(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Object a2;
			checkType(l,3,out a2);
			self.Insert(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int LastIndexOf(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
				System.Object a1;
				checkType(l,2,out a1);
				var ret=self.LastIndexOf(a1);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==3){
				QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
				System.Object a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				var ret=self.LastIndexOf(a1,a2);
				pushValue(l,true);
				pushValue(l,ret);
				return 2;
			}
			else if(argc==4){
				QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
				System.Object a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				System.Int32 a3;
				checkType(l,4,out a3);
				var ret=self.LastIndexOf(a1,a2,a3);
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
	static public int Remove(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			System.Object a1;
			checkType(l,2,out a1);
			var ret=self.Remove(a1);
			pushValue(l,true);
			pushValue(l,ret);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveAt(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.RemoveAt(a1);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveRange(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Int32 a2;
			checkType(l,3,out a2);
			self.RemoveRange(a1,a2);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Reverse(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==1){
				QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
				self.Reverse();
				pushValue(l,true);
				return 1;
			}
			else if(argc==3){
				QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
				System.Int32 a1;
				checkType(l,2,out a1);
				System.Int32 a2;
				checkType(l,3,out a2);
				self.Reverse(a1,a2);
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
	static public int Sort(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			self.Sort();
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Capacity(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Capacity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Capacity(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Capacity=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_Count(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.Count);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_Count(IntPtr l) {
		try {
			QuickUnity.QList self=(QuickUnity.QList)checkSelf(l);
			int v;
			checkType(l,2,out v);
			self.Count=v;
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.QList");
		addMember(l,Add);
		addMember(l,BinarySearch);
		addMember(l,Clear);
		addMember(l,Contains);
		addMember(l,GetRange);
		addMember(l,IndexOf);
		addMember(l,Insert);
		addMember(l,LastIndexOf);
		addMember(l,Remove);
		addMember(l,RemoveAt);
		addMember(l,RemoveRange);
		addMember(l,Reverse);
		addMember(l,Sort);
		addMember(l,"Capacity",get_Capacity,set_Capacity,true);
		addMember(l,"Count",get_Count,set_Count,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.QList),typeof(System.Collections.Generic.List<System.Object>));
	}
}
