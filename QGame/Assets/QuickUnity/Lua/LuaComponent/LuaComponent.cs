using UnityEngine;
using System.Collections;
using SLua;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

namespace QuickUnity
{
    public partial class LuaComponent : QuickBehaviour, IEventReceiver
    {
        protected virtual void Awake()
        {
            if(LuaEngine.enableLuaComponent)
            {
                if (string.IsNullOrEmpty(luaClassName)) return;
                ConnectLuaClass();
                if (onAwake != null) onAwake.call(_self);
            }
            else
            {
                Action.ActionBase action = null;
                action = Schedule(0.0f, () =>
                {
                    if (!LuaEngine.enableLuaComponent) return;
                    if (string.IsNullOrEmpty(luaClassName)) return;
                    UnSchedule(action);
                    ConnectLuaClass();
                    if (onAwake != null) onAwake.call(_self);
                    Start();
                });
                return;
            }
        }

        protected void Start()
        {
            if (onStart != null) onStart.call(_self);
        }

        void OnDestroy()
        {
            if (onDestroy != null) onDestroy.call(_self);
        }

        void OnDisable()
        {
            if (onDisable != null) onDisable.call(_self);
        }

        void OnEnable()
        {
            if (onEnable != null) onEnable.call(_self);
        }

        public void Invoke(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
            {
                Debug.LogError("Invalid function name");
                return; 
            }
            if(_self == null)
            {
                Debug.LogError("Lua has not connected");
                return;
            }
            var luaFunc = _self[functionName] as LuaFunction;
            if (luaFunc == null) return;
            luaFunc.call(_self);
        }

        public void ConnectLuaClass(string className)
        {
            if(string.IsNullOrEmpty(className))
            {
                Debug.LogError("lua class name is invalid to connect");
                return;
            }
            if(!string.IsNullOrEmpty(luaClassName))
            {
                Debug.LogError("Can not connect again");
                return;
            }
            _luaClassName = className;
            ConnectLuaClass();
            if (onAwake != null) onAwake.call(_self);
        }

        protected void ConnectLuaClass()
        {
            // Instantiate lua class
            string fname = string.Format("{0}.{1}", luaClassName, QConfig.Lua.instanceFunctionName);
            LuaFunction creator = (LuaFunction)LuaEngine.GetLuaObject(fname);
            if (creator == null) { Debug.LogError(string.Format("Can not init {0}", luaClassName)); return; }
            _self = (LuaTable)creator.call(this);
            if (_self == null) { Debug.LogError(string.Format("Can not init {0}", luaClassName)); return; }

            // Bind events
            onAwake = (LuaFunction)_self["Awake"];
            onStart = (LuaFunction)_self["Start"];
            onDestroy = (LuaFunction)_self["OnDestroy"];
            onDisable = (LuaFunction)_self["OnDisable"];
            onEnable = (LuaFunction)_self["OnEnable"];

            onUpdate = (LuaFunction)_self["Update"];
            onLateUpdate = (LuaFunction)_self["LateUpdate"];
            onFixedUpdate = (LuaFunction)_self["FixedUpdate"];

            // Set variables
            LuaTable table = _self;
            SyncVariables(0, table);
        }

        void SyncVariables(int index, LuaTable table)
        {
            var e = variables.GetEnumerator(index);
            while (e.MoveNext())
            {
                if(e.Current.isArray)
                {
                    var newTable = new LuaTable(LuaState.main);
                    table[e.Current.name] = newTable;
                    SyncVariables(e.Current.index, newTable);
                }
                else
                {
                    table[e.Current.name] = e.Current.value;
                }
            }
        }

        public void OnProcessEvent(int eventID, object parm, string functionName, EventManager source)
        {
            if(string.IsNullOrEmpty(functionName))
            {
                Debug.LogErrorFormat("Can not dispatch event {0}, Function name is null", eventID);
            }
            var f = (LuaFunction)_self[functionName];
            if (f == null) return;
            f.call(_self, parm, source);
        }

        public string luaClassName { get { return _luaClassName; } }
        [SerializeField]
        protected string _luaClassName;

        public LuaTable luaClassObject { get { return _self; } }
        protected LuaTable _self;

        protected LuaFunction onAwake;
        protected LuaFunction onStart;
        protected LuaFunction onDestroy;
        protected LuaFunction onDisable;
        protected LuaFunction onEnable;

        protected LuaFunction onUpdate;
        protected LuaFunction onLateUpdate;
        protected LuaFunction onFixedUpdate;

        [SerializeField]
        protected XType variables = XType.Array();
    }

    public enum LuaComponentType
    {
        Common = 0,
        Update,
        LateUpdate,
        FixedUpdate,
        Update_Fixed,
        Update_Late,
        Late_Fixed,
        Update_Late_Fixed,
    }



#if UNITY_EDITOR
    public partial class LuaComponent : QuickBehaviour
    {
        [DoNotToLua]
        public void SetLuaClassName(string name) { _luaClassName = name; }

        [DoNotToLua]
        public XType GetVariables() { return variables; }

       
        void OnDrawGizmosSelected()
        {
            if (enabled == false) return;
            if (onDrawGizmosSelected != null) onDrawGizmosSelected();
        }

        [DoNotToLua]
        public QEventHandler onDrawGizmosSelected = null;
    }

#endif
}

