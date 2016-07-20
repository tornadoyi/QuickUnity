using UnityEngine;
using System.Collections;
using System;

namespace QuickUnity
{
    public static class LuaUtility
    {
        // Operation
        public static int And(int x , int y) { return x & y; }

        public static int Or(int x, int y) { return x | y;}

        public static int Xor(int x, int y) { return x ^ y; }

        public static int Not(int x) { return ~x; }

        public static int RShift(int x, int c) { return x >> c; }

        public static int LShift(int x, int c) { return x << c; }


        // Type and Enum
        public static Type GetType(System.Object o) { return o != null ? o.GetType() : null; }

        public static string GetEnumName(Type enumType, int value) { return Enum.GetName(enumType, value); }
        public static string GetUnityEngineClassAQ(string className)
        {
            if (string.IsNullOrEmpty(className)) { Debug.LogError("className is null"); return string.Empty; }
            return string.Format(unityEngineAQ, className);
        }
        

        private static string unityEngineAQ
        {
            get
            {
                if (!string.IsNullOrEmpty(_unityEngineAQ)) return _unityEngineAQ;
                var aq = typeof(UnityEngine.Transform).AssemblyQualifiedName;
                _unityEngineAQ = aq.Replace("Transform", "{0}");
                return _unityEngineAQ;
            }
        }
        private static string _unityEngineAQ;
    }
}

