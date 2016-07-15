using UnityEngine;
using System.Collections;
using System;
using SLua;
using LuaInterface;
using System.Text;

namespace SLua
{
    public class LuaDebugHelper
    {
        public static void StackDump(IntPtr l, string tag = default(string))
        {
            var builder = new StringBuilder();
            int i;
            int top = LuaDLL.lua_gettop(l); //lua_gettop(l);

            tag = string.IsNullOrEmpty(tag) ? string.Empty : tag;
            builder.AppendFormat("[{1}]total in stack {0}\n", top, tag);

            for (i = top; i > 0; i--)
            {  /* repeat for each level */
                LuaTypes t = LuaDLL.lua_type(l, i);
                switch (t)
                {
                    case LuaTypes.LUA_TSTRING:  /* strings */
                        builder.AppendFormat("string: '{0}'\n", LuaDLL.lua_tostring(l, i));
                        break;
                    case LuaTypes.LUA_TBOOLEAN:  /* booleans */
                        builder.AppendFormat("boolean {0}\n", LuaDLL.lua_toboolean(l, i) ? "true" : "false");
                        break;
                    case LuaTypes.LUA_TNUMBER:  /* numbers */
                        builder.AppendFormat("number: {0}\n", LuaDLL.lua_tonumber(l, i));
                        break;
                    case LuaTypes.LUA_TTHREAD:
                        builder.AppendFormat("thread: {0}\n", LuaDLL.lua_tothread(l, i));
                        break;
                    case LuaTypes.LUA_TFUNCTION:
                        builder.AppendFormat("function: {0}\n", LuaDLL.lua_tocfunction (l, i));
                        break;
                    case LuaTypes.LUA_TNIL:
                        builder.Append("nil");
                        break;
                    default:  /* other values */
                        builder.AppendFormat("{0}\n", LuaDLL.lua_typename(l, (int)t));
                        break;
                }
                //builder.Append("  ");  /* put a separator */
            }
            //builder.Append("\n");  /* end the listing */
            Debug.Log(builder.ToString());
        }
       
    }
}

