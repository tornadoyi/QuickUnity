using UnityEngine;
using System.Collections;
using QuickUnity;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace QuickUnity
{
    public class SymbolManager : Singleton<SymbolManager>
    {
        public static bool CreateLibrary(string name)
        {
            if (string.IsNullOrEmpty(name)) { Debug.LogError("Invalid library name"); return false; }
            if (instance.libraries.ContainsKey(name)) return true;
            instance.libraries[name] = new Dictionary<string, string>();
            return true;
        }

        public static void DeleteLibrary(string name)
        {
            if(string.IsNullOrEmpty(name)) return;
            if (!instance.libraries.ContainsKey(name)) return;
            instance.libraries.Remove(name);
        }

        public static bool ContainLibrary(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            return instance.libraries.ContainsKey(name);
        }

        public static bool ContainSymbol(string libName, string key)
        {
            if (string.IsNullOrEmpty(libName)) { Debug.LogError("Invalid library name"); return false; }
            if (string.IsNullOrEmpty(key)) { return false; }
            Dictionary<string, string> lib = null;
            if (!instance.libraries.TryGetValue(libName, out lib)) { return false; }

            return lib.ContainsKey(key);
        }

        public static void AddSymbol(string libName, string key, string value)
        {
            if (string.IsNullOrEmpty(libName)) { Debug.LogError("Invalid library name"); return; }
            if (string.IsNullOrEmpty(key)) { Debug.LogError("Invalid key"); return; }
            if(value == null) { Debug.LogError("Invalid value"); return; }

            Dictionary<string, string> lib = null;
            if (!instance.libraries.TryGetValue(libName, out lib)) { Debug.LogError("Can not find symbol library " + libName); return; }

            lib[key] = value;
        }

        public static void AddSymbols(string libName, Dictionary<string, string> dict)
        {
            if (string.IsNullOrEmpty(libName)) { Debug.LogError("Invalid library name"); return; }
            if (dict == null) { Debug.LogError("Invalid symbol dictionary"); return; }

            Dictionary<string, string> lib = null;
            if (!instance.libraries.TryGetValue(libName, out lib)) { Debug.LogError("Can not find symbol library " + libName); return; }

            var e = dict.GetEnumerator();
            while(e.MoveNext())
            {
                lib[e.Current.Key] = e.Current.Value;
            }
        }

        public static void RemoveSymbol(string libName, string key)
        {
            if (string.IsNullOrEmpty(libName)) { Debug.LogError("Invalid library name"); return; }
            if (string.IsNullOrEmpty(key)) { Debug.LogError("Invalid key"); return; }

            Dictionary<string, string> lib = null;
            if (!instance.libraries.TryGetValue(libName, out lib)) { Debug.LogError("Can not find symbol library " + libName); return; }

            lib.Remove(key);
        }

        public static string Translate(string libName, string key)
        {
            if (string.IsNullOrEmpty(libName)) { Debug.LogError("Invalid library name"); return string.Empty; }
            if (string.IsNullOrEmpty(key)) { Debug.LogError("Invalid key"); return string.Empty; }

            Dictionary<string, string> lib = null;
            if (!instance.libraries.TryGetValue(libName, out lib)) return string.Empty;

            string value = null;
            if (!lib.TryGetValue(key, out value)) return string.Empty;

            return value;
        }


        public static string[] Translates(string libName, string key)
        {
            if (string.IsNullOrEmpty(libName)) { Debug.LogError("Invalid library name"); return new string[0]; }
            if (string.IsNullOrEmpty(key)) { Debug.LogError("Invalid key"); return new string[0]; }

            Dictionary<string, string> lib = null;
            if (!instance.libraries.TryGetValue(libName, out lib)) return new string[0];

            var keys = key.Split(separators);
            var values = new string[key.Length];
            for(int i=0; i< keys.Length; ++i)
            {
                string v = null;
                if (!lib.TryGetValue(keys[i], out v)) return new string[0];
                values[i] = v;
            }
            return values;
        }

        public static Dictionary<string, string> GetLibrary(string libName)
        {
            if(string.IsNullOrEmpty(libName)) { Debug.LogError("Invalid library name"); return new Dictionary<string, string>(); }

            Dictionary<string, string> lib = null;
            if (!instance.libraries.TryGetValue(libName, out lib)) return new Dictionary<string, string>();

            return lib;
        }

        public static string[] GetLibraryNames()
        {
            var array = new string[instance.libraries.Count];
            instance.libraries.Keys.CopyTo(array, 0);
            return array;
        }

        public static void NotifyUpdateSymbols()
        {
            for(int i=0; i< SceneManager.sceneCount; ++i)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;
                var roots = scene.GetRootGameObjects();
                for(int j=0; j< roots.Length; ++j)
                {
                    var widgets = roots[j].GetComponentsInChildren<SymbolWidget>();
                    for(int k=0; k<widgets.Length; ++k)
                    {
                        widgets[k].UpdateSymbol();
                    }
                }
            }
        }

        protected static char[] separators = new char[] { '/', '.'};
        protected Dictionary<string, Dictionary<string, string>> libraries = new Dictionary<string, Dictionary<string, string>>();
    }
}


