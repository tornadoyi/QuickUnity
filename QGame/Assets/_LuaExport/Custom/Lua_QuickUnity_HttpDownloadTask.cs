using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_HttpDownloadTask : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask o;
			System.String a1;
			checkType(l,2,out a1);
			System.String a2;
			checkType(l,3,out a2);
			System.Boolean a3;
			checkType(l,4,out a3);
			System.String a4;
			checkType(l,5,out a4);
			System.String a5;
			checkType(l,6,out a5);
			System.Int32 a6;
			checkType(l,7,out a6);
			System.Int32 a7;
			checkType(l,8,out a7);
			o=new QuickUnity.HttpDownloadTask(a1,a2,a3,a4,a5,a6,a7);
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_fileSavePath(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.fileSavePath);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_automaticDecompression(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.automaticDecompression);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_internetProxy(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.internetProxy);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_expectMD5(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.expectMD5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_expectFileSize(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.expectFileSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_md5(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.md5);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_fileSize(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.fileSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_downloadSize(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.downloadSize);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_processBytes(IntPtr l) {
		try {
			QuickUnity.HttpDownloadTask self=(QuickUnity.HttpDownloadTask)checkSelf(l);
			pushValue(l,true);
			pushValue(l,self.processBytes);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"QuickUnity.HttpDownloadTask");
		addMember(l,"fileSavePath",get_fileSavePath,null,true);
		addMember(l,"automaticDecompression",get_automaticDecompression,null,true);
		addMember(l,"internetProxy",get_internetProxy,null,true);
		addMember(l,"expectMD5",get_expectMD5,null,true);
		addMember(l,"expectFileSize",get_expectFileSize,null,true);
		addMember(l,"md5",get_md5,null,true);
		addMember(l,"fileSize",get_fileSize,null,true);
		addMember(l,"downloadSize",get_downloadSize,null,true);
		addMember(l,"processBytes",get_processBytes,null,true);
		createTypeMetatable(l,constructor, typeof(QuickUnity.HttpDownloadTask),typeof(QuickUnity.HttpTask));
	}
}
