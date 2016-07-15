using UnityEngine;
using System.Collections;
using UnityEditor;

namespace QuickUnity
{
    public class QEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawOnInspector();

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed) EditorUtility.SetDirty(target);

        }

        protected virtual void DrawOnInspector() { }
    }
}


