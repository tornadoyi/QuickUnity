using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace QuickUnity
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SymbolWidget), true)]
    public class SymbolWidgetInspector : Editor
    {
        public bool CheckSymbolValid(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return true;
        }
    }
}

