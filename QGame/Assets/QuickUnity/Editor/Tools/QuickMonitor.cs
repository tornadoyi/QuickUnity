using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace QuickUnity
{
    public class QuickMonitor : EditorWindow
    {
        protected string searchText;
        protected int tabIndex = 0;

        protected Vector2 scrollPos;
        

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
                if (GUILayout.Toggle(tabIndex == 0, "Task", "ButtonLeft")) ChangeTab(0);
                if (GUILayout.Toggle(tabIndex == 1, "Asset", "ButtonRight")) ChangeTab(1);
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

        void ChangeTab(int index)
        {
            if (tabIndex == index) return;
            tabIndex = index;
            searchText = string.Empty;
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
                        var state = task.sleep ? "Sleep" : task.running ? "Running" : "Finish";
                        EditorGUILayout.LabelField(state, style, option);
                    }
                }

                {// Last time
                    //var option = GUILayout.Width(100f);
                    if (task == null) GUILayout.Button("Last time", "miniButtonRight");
                    else
                    {
                        var content = string.Format("{0}s", task.costTime);
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
                    var option = GUILayout.Width(200f);
                    if (info == null) GUILayout.Button("Name", "miniButtonLeft", option);
                    else
                    {
                        EditorGUILayout.SelectableLabel(info.name, style, option);
                    }
                }


                {// Type
                    var option = GUILayout.Width(150);
                    if (info == null) GUILayout.Button("Type", "miniButtonMid", option);
                    else
                    {
                        EditorGUILayout.SelectableLabel(info.pathType.ToString(), style, option);
                    }
                }

                {// Reference
                    var option = GUILayout.Width(80);
                    if (info == null) GUILayout.Button("Reference", "miniButtonRight");
                    else
                    {
                        EditorGUILayout.SelectableLabel(info.reference.ToString(), style);
                    }
                }

            };

            // Draw menu
            using (QuickEditor.BeginHorizontal())
            {
                draw_row(null);
            }


            // Draw contents
            var table = AssetManager.GetAssetTable();
            if (table == null ) return;
            searchText = EditorGUILayout.TextField("", searchText, "SearchTextField");
            using (QuickEditor.BeginScrollView(ref scrollPos))
            {
                Dictionary<string, AssetBundleInfo>.Enumerator e = table.bundleDict.GetEnumerator();
                while (e.MoveNext())
                {
                    AssetBundleInfo info = e.Current.Value;
                    if (!string.IsNullOrEmpty(searchText) &&
                        info.name.IndexOf(searchText, System.StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        continue;
                    }

                    GUI.backgroundColor = info.loaded ? new Color(0.51f, 1.0f, 0.59f) : new Color(0.75f, 0.75f, 0.75f);
                    using (QuickEditor.BeginHorizontal("As TextArea", GUILayout.MinHeight(20f)))
                    {
                        draw_row(info);
                    }
                }
            }
        }

       
        
    }
}

