using UnityEngine;
using System.Collections;

namespace SLua
{

    #if !SLUA_STANDALONE
    using UnityEngine;
    #endif
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using LuaInterface;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public partial class LuaObject
    {
        public static void start(IntPtr l)
        {
            string newindexfun = @"

local getmetatable=getmetatable
local rawget=rawget
local error=error
local type=type
local function newindex(ud,k,v)
    local t=getmetatable(ud)
    repeat
        local h=rawget(t,k)
        if h then
			if h[2] then
				h[2](ud,v)
	            return
			else
				break --error('property '..k..' is read only')
			end
        end
        t=rawget(t,'__parent')
    until t==nil
    --error('can not find '..k)
    if type(ud) ~= 'table' then 
        error('new index error, can not set '..k ..' to '..type(ud))
    else
        rawset(ud, k, v)
    end
end

return newindex
";

            string indexfun = @"
local type=type
local error=error
local rawget=rawget
local getmetatable=getmetatable
local function index(ud,k)
    local t=getmetatable(ud)
    repeat
        local fun=rawget(t,k)
        local tp=type(fun)	
        if tp=='function' then 
            return fun 
        elseif tp=='table' then
			local f=fun[1]
			if f then
				return f(ud)
			else
				break --error('property '..k..' is write only')
			end
        end
        t = rawget(t,'__parent')
    until t==nil
    --error('Can not find '..k)
    return nil
end

return index
";
            LuaState L = LuaState.get(l);
            newindex_func = (LuaFunction)L.doString(newindexfun);
            index_func = (LuaFunction)L.doString(indexfun);

            // object method
            LuaDLL.lua_createtable(l, 0, 4);
            addMember(l, ToString);
            addMember(l, GetHashCode);
            addMember(l, Equals);
            addMember(l, GetType);
            LuaDLL.lua_setfield(l, LuaIndexes.LUA_REGISTRYINDEX, "__luabaseobject");

            LuaArray.init(l);
            LuaVarObject.init(l);

            LuaDLL.lua_newtable(l);
            LuaDLL.lua_setglobal(l, DelgateTable);


            setupPushVar();
        }

        protected static void addMember(IntPtr l, string name, LuaCSFunction func, bool instance)
        {
            checkMethodValid(func);

            pushValue(l, func);
            LuaDLL.lua_setfield(l, instance ? -2 : -3, name);
        }

        static public object checkLuaTable(IntPtr l, int p)
        {
            LuaTypes type = LuaDLL.lua_type(l, p);
            if (type != LuaTypes.LUA_TTABLE) return null;
            if (isLuaValueType(l, p)) return null;
            LuaTable v;
            checkType(l, p, out v);
            return v;
        }
    }
}