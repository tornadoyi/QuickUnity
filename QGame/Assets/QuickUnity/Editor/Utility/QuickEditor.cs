using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace QuickUnity
{
    public class QuickEditor
    {
        /////////////////////////////
        // Draw property automatic //
        /////////////////////////////

        public static bool DrawProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(serializedObject, property, null, false, options);
        }

        public static bool DrawStructure(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
        {
            return DrawProperty(serializedObject, property, null, true, options);
        }

        public static void DrawProperties(SerializedObject serializedObject, params string[] properties)
        {
            for (uint i = 0; i < properties.Length; ++i)
            {
                DrawProperty(serializedObject, properties[i]);
            }
        }

        public static void DrawStructures(SerializedObject serializedObject, params string[] properties)
        {
            for (uint i = 0; i < properties.Length; ++i)
            {
                DrawStructure(serializedObject, properties[i]);
            }
        }

        public static bool DrawProperty(SerializedObject serializedObject, string property, string label, bool child, params GUILayoutOption[] options)
        {
            SerializedProperty sp = serializedObject.FindProperty(property);
            if (sp == null)
            {
                Debug.LogError(string.Format("Can not find property {0}", property));
                return false;
            }

            string alias = label != null ? label : sp.displayName;
            return EditorGUILayout.PropertyField(sp, new GUIContent(alias), child, options);
        }

        public static System.Object DrawVariable(string label, System.Object value, params GUILayoutOption[] options)
        {
            System.Object v = value;
            System.Object nv = null;

            Type t = v != null ? v.GetType() : typeof(UnityEngine.Object);
            if (t == typeof(int)) nv = EditorGUILayout.IntField(label, (int)v, options);
            else if (t == typeof(uint)) nv = (uint)EditorGUILayout.IntField(label, Convert.ToInt32(v), options);
            else if (t == typeof(short)) nv = (short)EditorGUILayout.IntField(label, Convert.ToInt32(v), options);
            else if (t == typeof(ushort)) nv = (ushort)EditorGUILayout.IntField(label, Convert.ToInt32(v), options);
            else if (t == typeof(long)) nv = EditorGUILayout.LongField(label, Convert.ToInt64(v), options);
            else if (t == typeof(ulong)) nv = (ulong)EditorGUILayout.LongField(label, Convert.ToInt64(v), options);
            else if (t == typeof(float)) nv = EditorGUILayout.FloatField(label, (float)v, options);
            else if (t == typeof(double)) nv = EditorGUILayout.DoubleField(label, (double)v, options);
            else if (t == typeof(bool)) nv = EditorGUILayout.Toggle(label, (bool)v, options);
            else if (t == typeof(string)) nv = EditorGUILayout.TextField(label, (string)v, options);
            else if (t == typeof(Bounds)) nv = EditorGUILayout.BoundsField(label, (Bounds)v, options);
            else if (t == typeof(Color)) nv = EditorGUILayout.ColorField(label, (Color)v, options);
            else if (t == typeof(Rect)) nv = EditorGUILayout.RectField(label, (Rect)v, options);
            else if (t == typeof(Vector2)) nv = EditorGUILayout.Vector2Field(label, (Vector2)v, options);
            else if (t == typeof(Vector3)) nv = EditorGUILayout.Vector3Field(label, (Vector3)v, options);
            else if (t == typeof(Vector4)) nv = EditorGUILayout.Vector4Field(label, (Vector4)v, options);
            else if (t.IsSubclassOf(typeof(UnityEngine.Object))) nv = EditorGUILayout.ObjectField(label, value as UnityEngine.Object, t, true, options);
            else { Debug.LogError(string.Format("Type {0} can not support", t)); return nv; }
            return nv;
        }

        public static bool CanTypeDraw(Type t)
        {
            if (t == null) return false;
            return (t == typeof(int) ||
                (t == typeof(uint)) ||
                (t == typeof(short)) ||
                (t == typeof(ushort)) ||
                (t == typeof(long)) ||
                (t == typeof(ulong)) ||
                (t == typeof(float)) ||
                (t == typeof(double)) ||
                (t == typeof(bool)) ||
                (t == typeof(string)) ||
                (t == typeof(Bounds)) ||
                (t == typeof(Color)) ||
                (t == typeof(Rect)) ||
                (t == typeof(Vector2)) ||
                (t == typeof(Vector3)) ||
                (t == typeof(Vector4)) ||
                (t.IsSubclassOf(typeof(UnityEngine.Object))));
        }


        ///////////////////////////
        // Widget for GUI Layout //
        ///////////////////////////

        public static LayoutFormat BeginContents(float h_space = 10.0f, float v_space = 2.0f)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
            GUILayout.Space(h_space);
            GUILayout.BeginVertical();
            GUILayout.Space(v_space);
            return new LayoutFormat(EndContents);
        }

        protected static void EndContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3f);
        }

        public static LayoutFormat BeginHorizontal(params GUILayoutOption[] options)
        {
            return BeginHorizontal(GUIStyle.none);
        }

        public static LayoutFormat BeginHorizontal(GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(style, options);
            return new LayoutFormat(EndHorizontal);
        }


        protected static void EndHorizontal()
        {
            EditorGUILayout.EndHorizontal();
        }

        public static LayoutFormat BeginVertical(params GUILayoutOption[] options)
        {
            return BeginVertical(GUIStyle.none, options);
        }

        public static LayoutFormat BeginVertical(GUIStyle style, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginVertical(style, options);
            return new LayoutFormat(EndVertical);
        }

        protected static void EndVertical()
        {
            EditorGUILayout.EndVertical();
        }

        public static LayoutFormat BeginScrollView(ref Vector2 scrollPosition, params GUILayoutOption[] options)
        {
            LayoutFormat format = new LayoutFormat(EndScrollView);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, options);
            return format;
        }

        protected static void EndScrollView()
        {
            EditorGUILayout.EndScrollView();
        }


        /// <summary>
        /// Selection
        /// </summary>
        public static List<T> GetSelectObjects<T>() where T : UnityEngine.Object
        {
            var list = new List<T>();
            if (Selection.objects.Length == 0) return list;

            System.Action<UnityEngine.Object> filter = (o) =>
            {
                var asset = o as T;
                if (asset == null) return;
                list.Add(asset);
            };

            // Path check
            bool pathFind = false;
            if (Selection.objects.Length == 1)
            {
                var o = Selection.objects[0];
                if (o.GetType() == typeof(UnityEditor.DefaultAsset))
                {
                    pathFind = true;
                }

            }

            if (!pathFind)
            {
                var objects = Selection.objects;
                foreach (var o in objects)
                {
                    filter(o);
                }
            }
            else
            {
                var rootPath = AssetDatabase.GetAssetPath(Selection.objects[0]);
                var absRootPath = FileManager.PathCombine(FileManager.projectPath, rootPath);
                var files = FileManager.GetFilesFromDirectory(absRootPath, true);
                foreach (var file in files)
                {
                    var relativePath = FileManager.GetRelativePath(file, FileManager.projectPath);
                    var asset = AssetDatabase.LoadAssetAtPath(relativePath, typeof(UnityEngine.Object));
                    filter(asset);
                }
            }
            return list;
        }


        /////////////////////
        // Inner functions //
        /////////////////////


        protected const float widthCharacter = 7.5f;



        public class LayoutFormat : System.IDisposable
        {
            public LayoutFormat(System.Action destroyAction)
            {
                this.destroyAction = destroyAction;
            }

            public void Dispose()
            {
                if (destroyAction != null) destroyAction();
            }

            System.Action destroyAction;
        }
    }



    
}


