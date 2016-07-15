using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_QuickUnity_EngineEvent : LuaObject {
	static public void reg(IntPtr l) {
		getEnumTable(l,"QuickUnity.EngineEvent");
		addMember(l,0,"PointerEnter");
		addMember(l,1,"PointerExit");
		addMember(l,2,"PointerDown");
		addMember(l,3,"PointerUp");
		addMember(l,4,"PointerClick");
		addMember(l,5,"Drag");
		addMember(l,6,"Drop");
		addMember(l,7,"Scroll");
		addMember(l,8,"UpdateSelected");
		addMember(l,9,"Select");
		addMember(l,10,"Deselect");
		addMember(l,11,"Move");
		addMember(l,12,"InitializePotentialDrag");
		addMember(l,13,"BeginDrag");
		addMember(l,14,"EndDrag");
		addMember(l,15,"Submit");
		addMember(l,16,"Cancel");
		addMember(l,-1007,"TriggerExit2D");
		addMember(l,-1006,"TriggerEnter2D");
		addMember(l,-1005,"CollisionExit2D");
		addMember(l,-1004,"CollisionEnter2D");
		addMember(l,-1003,"TriggerExit");
		addMember(l,-1002,"TriggerEnter");
		addMember(l,-1001,"CollisionExit");
		addMember(l,-1000,"CollisionEnter");
		LuaDLL.lua_pop(l, 1);
	}
}
