using UnityEngine;
using System.Collections;
using UnityEditor;
using QuickUnity;
using System.IO;

public class ScriptableObjectCreator : EditorWindow
{
    [MenuItem("Assets/Create/Scriptable Object")]
    public static void CreateScriptObject()
    {
        var selectedObject = Selection.activeObject;
        if (selectedObject == null) return;
        ScriptableObject obj = ScriptableObject.CreateInstance(selectedObject.name);
        if(obj == null)
        {
            Debug.LogErrorFormat("Can not create ScriptableObject {0}", selectedObject.name);
            return;
        }

        var assetName = Path.ChangeExtension(selectedObject.name, "asset");
        var savePath = EditorUtility.SaveFilePanel("Save File", Application.dataPath, assetName, string.Empty);
        if (string.IsNullOrEmpty(savePath)) return;
        var relativeSavePath = FileManager.GetRelativePath(savePath, FileManager.projectPath);
        
        if(File.Exists(savePath))
        {
            AssetDatabase.MoveAssetToTrash(relativeSavePath);
        }

        AssetDatabase.CreateAsset(obj, relativeSavePath);
        ScriptableObject sobj = AssetDatabase.LoadAssetAtPath(relativeSavePath, typeof(ScriptableObject)) as ScriptableObject;
        EditorUtility.SetDirty(sobj);
        Selection.activeObject = sobj;
    }

}