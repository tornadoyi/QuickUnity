﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_RenderingPath : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.RenderingPath");
		addMember(l,0,"VertexLit");
		addMember(l,1,"Forward");
		addMember(l,2,"DeferredLighting");
		addMember(l,3,"DeferredShading");
		addMember(l,-1,"UsePlayerSettings");
		LuaDLL.lua_pop(l, 1);
	}
}
