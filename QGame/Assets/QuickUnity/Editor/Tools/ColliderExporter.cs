using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

namespace QuickUnity
{
    public class ColliderExporter : EditorWindow
    {
        [MenuItem("QuickUnity/CI/Collider Exporter")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow(typeof(ColliderExporter));
        }

        void OnGUI()
        {
            //EditorGUILayout.Separator();
            //showCollider = GUILayout.Toggle(showCollider, "Show colliders");
            EditorGUILayout.Separator();

            if (GUILayout.Button("Bake"))
            {
                Bake();
            }
            EditorGUILayout.Separator();

            using(QuickEditor.BeginHorizontal())
            {
                if (GUILayout.Button("Save"))
                {
                    string filePath = EditorUtility.SaveFilePanel("Select collider file", Application.dataPath, "", "cld");
                    if (filePath != "")
                    {
                        Export(filePath);
                    }
                }

                if (GUILayout.Button("Load"))
                {
                    string filePath = EditorUtility.OpenFilePanel("Select collider file", Application.dataPath, "cld");
                    if (filePath != "")
                    {
                        Import(filePath);
                    }
                }
            }
            

            GUI.enabled = true;

        }

        void Update()
        {
            DrawData();
        }

        protected void Bake()
        {
            // Clear data
            boxDatas.Clear();

            // Find all collider
            Collider[] colliders = GameObject.FindObjectsOfType<Collider>();

            // Pick box collider
            List<BoxCollider> list = new List<BoxCollider>();
            for (int i = 0; i < colliders.Length; ++i)
            {
                Collider collider = colliders[i];
                string name = collider.GetType().Name;
                if (name == "BoxCollider")
                {
                    list.Add(collider as BoxCollider);

                }
            }

            // Calculate points
            for (int i = 0; i < list.Count; ++i)
            {
                BoxCollider collider = list[i];
                Transform transform = collider.transform;
                Vector3 radius = collider.size / 2;

                Vector3[] data = new Vector3[8];
                data[0] = transform.TransformPoint(collider.center + new Vector3(-radius.x, -radius.y, -radius.z));
                data[1] = transform.TransformPoint(collider.center + new Vector3(radius.x, -radius.y, -radius.z));
                data[2] = transform.TransformPoint(collider.center + new Vector3(radius.x, -radius.y, radius.z));
                data[3] = transform.TransformPoint(collider.center + new Vector3(-radius.x, -radius.y, radius.z));
                data[4] = transform.TransformPoint(collider.center + new Vector3(-radius.x, radius.y, radius.z));
                data[5] = transform.TransformPoint(collider.center + new Vector3(radius.x, radius.y, radius.z));
                data[6] = transform.TransformPoint(collider.center + new Vector3(radius.x, radius.y, -radius.z));
                data[7] = transform.TransformPoint(collider.center + new Vector3(-radius.x, radius.y, -radius.z));

                boxDatas.Add(data);
            }

            Debug.Log("Bake finished");
        }

        protected void Export(string path)
        {
            try
            {
                FileInfo fi = new FileInfo(path);
                if (fi.Exists)
                {
                    fi.Delete();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("{0}", e.ToString()));
                return;
            }

            // Create file
            FileStream fs = null;
            BinaryWriter bw = null;
            try
            {
                fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                bw = new BinaryWriter(fs);

                // Export
                bw.Write(boxDatas.Count);
                for (int i = 0; i < boxDatas.Count; ++i)
                {
                    Vector3[] data = boxDatas[i];
                    for (int j = 0; j < data.Length; ++j)
                    {
                        WriteVector3(bw, data[j]);
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            finally
            {
                // Close stream writer
                bw.Close();
                fs.Close();
            }

            

            Debug.Log("Export finished");
        }

        protected void Import(string path)
        {
            // Check file exist
            try
            {
                FileInfo fi = new FileInfo(path);
                if (!fi.Exists)
                {
                    Debug.LogError(string.Format("File {0} not exist", path));
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("{0}", e.ToString()));
                return;
            }

            FileStream fs = File.Open(path, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            RowReader rr = new RowReader(br.ReadBytes((int)fs.Length));
            fs.Close();

            // Read 
            boxDatas.Clear();
            int count = rr.ReadInt();
            for (int i = 0; i < count; ++i)
            {
                Vector3[] box = new Vector3[8];
                for (int j = 0; j < 8; ++j)
                {
                    Vector3 p = rr.ReadVector3();
                    box[j] = p;
                }
                boxDatas.Add(box);
            }

            br.Close();

            Debug.Log("Import finished");
        }

        void DrawData()
        {
            //if (!showCollider) return;
            Color color = Color.red;
            for (int i = 0; i < boxDatas.Count; ++i)
            {
                Vector3[] points = boxDatas[i];
                Debug.DrawLine(points[0], points[1], color);
                Debug.DrawLine(points[1], points[2], color);
                Debug.DrawLine(points[2], points[3], color);
                Debug.DrawLine(points[3], points[0], color);


                Debug.DrawLine(points[4], points[5], color);
                Debug.DrawLine(points[5], points[6], color);
                Debug.DrawLine(points[6], points[7], color);
                Debug.DrawLine(points[7], points[4], color);


                Debug.DrawLine(points[0], points[7], color);
                Debug.DrawLine(points[3], points[4], color);
                Debug.DrawLine(points[1], points[6], color);
                Debug.DrawLine(points[2], points[5], color);
            }
        }


        protected void WriteVector3(BinaryWriter bw, Vector3 v)
        {
            bw.Write(v.x);
            bw.Write(v.y);
            bw.Write(v.z);
        }

        protected List<Vector3[]> boxDatas = new List<Vector3[]>();
        protected bool showCollider = false;

    }
}

