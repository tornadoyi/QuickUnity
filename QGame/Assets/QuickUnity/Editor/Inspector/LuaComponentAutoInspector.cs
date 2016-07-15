using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace QuickUnity
{
    public class LuaComponentAutoInspector : LuaComponentVariableInspector
    {
        public override void Init(LuaComponent target, SerializedObject serializedObject)
        {
            base.Init(target, serializedObject);

            var variables = target.GetVariables();
            xTypeEditor = new XTypeInspector(variables);
            xTypeEditor.drawWithoutOutermostArray = true;
            xTypeEditor.repeatedNameCheckInArray = true;
            xTypeEditor.forbidNamelessXtype = true;
        }

        protected override void OnDrawVariables()
        {
            
            xTypeEditor.Draw();

        }

        protected XTypeInspector xTypeEditor;

    }
}
