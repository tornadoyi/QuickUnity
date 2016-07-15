using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace QuickUnity
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LuaComponent), true)]
    public class LuaComponentInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawOnInspector();

            if (GUI.changed) EditorUtility.SetDirty(target);

        }

        protected virtual void DrawOnInspector()
        {
            LuaComponent com = target as LuaComponent;
            var className = EditorGUILayout.TextField("Lua class name", com.luaClassName);
            Type type = comProperty != null ? comProperty.GetType() : typeof(LuaComponentAutoInspector);
            if(className != com.luaClassName)
            {
                com.SetLuaClassName(className);

                // Find editor
                var editorType = AssemblyHelper.GetType("Assembly-CSharp-Editor", com.luaClassName + "Editor");
                if (editorType != null) type = editorType;
            }

            // Create Editor
            if (comProperty == null || comProperty.GetType().Name != type.Name)
            {
                comProperty = Activator.CreateInstance(type) as LuaComponentVariableInspector;
                if (comProperty == null) return;
                comProperty.Init(target as LuaComponent, serializedObject);
                (target as LuaComponent).onDrawGizmosSelected = OnDrawGizmosSelected;
            }
            comProperty.DrawVariables();            
        }

        protected void OnDrawGizmosSelected()
        {
            if (comProperty == null) return;
            comProperty.DrawGizmosSelected();
        }

        public static void DrawLuaVariable(string label, LuaVariable variable, params GUILayoutOption[] options)
        {
            if (variable == null) { Debug.LogError("Variable must not be null"); return; }

            if(variable.isArrayTag)
            {
                if (variable.isArrayBegin)
                {
                    variable.foldout = EditorGUILayout.Foldout(variable.foldout, label);
                }
                else if (variable.isArrayEnd)
                {
                    // Nothing to do
                }
            }
            else if(variable.isValue)
            {
                System.Object v = variable.GetValue();
                System.Object nv = null;
                if (variable.isCommonValue)
                {
                    nv = QuickEditor.DrawVariable(label, v);
                    if (v != nv) { variable.SetValue(nv); }
                }
                else if (variable.isUnityObject)
                {
                    var type = AssemblyHelper.GetTypeByModule(variable.moduleName, variable.typeName);
                    nv = EditorGUILayout.ObjectField(label, v as UnityEngine.Object, type, true, options);
                    if (v != nv) { variable.SetValue(nv); }
                }
            }
            else { Debug.LogError("Invalid lua variable"); return; }
        }

        protected LuaComponentVariableInspector comProperty;
    }


    public class LuaComponentVariableInspector
    {
        public virtual void Init(LuaComponent target, SerializedObject serializedObject)
        {
            // Base
            this.target = target;
            this.serializedObject = serializedObject;
        }


        /// <summary>
        ///  Variable Functions
        /// </summary>
        //protected LuaVariable GetVariable(string name) { return target.GetVariable(name); }
        //protected LuaVariable DefineVariable(string name, Type type, int index = -1) { return target.AddVariable(name, type, index); }
        //protected LuaVariable DefineVariable(string name, System.Object value, int index = -1) { return target.AddVariable(name, value, index); }
        //protected LuaVariable DefineArrayVariable(string name) { return target.AddArrayVariable(name); }

        


        /// <summary>
        /// Events
        /// </summary>
        public void DrawVariables() { OnDrawVariables(); }
        public void DrawGizmosSelected() { OnDrawGizmosSelected(); }
        protected virtual void OnDefineVariables() { }
        protected virtual void OnDrawVariables() { }
        protected virtual void OnDrawGizmosSelected() { }



        /// <summary>
        /// Component
        /// </summary>
        protected LuaComponent target;
        protected SerializedObject serializedObject;
    }    
}

