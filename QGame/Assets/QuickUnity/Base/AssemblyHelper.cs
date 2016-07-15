using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace QuickUnity
{
    public class AssemblyHelper
    {
        public static System.Object GetDefaultValue(string assemblyQualifiedName)
        {
            return GetDefaultValue(Type.GetType(assemblyQualifiedName));
        }
        public static System.Object GetDefaultValue(string moduleName, string typeName)
        {
            var t = GetTypeByModule(moduleName, typeName);
            return GetDefaultValue(t);
        }

        public static System.Object GetDefaultValue(Type t)
        {
            if (t == null) return null;
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }

        public static Type GetTypeByModule(string moduleName, string typeName)
        {
            var assemblyName = moduleName.Replace(".dll", "");
            return GetType(assemblyName, typeName);
        }
        public static Type GetType(string assemblyName, string typeName)
        {
            var assembly = GetOrLoadAssembly(assemblyName);
            if (assembly == null) return null;
            return assembly.GetType(typeName, false, true);
        }

        public static List<Type> LoadAssemblyTypes(string assemblyName, Func<Type, bool> filter = null)
        {
            var list = new List<Type>();
            var assembly = GetOrLoadAssembly(assemblyName);
            if (assembly == null) return list;
            Type[] types = assembly.GetExportedTypes();
            for (int i = 0; i < types.Length; ++i)
            {
                var t = types[i];
                if (filter != null && !filter(t)) continue;
                list.Add(t);
            }
            return list;
        }

        public static Assembly GetOrLoadAssembly(string assemblyName)
        {
            Assembly assembly = null;
            if(assemblyDict.TryGetValue(assemblyName, out assembly))
            {
                return assembly;
            }
            assembly = Assembly.Load(assemblyName);
            if (assembly == null) return null;
            assemblyDict.Add(assemblyName, assembly);
            return assembly;
        }

        protected static Dictionary<string, Assembly> assemblyDict = new Dictionary<string, Assembly>();
    }
}

