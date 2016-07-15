using UnityEngine;
using System.Collections;

namespace QuickUnity
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [RequireComponent(typeof(Renderer))]
    public class SortingLayerOverride : MonoBehaviour
    {
        public void Awake()
        {
            if (render == null) render = GetComponent<Renderer>();
            SetSortingLayer(sortingLayerID);
            SetSortingOrder(sortingOrder);
        }

        public void SetSortingLayer(int id)
        {
            if (!UnityEngine.SortingLayer.IsValid(id)) { Debug.LogError("Invalid sorting layer id " + id); return; }
            render.sortingLayerID = id;
        }

        public void SetSortingLayer(string name)
        {
            if (render == null || string.IsNullOrEmpty(name)) return;
            int id = UnityEngine.SortingLayer.GetLayerValueFromName(name);
            if (!UnityEngine.SortingLayer.IsValid(id)) { Debug.LogError("Invalid sorting layer name " + name); return; }
            render.sortingLayerID = id;
        }

        public void SetSortingOrder(int order)
        {
            if (render == null) return;
            render.sortingOrder = order;
        }

        public int sortingLayerID;
        public int sortingOrder;
        public bool includeChildren;

        public Renderer render;
    }
}


