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

                if (GUILayout.Button("Build AssetBundles"))
                {
                    BuildAssetBundles();
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

        void BuildAssetBundles()
        {
            AssetBundleBuilder.BuildAssetBundles(target, config);
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
            var map = AssetBundleBuilder.CalculateAssetBundles(
                config.rootPath, config.ignoreFilePatterns, config.assetBundlePatterns, config.assetBundleExtension);

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

            [YamlMember(Alias = "file_extension_replace")]
            public List<string[]> fileExtReplace { get; set; }

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


        public class BuildConfigFile
        {
            public class AssetBundleItem
            {
                [YamlMember(Alias = "name")]
                public string name { get; set; }

                [YamlMember(Alias = "size")]
                public long size { get; set; }

                [YamlMember(Alias = "variant")]
                public string variant { get; set; }

                [YamlMember(Alias = "version")]
                public long version { get; set; }

                [YamlMember(Alias = "hash")]
                public string hash { get; set; }

                [YamlMember(Alias = "dependencies")]
                public string[] dependencies { get; set; }

                [YamlMember(Alias = "assets")]
                public string[] assets { get; set; }
            }

            [YamlMember(Alias = "asset_bundles")]
            public List<AssetBundleItem> assetBundles { get; set; }
        }

        public static void BuildAssetBundles(BuildTarget target, Config config)
        {
            var buildPath = FileManager.PathCombine(config.buildPath, target.ToString());
            if(!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);

            var assetList = CollectTargetAssets(config.rootPath, config.ignoreFilePatterns);
            ProcessFileExtensions(assetList, config.fileExtReplace, true);
            UpdateAssetBundleNames(config);

            var manifest = BuildPipeline.BuildAssetBundles(buildPath, config.buildOptions, target);
            var content = GenerateBuildConfigFile(buildPath, manifest);
            File.WriteAllText(Path.Combine(buildPath, "asset_table.yml"), content);

            ProcessFileExtensions(assetList, config.fileExtReplace, false);
        }


        public static void UpdateAssetBundleNames(Config config)
        {
            if (string.IsNullOrEmpty(config.rootPath) || config.assetBundlePatterns == null)
            {
                Debug.LogError("Please set root path and asset bundle reg ex");
                return;
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();

            var assetList = CollectTargetAssets(config.rootPath, config.ignoreFilePatterns);
            foreach (var path in assetList)
            {
                var assetBundleName = GenerateAssetBundleName(path, config.rootPath, config.assetBundlePatterns, config.assetBundleExtension);
                SaveAssetBundleName(path, assetBundleName);
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();

            System.GC.Collect();
        }


        public static Dictionary<string, List<string>> CalculateAssetBundles(string rootPath, string[] ignoreFilePatterns, string[] assetBundlePatterns, string assetBundleExtension)
        {
            var assetBundles = new Dictionary<string, List<string>>();

            var assetList = CollectTargetAssets(rootPath, ignoreFilePatterns);
            foreach (var path in assetList)
            {
                var assetBundleName = GenerateAssetBundleName(path, rootPath, assetBundlePatterns, assetBundleExtension);
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


        public static List<string> CollectTargetAssets(string rootPath, string[] ignoreFilePatterns)
        {
            var allList = AssetDatabase.GetAllAssetPaths().Where(path => path.StartsWith(rootPath)).ToList();
            var assetList = new List<string>();
            foreach(var asset in allList)
            {
                if (!IsTarget(asset, rootPath, ignoreFilePatterns)) continue;
                assetList.Add(asset);
            }
            return assetList;
        }

        public static bool IsTarget(string path, string rootPath, string[] ignoreFilePatterns)
        {
            if (string.IsNullOrEmpty(path) || Directory.Exists(path)) return false;

            var subPath = path.Substring(rootPath.Length);
            if (!path.StartsWith(rootPath))
            {
                return false;
            }

            if (ignoreFilePatterns != null)
            {
                foreach (var pattern in ignoreFilePatterns)
                {
                    if (Regex.IsMatch(subPath, pattern)) return false;
                }
            }
            return true;
        }

        public static string GenerateAssetBundleName(string path, string rootPath, string[] assetBundlePatterns, string assetBundleExtension)
        {
            var assetPath = path.Substring(rootPath.Length + 1); ;
            var directory = Path.GetDirectoryName(assetPath).ToLower();
            var filename = Path.GetFileNameWithoutExtension(assetPath).ToLower();

            foreach (var pattern in assetBundlePatterns)
            {
                var match = Regex.Match(directory, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return PathToAssetBundleName(match.Value, assetBundleExtension);
                }
            }

            return PathToAssetBundleName(Path.Combine(directory, filename), assetBundleExtension);
        }

        public static string PathToAssetBundleName(string path, string extension)
        {
            var assetBundlePath = Path.ChangeExtension(path, extension);
            assetBundlePath = assetBundlePath.Replace("\\", "/");
            assetBundlePath = assetBundlePath.ToLower();
            return assetBundlePath;
        }

        public static void SaveAssetBundleName(string path, string assetBundleName)
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

        public static void CleanAssetBundleName(string path)
        {
            var importer = AssetImporter.GetAtPath(path);
            if (!string.IsNullOrEmpty(importer.assetBundleName))
            {
                importer.assetBundleName = string.Empty;
                importer.assetBundleVariant = null;
            }
        }


        public static void ProcessFileExtensions(List<string> assets, List<string[]> replace, bool forward = true)
        {
            if (replace == null) return;
            for(int i=0; i< assets.Count; ++i)
            {
                var asset = assets[i];
                var ext = Path.GetExtension(asset).Replace(".", "");
                int src = 0;
                int dst = 1;
                if(!forward)
                {
                    src = 1;
                    dst = 0; 
                }
                foreach (var rep in replace)
                {
                    if (rep.Length < 2) continue;
                    if (ext != rep[src]) continue;
                    var newAsset = Path.ChangeExtension(asset, rep[dst]);
                    File.Move(asset, newAsset);
                    assets[i] = newAsset;
                    break;
                }
            }
            AssetDatabase.Refresh();
        }

        public static string GenerateBuildConfigFile(string buildPath, AssetBundleManifest manifest)
        {
            var serializer = new YamlDotNet.Serialization.Serializer();
            var stringBuilder = new StringBuilder();
            var stringWriter = new StringWriter(stringBuilder);
            var config = new BuildConfigFile();
            config.assetBundles = new List<BuildConfigFile.AssetBundleItem>();

            foreach (var assetBundleName in manifest.GetAllAssetBundles())
            {
                var item = new BuildConfigFile.AssetBundleItem();
                var assetBundlePath = FileManager.PathCombine(buildPath, assetBundleName);

                // Name
                item.name = FileManager.GetFilePathWithoutExtension(assetBundleName);

                // Size
                var fi = new System.IO.FileInfo(assetBundlePath);
                item.size = fi.Length;

                // Varriant
                item.variant = string.Empty;

                // Version
                item.version = 1;

                // Hash
                Hash128 hash = new Hash128();
                BuildPipeline.GetHashForAssetBundle(assetBundlePath, out hash);
                item.hash = hash.ToString();

                // Dependencies
                item.dependencies = manifest.GetAllDependencies(assetBundleName)
                    .Select(path => FileManager.GetFilePathWithoutExtension(path) ).ToArray();

                // Assets
                item.assets = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);

                config.assetBundles.Add(item);
            }

            serializer.Serialize(stringWriter, config);
            return stringBuilder.ToString();
        }
    }
}
