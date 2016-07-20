﻿using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using SLua;
using LuaInterface;

namespace QuickUnity
{
    public class LuaHelper : LuaObject
    {
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int _iter(IntPtr l)
        {
            object obj = checkObj(l, LuaDLL.lua_upvalueindex(1));
            IEnumerator it = (IEnumerator)obj;
            if (it.MoveNext())
            {
                pushVar(l, it.Current);
                return 1;
            }
            else
            {
                if (obj is IDisposable)
                    (obj as IDisposable).Dispose();
            }
            return 0;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int iter(IntPtr l)
        {
            object o = checkObj(l, 1);
            if (o is IEnumerable)
            {
                IEnumerable e = o as IEnumerable;
                IEnumerator iter = e.GetEnumerator();
                pushValue(l, true);
                pushLightObject(l, iter);
                LuaDLL.lua_pushcclosure(l, _iter, 1);
                return 2;
            }
            return error(l,"passed in object isn't enumerable");
        }

        /// <summary>
        /// Create standard System.Action
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        [MonoPInvokeCallbackAttribute(typeof (LuaCSFunction))]
        public static int CreateAction(IntPtr l)
        {
            try
            {

                LuaFunction func;
                checkType(l, 1, out func);
                var action = new System.Action(() =>
                    {
                        func.call();

                    });
                pushValue(l, true);
                pushVar(l, action);
                return 2;
            }
            catch (Exception e)
            {
                return error(l, e);
            }

        }

        [MonoPInvokeCallbackAttribute(typeof (LuaCSFunction))]
        static public int CreateClass(IntPtr l)
        {
            try
            {
                string cls;
                checkType(l, 1, out cls);
                Type t = LuaObject.FindType(cls);
                if (t == null)
                {
                    return error(l, string.Format("Can't find {0} to create", cls));
                }

                ConstructorInfo[] cis = t.GetConstructors();
                ConstructorInfo target = null;
                for (int n = 0; n < cis.Length; n++)
                {
                    ConstructorInfo ci = cis[n];
                    if (matchType(l, LuaDLL.lua_gettop(l), 2, ci.GetParameters()))
                    {
                        target = ci;
                        break;
                    }
                }

                if (target != null)
                {
                    ParameterInfo[] pis = target.GetParameters();
                    object[] args = new object[pis.Length];
                    for (int n = 0; n < pis.Length; n++)
                        args[n] = Convert.ChangeType(checkVar(l, n + 2), pis[n].ParameterType);

                    object ret = target.Invoke(args);
                    pushValue(l, true);
                    pushVar(l, ret);
                    return 2;
                }
                pushValue(l, true);
                return 1;
            }
            catch (Exception e)
            {
                return error(l,e);
            }
        }




        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int GetClass(IntPtr l)
        {
            try
            {
                string cls;
                checkType(l, 1, out cls);
                Type t = LuaObject.FindType(cls);
                if (t == null)
                {
                    return error(l, "Can't find {0} to create", cls);
                }

                LuaClassObject co = new LuaClassObject(t);
                pushValue(l, true);
                LuaObject.pushObject(l,co);
                return 2;
            }
            catch (Exception e)
            {
                return error(l, e);
            }
        }

        //convert lua binary string to c# byte[]
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int ToBytes(IntPtr l){
            try{
                byte[] bytes = null;
                checkBinaryString(l,1,out bytes);
                pushValue(l,true);
                LuaObject.pushObject(l,bytes);
                return 2;

            }catch(System.Exception e){
                return error(l, e);
            }
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public new int ToString(IntPtr l)
        {
            try
            {
                object o = checkObj(l, 1);
                if (o == null)
                {
                    pushValue(l, true);
                    LuaDLL.lua_pushnil(l);
                    return 2;
                }

                pushValue(l, true);
                if (o is byte[])
                {
                    byte[] b = (byte[])o;
                    LuaDLL.lua_pushlstring(l, b, b.Length);
                }
                else
                {
                    pushValue(l, o.ToString());
                }
                return 2;
            }
            catch (Exception e)
            {
                return error(l, e);
            }
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int MakeArray(IntPtr l)
        {
            try
            {
                Type t;
                checkType (l,1,out t);
                LuaDLL.luaL_checktype(l, 2, LuaTypes.LUA_TTABLE);
                int n = LuaDLL.lua_rawlen(l, 2);
                Array array=Array.CreateInstance(t,n);
                for (int k = 0; k < n; k++)
                {
                    LuaDLL.lua_rawgeti(l, 2, k + 1);
                    array.SetValue(Convert.ChangeType(checkVar(l, -1),t),k);
                    LuaDLL.lua_pop(l, 1);
                }
                pushValue(l, true);
                pushValue(l, array);
                return 2;
            }
            catch (Exception e)
            {
                return error(l, e);
            }
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int As(IntPtr l)
        {
            try
            {
                if (!isTypeTable(l, 2))
                {
                    return error(l, "No matched type of param 2");
                }
                string meta = LuaDLL.lua_tostring(l, -1);
                LuaDLL.luaL_getmetatable(l, meta);
                LuaDLL.lua_setmetatable(l, 1);
                pushValue(l, true);
                LuaDLL.lua_pushvalue(l, 1);
                return 2;
            }
            catch (Exception e)
            {
                return error(l, e);
            }
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int IsNull(IntPtr l)
        {
            try
            {
                LuaTypes t = LuaDLL.lua_type(l, 1);
                pushValue(l, true);

                if (t == LuaTypes.LUA_TNIL)
                {
                    pushValue(l, true);
                }
                // LUA_TUSERDATA or LUA_TTABLE(Class inherited from Unity Native)
                else if (t == LuaTypes.LUA_TUSERDATA || isLuaClass(l, 1))
                {
                    object o = checkObj(l, 1);
                    #if !SLUA_STANDALONE
                    if( o is UnityEngine.Object )
                    {
                        pushValue(l, ((UnityEngine.Object)o)==null);
                    }
                    else
                    #endif
                        pushValue(l, o.Equals(null));
                }
                else
                    pushValue(l, false);

                return 2;
            }
            catch (Exception e)
            {
                return error(l, e);
            }
        }

        static LuaOut luaOut = new LuaOut();
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int get_out(IntPtr l)
        {
            pushValue(l, true);
            pushLightObject(l, luaOut);
            return 2;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int get_version(IntPtr l)
        {
            pushValue(l, true);
            pushValue(l, VersionNumber);
            return 2;
        }

        static public void reg(IntPtr l)
        {
            getTypeTable(l, "slua");
            addMember(l, "action",      CreateAction,   false);
            addMember(l, "createclass", CreateClass,    false);
            addMember(l, "getclass",    GetClass,       false);
            addMember(l, "iter",        iter,           false);
            addMember(l, "tostring",    ToString,       false);
            addMember(l, "as",          As,             false);
            addMember(l, "isnull",      IsNull,         false);
            addMember(l, "array",       MakeArray,      false);
            addMember(l, "tobytes",     ToBytes,        false);
            addMember(l, "out",         get_out,        null, false);
            addMember(l, "version",     get_version,    null, false);

            //LuaFunction func = LuaState.get(l).doString(classfunc) as LuaFunction;
            //func.push(l);
            //LuaDLL.lua_setfield(l, -3, "Class");


            createTypeMetatable(l, null, typeof(Helper));
        }
    }
}

