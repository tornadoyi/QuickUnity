using UnityEngine;
using System.Collections;
using UnityEditor;

namespace QuickUnity
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SortingLayerOverride), true)]
    public class SortingLayerOverrideInspector : Editor
    {

        public override void OnInspectorGUI()
        {
            var com = target as SortingLayerOverride;

            // Sorting Layer
            var layers = UnityEngine.SortingLayer.layers;
            string[] layerNames = new string[layers.Length];
            int index = 0;
            for (int i = 0; i < layers.Length; ++i)
            {
                layerNames[i] = layers[i].name;
                if (com.sortingLayerID == layers[i].id) index = i;
            }
            index = EditorGUILayout.Popup("Sorting Layer", index, layerNames);
            com.sortingLayerID = layers[index].id;
            com.SetSortingLayer(com.sortingLayerID);

            // Sorting Order
            com.sortingOrder = EditorGUILayout.IntField("Sorting Order", com.sortingOrder);
            com.SetSortingOrder(com.sortingOrder);

            // Renderer
            QuickEditor.DrawProperty(serializedObject, "render");

            if (GUI.changed) EditorUtility.SetDirty(target);

        }
    }
}