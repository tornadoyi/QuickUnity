
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

namespace SLua
{
    public partial class LuaDelegation : LuaObject
    {

        static internal int checkDelegate(IntPtr l,int p,out QuickUnity.WWWTask.WWWProcessor ua) {
            int op = extractFunction(l,p);
			if(LuaDLL.lua_isnil(l,p)) {
				ua=null;
				return op;
			}
            else if (LuaDLL.lua_isuserdata(l, p)==1)
            {
                ua = (QuickUnity.WWWTask.WWWProcessor)checkObj(l, p);
                return op;
            }
            LuaDelegate ld;
            checkType(l, -1, out ld);
            if(ld.d!=null)
            {
                ua = (QuickUnity.WWWTask.WWWProcessor)ld.d;
                LuaDLL.lua_pop(l,1);    // [FIX] +gusir, if not pop, the count of params Type[] will be error 
                return op;
            }
			LuaDLL.lua_pop(l,1);
			
			l = LuaState.get(l).L;
            ua = (UnityEngine.WWW a1) =>
            {
                int error = pushTry(l);

				pushValue(ld.L,a1);
				ld.pcall(1, error);
				System.Object ret;
				checkType(l,error+1,out ret);
				LuaDLL.lua_settop(l, error-1);
				return ret;
			};
			ld.d=ua;
			return op;
		}
	}
}
