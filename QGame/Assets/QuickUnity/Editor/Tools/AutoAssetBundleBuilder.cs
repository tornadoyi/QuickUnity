using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Text;
using SimpleJson;
using System.IO;
using Excel;
using System.Data;
using System.Text.RegularExpressions;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace QuickUnity
{
    public class AutoAssetBundleBuilderWindow : EditorWindow
    {
        public class AssetBundleItem
        {
            public string name { get; set; }
            public List<string> assets { get; set; }
            public bool foldout {get; set;}
    }


        protected BuildTarget target = BuildTarget.WebPlayer;
        protected string configFilePath;

        protected AssetBundleBuilder.Config config;
        protected List<AssetBundleItem> assetBundles;
        protected string searchText;
        protected Vector2 scrollPos;

        protected SerializedObject serializedObject
        {
            get
            {
                if (_serializedObject != null) return _serializedObject;
                _serializedObject = new UnityEditor.SerializedObject(this);
                return _serializedObject;
            }
        }
        protected SerializedObject _serializedObject;

        [MenuItem("QuickUnity/CI/Auto AssetBundle Builder")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            EditorWindow.GetWindow<AutoAssetBundleBuilderWindow>();
        }

        void OnGUI()
        {
            // Asset table name
            target = (BuildTarget)EditorGUILayout.EnumPopup("Bundle Target", target);

            EditorGUILayout.TextField("Config Path", configFilePath);

            // Update asset bundle names
            if (config != null)
            {
                EditorGUILayout.TextField("Build Path", config.buildPath);

                EditorGUILayout.TextField("Asset Bundle Extension", config.assetBundleExtension);

                string options = "";
                foreach (var opt in config.buildOptionDescs) options += string.Format("{0} | ", opt);
                EditorGUILayout.TextField("Build Options", options);


                if (GUILayout.Button("Update AssetBundle Names"))
                {
                    UpdateAssetBundleNames();
                }
            }


            // Load config
            using (QuickEditor.BeginHorizontal())
            {
                if (GUILayout.Button("Load config"))
                {
                    configFilePath = EditorUtility.OpenFilePanel("Select config file", Application.dataPath, "yml");
                    if (!string.IsNullOrEmpty(configFilePath))
                    {
                        LoadConfig(configFilePath);
                    }
                }

                if (GUILayout.Button("Reload"))
                {
                    if(string.IsNullOrEmpty(configFilePath) ||
                        !File.Exists(configFilePath))
                    {
                        configFilePath = EditorUtility.OpenFilePanel("Select config file", Application.dataPath, "yml");
                        if (!string.IsNullOrEmpty(configFilePath))
                        {
                            LoadConfig(configFilePath);
                        }
                    }
                    else
                        LoadConfig(configFilePath);
                }
            }
                

            // Show Asset Bundles
            DrawAssetBundles();

        }

        void DrawAssetBundles()
        {
            if (assetBundles == null) return;
            searchText = EditorGUILayout.TextField("", searchText, "SearchTextField");
            using (QuickEditor.BeginScrollView(ref scrollPos))
            {
                var bundleNameStyle = EditorStyles.foldout;
                bundleNameStyle.fontStyle = FontStyle.Bold;
                foreach (var item in assetBundles)
                {
                    if (!string.IsNullOrEmpty(searchText) &&
                        item.name.IndexOf(searchText, System.StringComparison.CurrentCultureIgnoreCase) == -1)
                    {
                        continue;
                    }
                    using (var v = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        item.foldout = EditorGUILayout.Foldout(item.foldout, item.name, bundleNameStyle);
                        if (!item.foldout) continue;
                        foreach(var asset in item.assets)
                        {
                            EditorGUILayout.SelectableLabel(string.Format(" - {0}", asset));
                        }
                    }
                }
            }
        }

        void UpdateAssetBundleNames()
        {
            if (config == null)
            {
                Debug.LogError("Please load config first");
                return;
            }
            AssetBundleBuilder.UpdateAssetBundleNames(config);
        }

        void LoadConfig(string configFilePath)
        {
            var document = File.ReadAllText(configFilePath);
            var input = new StringReader(document);
            var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
            config = deserializer.Deserialize<AssetBundleBuilder.Config>(input);
            var map = AssetBundleBuilder.CalculateAssetBundles(config);

            config.buildPath = FileManager.FormatLinuxPathSeparator(System.IO.Path.GetFullPath(config.buildPath));

            assetBundles = new List<AssetBundleItem>();
            foreach (var kv in map)
            {
                var item = new AssetBundleItem();
                item.name = kv.Key;
                item.assets = kv.Value;
                assetBundles.Add(item);
            }
        }
    }


    public class AssetBundleBuilder
    {
        public class Config
        {
            [YamlMember(Alias = "build_path")]
            public string buildPath { get; set; }

            [YamlMember(Alias = "asset_bundle_ext")]
            public string assetBundleExtension { get; set; }

            [YamlMember(Alias = "root_path")]
            public string rootPath { get; set; }

            [YamlMember(Alias = "asset_bundle_pattern")]
            public string[] assetBundlePatterns { get; set; }

            [YamlMember(Alias = "ignore_file_pattern")]
            public string[] ignoreFilePatterns { get; set; }

            [YamlMember(Alias = "build_options")]
            public string[] buildOptionDescs { get; set; }

            public BuildAssetBundleOptions buildOptions
            {
                get
                {
                    BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
                    if (buildOptionDescs == null) return options;
                    foreach (var desc in buildOptionDescs)
                    {
                        options |= (BuildAssetBundleOptions)Enum.Parse(typeof(BuildAssetBundleOptions), desc);
                    }
                    return options;
                }
            }
        }

        public static void BuildAssetBundles(string buildPath, BuildTarget target, Config config)
        {
            UpdateAssetBundleNames(config);
            BuildPipeline.BuildAssetBundles(buildPath, config.buildOptions, target);
        }


        public static void UpdateAssetBundleNames(Config config)
        {
            if (string.IsNullOrEmpty(config.rootPath) || config.assetBundlePatterns == null)
            {
                Debug.LogError("Please set root path and asset bundle reg ex");
                return;
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();

            var assetList = AssetDatabase.GetAllAssetPaths().Where(path => path.StartsWith(config.rootPath)).ToList();
            foreach (var path in assetList)
            {
                UpdateAssetBundleName(path, config);
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();

            System.GC.Collect();
        }

        public static void UpdateAssetBundleName(string path, Config config)
        {
            if (Directory.Exists(path)) return;

            if (!IsTarget(path, config))
            {
                CleanAssetBundleName(path);
                return;
            }

            var assetBundleName = GenerateAssetBundleName(path, config);
            SaveAssetBundleName(path, assetBundleName);
        }


        public static Dictionary<string, List<string>> CalculateAssetBundles(Config config)
        {
            var assetBundles = new Dictionary<string, List<string>>();

            var assetList = AssetDatabase.GetAllAssetPaths().Where(path => path.StartsWith(config.rootPath)).ToList();
            foreach (var path in assetList)
            {
                if (Directory.Exists(path) || !IsTarget(path, config)) continue;
                var assetBundleName = GenerateAssetBundleName(path, config);
                List<string> assets;
                if(!assetBundles.TryGetValue(assetBundleName, out assets))
                {
                    assets = new List<string>();
                    assetBundles[assetBundleName] = assets;
                }
                assets.Add(path);
            }
            return assetBundles;
        }



        private static bool IsTarget(string path, Config config)
        {
            var subPath = path.Substring(config.rootPath.Length);
            if (!path.StartsWith(config.rootPath))
            {
                return false;
            }

            if (config.ignoreFilePatterns != null)
            {
                foreach (var pattern in config.ignoreFilePatterns)
                {
                    if (Regex.IsMatch(subPath, pattern)) return false;
                }
            }
            return true;
        }

        private static string GenerateAssetBundleName(string path, Config config)
        {
            var assetPath = path.Substring(config.rootPath.Length + 1); ;
            var directory = Path.GetDirectoryName(assetPath).ToLower();
            var filename = Path.GetFileNameWithoutExtension(assetPath).ToLower();


            foreach (var pattern in config.assetBundlePatterns)
            {
                var match = Regex.Match(directory, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return PathToAssetBundleName(match.Value, config.assetBundleExtension);
                }
            }

            return PathToAssetBundleName(Path.Combine(directory, filename), config.assetBundleExtension);
        }

        private static string PathToAssetBundleName(string path, string extension)
        {
            var assetBundlePath = Path.ChangeExtension(path, extension);
            assetBundlePath = assetBundlePath.Replace("\\", "/");
            assetBundlePath = assetBundlePath.ToLower();
            return assetBundlePath;
        }

        private static void SaveAssetBundleName(string path, string assetBundleName)
        {
            var importer = AssetImporter.GetAtPath(path);
            if (importer.assetBundleName != assetBundleName)
            {
                importer.assetBundleName = assetBundleName;
                importer.assetBundleVariant = null;
                //importer.SaveAndReimport();
                Debug.Log(string.Format("Update AB Name: file: {0} [{1}] -> [{2}]", path, importer.assetPath, importer.assetBundleName));
            }
        }

        private static void CleanAssetBundleName(string path)
        {
            var importer = AssetImporter.GetAtPath(path);
            if (!string.IsNullOrEmpty(importer.assetBundleName))
            {
                importer.assetBundleName = string.Empty;
                importer.assetBundleVariant = null;
            }
        }
    }
}
