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

        public static object GetLuaObject(string name) { return instance._luaState[name]; }

        public static object DoFile(string fileName) { return instance._luaState.doFile(fileName); }

        public static void Restart() { instance.ReInit(); }

        protected override void Awake()
        {
            base.Awake();
            init();
        }

        protected override void OnDestroy()
        {
            if (_luaState != null)
            {
                _luaState.Close();
                _luaState = null;
            }
            base.OnDestroy();
        }

        protected void init()
        {
            _luaState = new LuaState();
            LuaObject.init(_luaState.L);
            bindAll(_luaState.L);
            LuaTimer.reg(_luaState.L);
            //LuaCoroutine.reg(_luaState.L, this);
            Helper.reg(_luaState.L);
            Lua3rdDLL.open(_luaState.L);

            if (LuaDLL.lua_gettop(_luaState.L) != errorReported)
            {
                Debug.LogError("Some function not remove temp value from lua stack. You should fix it.");
                errorReported = LuaDLL.lua_gettop(_luaState.L);
            }

            LuaState.loaderDelegate = InnerLoaderDelegate;
        }

        protected void ReInit()
        {
            if (_luaState != null)
            {
                _luaState.Close();
                _luaState = null;
            }
            init();
        }

        void Update()
        {
            Tick();
        }

        void Tick()
        {
            if (_luaState == null) return;
            if (LuaDLL.lua_gettop(_luaState.L) != errorReported)
            {
                Debug.LogError("Some function not remove temp value from lua stack. You should fix it.");
                errorReported = LuaDLL.lua_gettop(_luaState.L);
            }

            _luaState.checkRef();
            LuaTimer.tick(Time.deltaTime);
        }

        void bindAll(IntPtr l)
        {
            Assembly[] ams = AppDomain.CurrentDomain.GetAssemblies();

            List<Type> bindlist = new List<Type>();
            foreach (Assembly a in ams)
            {
                Type[] ts = a.GetExportedTypes();
                foreach (Type t in ts)
                {
                    if (t.GetCustomAttributes(typeof(LuaBinderAttribute), false).Length > 0)
                    {
                        bindlist.Add(t);
                    }
                }
            }

            bindlist.Sort(new System.Comparison<Type>((Type a, Type b) =>
            {
                LuaBinderAttribute la = (LuaBinderAttribute)a.GetCustomAttributes(typeof(LuaBinderAttribute), false)[0];
                LuaBinderAttribute lb = (LuaBinderAttribute)b.GetCustomAttributes(typeof(LuaBinderAttribute), false)[0];

                return la.order.CompareTo(lb.order);
            })
            );

            foreach (Type t in bindlist)
            {
                t.GetMethod("Bind").Invoke(null, new object[] { l });
            }
        }

        byte[] InnerLoaderDelegate(string fn)
        {
            if(loaderStack.Count <= 0)
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

        protected LuaState _luaState;
        int errorReported = 0;

        protected Stack<LuaLoaderDelegate> loaderStack = new Stack<LuaLoaderDelegate>();

        [DoNotToLua]
        public static bool enableLuaComponent
        {
            get { return instance._enableLuaComponent; }
            set { instance._enableLuaComponent = value; }
        }
        protected bool _enableLuaComponent = false;

        public delegate byte[] LuaLoaderDelegate(string fn);
    }

    


    /*
    public class LuaEngine : BaseManager<LuaEngine>
    {
        protected override void Awake()
        {
            base.Awake();
            init(null, null, false);
        }

        protected override void OnDestroy()
        {
            if (_luaState != null)
            {
                _luaState.Close();
                _luaState = null;
            }
            LuaState.loaderDelegate = null;
            base.OnDestroy();
        }

        void Update()
        {
            tick();
        }

        public IEnumerator waitForDebugConnection(System.Action complete)
        {
            skipDebugger = false;
            Debug.Log("Waiting for debug connection");
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (skipDebugger) break;
            }
            complete();
        }

        private volatile int bindProgress = 0;
        private void doBind(object state)
        {
            IntPtr L = (IntPtr)state;

            Assembly[] ams = AppDomain.CurrentDomain.GetAssemblies();

            bindProgress = 0;

            List<Type> bindlist = new List<Type>();
            for (int n = 0; n < ams.Length; n++)
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
                    if (t.GetCustomAttributes(typeof(LuaBinderAttribute), false).Length > 0)
                    {
                        bindlist.Add(t);
                    }
                }
            }

            bindProgress = 1;

            bindlist.Sort(new System.Comparison<Type>((Type a, Type b) =>
            {
                LuaBinderAttribute la = (LuaBinderAttribute)a.GetCustomAttributes(typeof(LuaBinderAttribute), false)[0];
                LuaBinderAttribute lb = (LuaBinderAttribute)b.GetCustomAttributes(typeof(LuaBinderAttribute), false)[0];

                return la.order.CompareTo(lb.order);
            }));

            List<Action<IntPtr>> list = new List<Action<IntPtr>>();
            for (int n = 0; n < bindlist.Count; n++)
            {
                Type t = bindlist[n];
                var sublist = (Action<IntPtr>[])t.GetMethod("GetBindList").Invoke(null, null);
                list.AddRange(sublist);
            }

            bindProgress = 2;

            int count = list.Count;
            for (int n = 0; n < count; n++)
            {
                try
                {
                    Action<IntPtr> action = list[n];
                    action(L);
                    bindProgress = (int)(((float)n / count) * 98.0) + 2;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            bindProgress = 100;
        }

        public IEnumerator waitForBind(Action<int> tick, System.Action complete)
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

        void doinit(IntPtr L)
        {
            LuaTimer.reg(L);
            LuaCoroutine.reg(L, this);
            Helper.reg(L);
            //LuaValueType.reg(L);
            SLuaDebug.reg(L);
            LuaDLL.luaS_openextlibs(L);
            Lua3rdDLL.open(L);

            _inited = true;
        }

        void checkTop(IntPtr L)
        {
            if (LuaDLL.lua_gettop(luaState.L) != errorReported)
            {
                Debug.LogError("Some function not remove temp value from lua stack. You should fix it.");
                errorReported = LuaDLL.lua_gettop(luaState.L);
            }
        }

        public void init(Action<int> tick, System.Action complete, bool debug = false)
        {
            LuaState luaState = new LuaState();

            IntPtr L = luaState.L;
            LuaObject.init(L);

            ThreadPool.QueueUserWorkItem(doBind, L);

            StartCoroutine(waitForBind(tick, () =>
            {
                this._luaState = luaState;
                doinit(L);
                if (debug)
                {
                    StartCoroutine(waitForDebugConnection(() =>
                    {
                        if (complete != null) complete();
                        checkTop(L);
                    }));
                }
                else
                {
                    if(complete != null) complete();
                    checkTop(L);
                }
            }));
        }

        public object start(string main)
        {
            if (main != null)
            {
                luaState.doFile(main);
                LuaFunction func = (LuaFunction)luaState["main"];
                if (func != null)
                    return func.call();
            }
            return null;
        }

        void tick()
        {
            if (!inited)
                return;

            if (LuaDLL.lua_gettop(luaState.L) != errorReported)
            {
                errorReported = LuaDLL.lua_gettop(luaState.L);
                Debug.LogError(string.Format("Some function not remove temp value({0}) from lua stack. You should fix it.", LuaDLL.luaL_typename(luaState.L, errorReported)));
            }

            luaState.checkRef();
            LuaTimer.tick(Time.deltaTime);
        }

        public static LuaState luaState { get { return instance._luaState; } }
        protected LuaState _luaState;

        public static bool inited { get { return instance._inited; } }
        protected bool _inited = false;

        public static bool initLuaComponent { get { return instance._initLuaComponent; } set { instance._initLuaComponent = value; } }
        protected bool _initLuaComponent = false;

        int errorReported = 0;
        protected bool skipDebugger = true;
    }
    */
}
