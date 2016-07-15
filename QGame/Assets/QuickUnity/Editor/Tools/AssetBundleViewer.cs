using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace QuickUnity
{
    public class AssetBundleViewer : EditorWindow
    {
        [MenuItem("QuickUnity/CI/AssetBundle Viewer")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow<AssetBundleViewer>();
        }


        void OnGUI()
        {
            using (QuickEditor.BeginHorizontal())
            {
                EditorGUILayout.TextField("AssetBundle Path", assetBundlePath);
                if (GUILayout.Button("Open", GUILayout.MaxWidth(100)))
                {
                    assetBundlePath = EditorUtility.OpenFilePanel("Select AssetBundle", Application.dataPath, "unity3d");
                    if (!string.IsNullOrEmpty(assetBundlePath))
                    {
                        bundleContent = OpenAssetBundle(assetBundlePath);
                    }

                }
            }
            

            if(bundleContent != null)
            {
                EditorGUILayout.TextField("Path", bundleContent.path);
                for(int i=0; i<bundleContent.assetNames.Length; ++i)
                {
                    var name = bundleContent.assetNames[i];
                    EditorGUILayout.TextField(i.ToString(), name);
                }
            }
        }

        protected AssetBundleContent OpenAssetBundle(string path)
        {
            var bytes = FileManager.LoadBinaryFile(path);
            if (bytes == null) return null;
            var assetBundle = AssetBundle.LoadFromMemory(bytes);
            if (assetBundle == null) return null;

            var content = new AssetBundleContent();
            content.path = path;
            content.assetNames = assetBundle.GetAllAssetNames();
            assetBundle.Unload(true);
            return content;
        }

        protected class AssetBundleContent
        {
            public string path;
            public string[] assetNames = new string[0];
        }

        protected string assetBundlePath = string.Empty;
        protected AssetBundleContent bundleContent = null;
    }
}

