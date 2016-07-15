using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace QuickUnity
{
    public class QuickMonitor : EditorWindow
    {
        [MenuItem("QuickUnity/Tools/Monitor")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow<QuickMonitor>();
        }

        void OnSelectionChange() { Repaint(); }

        void OnGUI()
        {
            using (QuickEditor.BeginHorizontal())
            {
                if (GUILayout.Toggle(tabIndex == 0, "Task", "ButtonLeft")) tabIndex = 0;
                if (GUILayout.Toggle(tabIndex == 1, "Asset", "ButtonRight")) tabIndex = 1;
            }

            GUILayout.Space(10);

            switch(tabIndex)
            {
                case 0: { DrawTaskMonitor(); break; }
                case 1: { DrawAssetMonitor(); break; }
            }
        }

        void Update()
        {
            Repaint();
        }

        void DrawTaskMonitor()
        {
            System.Action<Task> draw_row = (task) =>
            {
                var style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;

                {// Name
                    var option = GUILayout.Width(300f);
                    if (task == null) GUILayout.Button("Name", "miniButtonLeft", option);
                    else
                    {
                        EditorGUILayout.LabelField(task.GetType().Name, style, option);
                    }
                }

                {// State
                    var option = GUILayout.Width(100f);
                    if (task == null) GUILayout.Button("State", "miniButtonMid", option);
                    else
                    {
                        EditorGUILayout.LabelField(task.state.ToString(), style, option);
                    }
                }

                {// Last time
                    //var option = GUILayout.Width(100f);
                    if (task == null) GUILayout.Button("Last time", "miniButtonRight");
                    else
                    {
                        var content = string.Format("{0}s", task.lastTime);
                        EditorGUILayout.LabelField(content, style);
                    }
                }
            };

            // Draw menu
            using (QuickEditor.BeginHorizontal())
            {
                draw_row(null);
            }

            // Draw contents
            var taskList = TaskMonitor.GetRuntimeInfo();
            foreach(var task in taskList)
            {
                using (QuickEditor.BeginHorizontal("As TextArea", GUILayout.MinHeight(20f)))
                {
                    draw_row(task);
                }   
            }
        }

        void DrawAssetMonitor()
        {
            System.Action<AssetBundleInfo> draw_row = (info) =>
            {
                var style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;

                {// Name
                    var option = GUILayout.Width(100f);
                    if (info == null) GUILayout.Button("Name", "miniButtonLeft", option);
                    else
                    {
                        EditorGUILayout.LabelField(info.name, style, option);
                    }
                }


                {// Type
                    var option = GUILayout.Width(100f);
                    if (info == null) GUILayout.Button("Type", "miniButtonMid", option);
                    else
                    {
                        EditorGUILayout.LabelField(info.pathType.ToString(), style, option);
                    }
                }

                {// Path
                    if (info == null) GUILayout.Button("Path", "miniButtonRight");
                    else
                    {
                        EditorGUILayout.LabelField(info.relativePath, style);
                    }
                }
            };

            // Draw menu
            using (QuickEditor.BeginHorizontal())
            {
                draw_row(null);
            }

            // Draw contents
            Dictionary<string, AssetBundleInfo> bundleDict = null;
            Dictionary<string, AssetInfo> assetDict = null;
            AssetManager.GetRuntimeInfo(out bundleDict, out assetDict);

            using(QuickEditor.BeginScrollView(ref scrollPos))
            {
                Dictionary<string, AssetBundleInfo>.Enumerator e = bundleDict.GetEnumerator();
                while (e.MoveNext())
                {
                    AssetBundleInfo info = e.Current.Value;
                    using(QuickEditor.BeginHorizontal("As TextArea", GUILayout.MinHeight(20f)))
                    {
                        draw_row(info);
                    }
                }
            }
            
        }

        void DrawAsset()
        {
            Dictionary<string, AssetBundleInfo> bundleDict = null;
            Dictionary<string, AssetInfo> assetDict = null;
            AssetManager.GetRuntimeInfo(out bundleDict, out assetDict);

            using (QuickEditor.BeginScrollView(ref scrollPos))
            {
                Dictionary<string, AssetInfo>.Enumerator e = assetDict.GetEnumerator();
                while (e.MoveNext())
                {
                    AssetInfo info = e.Current.Value;
                    using (QuickEditor.BeginHorizontal())
                    {
                        EditorGUILayout.LabelField(info.name);
                        EditorGUILayout.LabelField("loaded");
                    }
                }
            }
        }

        protected Vector2 scrollPos;

        protected int tabIndex = 0;
    }
}

