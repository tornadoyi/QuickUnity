﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_ParticleSystemSortMode : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"UnityEngine.ParticleSystemSortMode");
		addMember(l,0,"None");
		addMember(l,1,"Distance");
		addMember(l,2,"OldestInFront");
		addMember(l,3,"YoungestInFront");
		LuaDLL.lua_pop(l, 1);
	}
}
