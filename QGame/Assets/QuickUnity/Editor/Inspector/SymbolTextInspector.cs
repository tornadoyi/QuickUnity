using UnityEngine;
using System.Collections;
using UnityEditor;
using QuickUnity;


[CanEditMultipleObjects]
[CustomEditor(typeof(SymbolText), true)]
public class SymbolTextInspector : Editor
{
    void OnEnable()
    {
        // Find text
        var com = target as SymbolText;
        if (com.text == null) com.text = com.GetComponent<UnityEngine.UI.Text>();


    }

    public override void OnInspectorGUI()
    {
        var com = target as SymbolText;
        serializedObject.Update();

        // Set symbol name
        QuickEditor.DrawProperty(serializedObject, "text");

        using (QuickEditor.BeginHorizontal())
        {
            QuickEditor.DrawProperty(serializedObject, "symbolText");
            var sp = serializedObject.FindProperty("symbolText");
            var isPresent = SymbolEditor.ContainSymbol(com.libraryName, sp.stringValue);
            GUI.color = isPresent ? Color.green : Color.red;
            GUILayout.Label(isPresent ? "\u2714" : "\u2718", "TL SelectionButtonNew", GUILayout.Height(20f));
            GUI.color = Color.white;
        }

        if (GUI.changed) EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }

}


