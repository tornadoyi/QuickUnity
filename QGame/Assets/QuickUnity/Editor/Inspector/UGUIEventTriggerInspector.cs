using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UGUIEventTrigger), true)]
    public class UGUIEventTriggerInspector : EventTriggerInspector
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            events = new int[] {
                (int)EngineEvent.PointerEnter,
                (int)EngineEvent.PointerExit,
                (int)EngineEvent.PointerDown,
                (int)EngineEvent.PointerUp,
                (int)EngineEvent.PointerClick,
                (int)EngineEvent.Drag,
                (int)EngineEvent.Drop,
                (int)EngineEvent.Scroll,
                (int)EngineEvent.UpdateSelected,
                (int)EngineEvent.Select,
                (int)EngineEvent.Deselect,
                (int)EngineEvent.Move,
                (int)EngineEvent.InitializePotentialDrag,
                (int)EngineEvent.BeginDrag,
                (int)EngineEvent.EndDrag,
                (int)EngineEvent.Submit,
                (int)EngineEvent.Cancel
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var com = target as UGUIEventTrigger;
            com.interactable = EditorGUILayout.Toggle("Interactable", com.interactable);
            QuickEditor.DrawProperty(serializedObject, "selectables", "Selectable List", true);
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected override string OnEventToString(int id)
        {
            var e = (EngineEvent)id;
            return e.ToString();
        }
    }
}


