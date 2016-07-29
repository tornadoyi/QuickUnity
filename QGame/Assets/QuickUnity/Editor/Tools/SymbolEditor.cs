using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using QuickUnity;
using System.Collections.Generic;
using SimpleJson;

namespace QuickUnity
{
    public class SymbolEditor : EditorWindow
    {
        [MenuItem("QuickUnity/Tools/Symbol Editor")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            var window = EditorWindow.GetWindow<SymbolEditor>();
            window.Init();
        }

        public void Init()
        {
            cacheSymbolFiles.Clear();
            foreach (var item in symbolFileNames)
            {
                var key = item.Key;
                var list = item.Value;
                var textList = new List<TextAsset>();
                cacheSymbolFiles.Add(key, textList);
                foreach(var path in list)
                {
                    textList.Add(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
                }
            }
        }


        void OnGUI()
        {
            bool sync = false;
            var keys = new List<string>();
            foreach(var key in cacheSymbolFiles.Keys) { keys.Add(key); }
            foreach (var key in keys)
            {
                var list = cacheSymbolFiles[key];
                int count = EditorGUILayout.IntField(new GUIContent(key), list.Count);
                if(count != list.Count)
                {
                    var minCount = Mathf.Min(count, list.Count);
                    var newlist = new List<TextAsset>();
                    for(int i=0; i< count; ++i)
                    {
                        if(i < minCount)
                        {
                            newlist.Add(list[i]);
                        }
                        else
                        {
                            newlist.Add(null);
                        }
                    }
                    list = newlist;
                    cacheSymbolFiles[key] = list;
                    sync = true;
                }
                for (int i=0; i<list.Count; ++i)
                {
                    var asset = list[i];

                    using (QuickEditor.BeginHorizontal())
                    {
                        GUILayout.Space(10);
                        asset = EditorGUILayout.ObjectField(new GUIContent(i.ToString()), asset, typeof(TextAsset), false) as TextAsset;
                    }
                    if(asset != list[i])
                    {
                        list[i] = asset;
                        sync = true;
                    }
                }
            }

            if(sync)
            {
                symbolFileNames.Clear();
                foreach (var item in cacheSymbolFiles)
                {
                    var key = item.Key;
                    var list = item.Value;
                    var paths = new List<string>();
                    foreach(var asset in list)
                    {
                        if (asset == null) continue;
                        var path = AssetDatabase.GetAssetPath(asset);
                        paths.Add(path);
                    }
                    symbolFileNames.Add(key, paths);
                }
                SaveSymbolFileNames();
                Reload();
            }
        }

        
        static void Reload()
        {
            libraries.Clear();
            LoadSymbolFileNames();
            foreach (var key in symbolKeys)
            {
                var dict = new Dictionary<string, string>();
                libraries.Add(key, dict);
                List<string> paths = null;
                if (!symbolFileNames.TryGetValue(key, out paths)) continue;
                foreach(var path in paths)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    if (asset == null) continue;
                    var bytes = asset.bytes;
                    var reader = new KVReader(bytes);
                    var sdict = reader.ReadDictionary();
                    foreach(var item in sdict) { dict[item.Key] = item.Value; }
                }
            }
            Debug.Log("Reload symbol files");
        }


        static void LoadSymbolFileNames()
        {
            symbolFileNames.Clear();

            foreach (var key in symbolKeys)
            {
                var list = new List<string>();
                symbolFileNames.Add(key, list);
            }

            var jsonStr = EditorPrefs.GetString("Q_SYMBOL_CONFIG", string.Empty);
            if (string.IsNullOrEmpty(jsonStr)) return;
            var jtable = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(jsonStr);
            
            foreach(var item in symbolFileNames)
            {
                var key = item.Key;
                var list = item.Value;
                var array = jtable.GetArray(key);
                foreach(var jstr in array)
                {
                    var path = jstr as string;
                    list.Add(path);
                }
            }
        }

        static void SaveSymbolFileNames()
        {
            var json = new SimpleJson.JsonObject();
            foreach (var item in symbolFileNames)
            {
                var key = item.Key;
                var list = item.Value;

                var jpaths = new SimpleJson.JsonArray();
                json.Add(key, jpaths);

                foreach (var path in list)
                {
                    jpaths.Add(path);
                }
            }
            EditorPrefs.SetString("Q_SYMBOL_CONFIG", json.ToString());
        }


        public static bool ContainSymbol(string libName, string key)
        {
            if (string.IsNullOrEmpty(libName)) { Debug.LogError("Invalid library name"); return false; }
            if (string.IsNullOrEmpty(key)) { return false; }
            Dictionary<string, string> lib = null;
            if (!libraries.TryGetValue(libName, out lib)) { return false; }

            return lib.ContainsKey(key);
        }

        protected static Dictionary<string, Dictionary<string, string>> libraries = new Dictionary<string, Dictionary<string, string>>();

        private static Dictionary<string, List<string>> symbolFileNames = new Dictionary<string, List<string>>();
        
        private static List<string> symbolKeys = new List<string>() { QConfig.Symbol.textLibrary, QConfig.Symbol.assetLibrary };


        private Dictionary<string, List<TextAsset>> cacheSymbolFiles = new Dictionary<string, List<TextAsset>>();

        private static bool startupReload = false;

        [InitializeOnLoad]
        public class Startup
        {
            static Startup()
            {
                if (startupReload) return;
                Reload();
                startupReload = true;
            }
        }


        public class AssetPostProcessLanguage : AssetPostprocessor
        {
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                bool reload = false;
                foreach (var path in importedAssets)
                {
                    foreach(var item in symbolFileNames)
                    {
                        if (!item.Value.Contains(path)) continue;
                        Debug.LogFormat("Detected {0} changed", path);
                        reload = true;
                    }
                }
                if (reload) Reload();
            }
        }







    }
}



