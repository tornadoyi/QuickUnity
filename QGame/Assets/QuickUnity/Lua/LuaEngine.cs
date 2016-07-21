using UnityEngine;
using System.Collections;

using SLua;
using System;
using System.Reflection;
using System.Collections.Generic;
using LuaInterface;
using System.Threading;

namespace QuickUnity
{
    public class LuaEngine : BaseManager<LuaEngine>
    {
        public delegate byte[] LuaLoaderDelegate(string fn);

        public bool inited { get; private set; }

        [DoNotToLua]
        public static bool enableLuaComponent
        {
            get { return instance._enableLuaComponent; }
            set { instance._enableLuaComponent = value; }
        }
        protected bool _enableLuaComponent = false;

        private LuaState _luaState;
        private int errorReported = 0;
        private volatile int bindProgress = 0;

        protected Stack<LuaLoaderDelegate> loaderStack = new Stack<LuaLoaderDelegate>();


        void Update()
        {
            if (!inited)
                return;

            if (LuaDLL.lua_gettop(_luaState.L) != errorReported)
            {
                errorReported = LuaDLL.lua_gettop(_luaState.L);
                Debug.LogError(string.Format("Some function not remove temp value({0}) from lua stack. You should fix it.", LuaDLL.luaL_typename(_luaState.L, errorReported)));
            }

            _luaState.checkRef();
            LuaTimer.tick(Time.deltaTime);
        }

        public static Task Start()
        {
            var task = new CustomTask();
            instance.init(null, () => { task.SetSuccess(); });
            LuaState.loaderDelegate = instance.InnerLoaderDelegate;
            return task;
        }

        public static void DoFile(string file)
        {
            instance._luaState.doFile(file);
        }

        public static object GetGlobalObject(string name) { return instance._luaState[name]; }

        public static void PushLuaLoader(LuaLoaderDelegate loader)
        {
            if (loader == null)
            {
                Debug.LogError("Loader is invalid");
                return;
            }
            instance.loaderStack.Push(loader);
        }

        public static void PopLuaLoader()
        {
            instance.loaderStack.Pop();
        }

        byte[] InnerLoaderDelegate(string fn)
        {
            if (loaderStack.Count <= 0)
            {
                Debug.LogError("No Self_Define Lua Load Delegate");
                return null;
            }
            var loader = loaderStack.Peek();
            if (loader == null)
            {
                Debug.LogError("No Self_Define Lua Load Delegate");
                return null;
            }
            return loader(fn);
        }

        protected void init(Action<int> tick, System.Action complete, LuaSvrFlag flag = LuaSvrFlag.LSF_BASIC)
        {
            LuaState luaState = new LuaState();
            this._luaState = luaState;

            IntPtr L = luaState.L;
            LuaObject.start(L);

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                doBind(L);
                doinit(L, flag);
                if (complete != null) complete.Invoke();
                checkTop(L);
            }
            else
            {
#endif
                ThreadPool.QueueUserWorkItem(doBind, L);
                StartCoroutine(waitForBind(tick, () =>
                {
                    doinit(L, flag);
                    if (complete != null) complete.Invoke();
                    checkTop(L);
                }));
#if UNITY_EDITOR
            }
#endif
        }

        private IEnumerator waitForBind(System.Action<int> tick, System.Action complete)
        {
            int lastProgress = 0;
            do
            {
                if (tick != null)
                    tick(bindProgress);
                // too many yield return will increase binding time
                // so check progress and skip odd progress
                if (lastProgress != bindProgress && bindProgress % 2 == 0)
                {
                    lastProgress = bindProgress;
                    yield return null;
                }
            } while (bindProgress != 100);

            if (tick != null)
                tick(bindProgress);

            complete();
        }


        private void doBind(object state)
        {
            IntPtr L = (IntPtr)state;

            List<Action<IntPtr>> list = new List<Action<IntPtr>>();

#if !SLUA_STANDALONE
#if USE_STATIC_BINDER
			Assembly[] ams = AppDomain.CurrentDomain.GetAssemblies();
			
			bindProgress = 0;

			List<Type> bindlist = new List<Type>();
			for (int n = 0; n < ams.Length;n++ )
			{
				Assembly a = ams[n];
				Type[] ts = null;
				try
				{
					ts = a.GetExportedTypes();
				}
				catch
				{
					continue;
				}
				for (int k = 0; k < ts.Length; k++)
				{
					Type t = ts[k];
					if (t.IsDefined(typeof(LuaBinderAttribute), false))
					{
						bindlist.Add(t);
					}
				}
			}
			
			bindProgress = 1;
			
			bindlist.Sort(new System.Comparison<Type>((Type a, Type b) => {
				LuaBinderAttribute la = System.Attribute.GetCustomAttribute( a, typeof(LuaBinderAttribute) ) as LuaBinderAttribute;
				LuaBinderAttribute lb = System.Attribute.GetCustomAttribute( b, typeof(LuaBinderAttribute) ) as LuaBinderAttribute;
				
				return la.order.CompareTo(lb.order);
			}));
			
			for (int n = 0; n < bindlist.Count; n++)
			{
				Type t = bindlist[n];
				var sublist = (Action<IntPtr>[])t.GetMethod("GetBindList").Invoke(null, null);
				list.AddRange(sublist);
			}
#else
            var assemblyName = "Assembly-CSharp";
            Assembly assembly = Assembly.Load(assemblyName);
            list.AddRange(getBindList(assembly, "SLua.BindUnity"));
            list.AddRange(getBindList(assembly, "SLua.BindUnityUI"));
            list.AddRange(getBindList(assembly, "SLua.BindDll"));
            list.AddRange(getBindList(assembly, "SLua.BindCustom"));
#endif
#endif

            bindProgress = 2;

            int count = list.Count;
            for (int n = 0; n < count; n++)
            {
                Action<IntPtr> action = list[n];
                action(L);
                bindProgress = (int)(((float)n / count) * 98.0) + 2;
            }

            bindProgress = 100;
        }

        Action<IntPtr>[] getBindList(Assembly assembly, string ns)
        {
            Type t = assembly.GetType(ns);
            if (t != null)
                return (Action<IntPtr>[])t.GetMethod("GetBindList").Invoke(null, null);
            return new Action<IntPtr>[0];
        }

        void doinit(IntPtr L, LuaSvrFlag flag)
        {
#if !SLUA_STANDALONE
            LuaTimer.reg(L);
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
#endif
                //LuaCoroutine.reg(L, this);
#endif
            //Helper.reg(L);
            //LuaValueType.reg(L);
            LuaHelper.reg(L);

            if ((flag & LuaSvrFlag.LSF_EXTLIB) != 0)
                LuaDLL.luaS_openextlibs(L);
            if ((flag & LuaSvrFlag.LSF_3RDDLL) != 0)
                Lua3rdDLL.open(L);

            inited = true;
        }

        void checkTop(IntPtr L)
        {
            if (LuaDLL.lua_gettop(_luaState.L) != errorReported)
            {
                Debug.LogError("Some function not remove temp value from lua stack. You should fix it.");
                errorReported = LuaDLL.lua_gettop(_luaState.L);
            }
        }


    }
}