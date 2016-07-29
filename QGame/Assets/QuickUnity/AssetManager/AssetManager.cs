using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace QuickUnity
{
    public enum AssetUnloadLevel { AssetBundles = 1, Assets = 2, All = 3, Default = 3 }

    public partial class AssetManager : BaseManager<AssetManager>
    {
        public static string streamingAssetsPath { get; private set; }
        public static string assetServerUrl { get; private set;}
        public static string downloadCachePath { get; private set; }
        public static string streamingAssetsTablePath { get; private set; }
        public static string serverTableUrl { get; private set; }
        public static string serverTablePath { get; private set; }

        protected AssetTable assetTable;
        protected Dictionary<string, AssetBundleInfo> loadedBundleDict = new Dictionary<string, AssetBundleInfo>();
        protected Dictionary<string, AssetInfo> loadedAssetDict = new Dictionary<string, AssetInfo>();


        public static bool init
        {
            get
            {
                if (!QConfig.Asset.loadAssetFromAssetBundle)
                {
                    return true;
                }
                return instance.assetTable != null;
            }
        }

        public static void Start(
            string streamingAssetsPath, 
            string assetServerUrl, 
            string downloadCachePath,
            string streamingAssetsTablePath, 
            string serverTableUrl,
            string serverTablePath)
        {
            AssetManager.streamingAssetsPath = streamingAssetsPath;
            AssetManager.assetServerUrl = assetServerUrl;
            AssetManager.downloadCachePath = downloadCachePath;
            AssetManager.streamingAssetsTablePath = streamingAssetsTablePath;
            AssetManager.serverTableUrl = serverTableUrl;
            AssetManager.serverTablePath = serverTablePath;
        }

        public static Task LoadLocalAssetTable() { return new LoadLocalAssetTableTask(streamingAssetsTablePath).Start(); }

        public static Task LoadServerAssetTable(bool download) { return new LoadServerAssetTableTask(serverTableUrl, serverTablePath, download).Start(); }


        public static DownloadAssetBundleTask DownloadAssetBundle(string name)
        {
            DownloadAssetBundleTask task = new DownloadAssetBundleTask(name);
            task.Start();
            return task;
        }

        public static LoadAssetBundleTask LoadAssetBundle(string name)
        {
            LoadAssetBundleTask task = new LoadAssetBundleTask(name);
            task.Start();
            return task;
        }

        public static Object LoadAsset(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("Can not load asset, asset name is null");
                return null;
            }
            var info = instance.assetTable.GetAssetInfo(assetName);
            if (info == null)
            {
                info = new LocalAssetInfo(assetName);
                instance.assetTable.AddAssetInfo(info);
            }

            info.Load();
            return info.asset;
        }

        public static Object[] LoadSubAssets(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("Can not load asset, asset name is null");
                return null;
            }
            var info = instance.assetTable.GetAssetInfo(assetName);
            if (info == null)
            {
                info = new LocalAssetInfo(assetName);
                instance.assetTable.AddAssetInfo(info);
            }

            info.Load();
            return info.subAssets;
        }

        public static LoadAssetTask LoadAssetAsync(string assetName)
        {
            LoadAssetTask task = new LoadAssetTask(assetName);
            task.Start();
            return task;
        }

        public static Object Instantiate(string name)
        {
            Object asset = LoadAsset(name);
            if (asset == null) return null;
            return GameObject.Instantiate(asset);
        }

        public static T Instantiate<T>(string name) where T : Object
        {
            var obj = Instantiate(name);
            if (obj == null) return null;
            return obj as T;
        }

        public static AssetUnloadTask UnloadUnusedResources() { return UnloadUnusedResources(AssetUnloadLevel.Default); }

        public static AssetUnloadTask UnloadUnusedResources(AssetUnloadLevel level)
        {
            var task = new AssetUnloadTask(level);
            task.Start();
            return task;
        }

        public static void UnloadAllResources()
        {
            // Unload all assets
            {
                var infos = new List<AssetInfo>();
                infos.AddRange(instance.loadedAssetDict.Values);
                for (int i = 0; i < infos.Count; ++i)
                {
                    var info = infos[i];
                    info.Unload();
                }
            }

            // Unload all asset bundles
            {
                var infos = new List<AssetBundleInfo>();
                infos.AddRange(instance.loadedBundleDict.Values);
                for (int i = 0; i < infos.Count; ++i)
                {
                    var info = infos[i];
                    info.Unload(true);
                }
            }
        }

            
        public static AssetBundle GetAssetBundle(string name)
        {
            var info = GetAssetBundleInfo(name);
            return info != null ? info.assetBundle : null;
        }

        public static void SetAssetBundleKeepTag(string name, bool isKeep)
        {
            var info = GetAssetBundleInfo(name);
            if (info == null) { return; }
            info.keepTag = isKeep;
        }

        public static void SetAssetKeepTag(string name, bool isKeep)
        {
            var info = GetAssetInfo(name);
            if (info == null) { return; }
            info.keepTag = isKeep;
        }

#if UNITY_EDITOR
        [SLua.DoNotToLua]
        public static AssetTable GetAssetTable() { return instance == null ? null : instance.assetTable; }
#endif

        protected override void Awake()
        {
            base.Awake();
            assetTable = null;
            AssetBundleInfo.bundleLoad += OnBundleLoad;
            AssetBundleInfo.bundleUnLoad += OnBundleUnload;
            AssetInfo.assetLoad += OnAssetLoad;
            AssetInfo.assetUnLoad += OnAssetUnload;
        }

        protected override void OnDestroy()
        {
            UnloadAllResources();

            AssetBundleInfo.bundleLoad -= OnBundleLoad;
            AssetBundleInfo.bundleUnLoad -= OnBundleUnload;
            AssetInfo.assetLoad -= OnAssetLoad;
            AssetInfo.assetUnLoad -= OnAssetUnload;

            base.OnDestroy();
        }


        protected void OnBundleLoad(System.Object o)
        {
            AssetBundleInfo assetBundleInfo = o as AssetBundleInfo;
            if (assetBundleInfo == null || !assetBundleInfo.loaded) return;
            loadedBundleDict[assetBundleInfo.name] = assetBundleInfo;
            //Debug.Log(string.Format("Bundle {0} has been load", assetBundleInfo.name));
        }

        protected void OnBundleUnload(System.Object o)
        {
            AssetBundleInfo assetBundleInfo = o as AssetBundleInfo;
            if (assetBundleInfo == null || assetBundleInfo.loaded) return;
            loadedBundleDict.Remove(assetBundleInfo.name);
            //Debug.Log(string.Format("Bundle {0} has been unload", assetBundleInfo.name));
        }

        protected void OnAssetLoad(System.Object o)
        {
            AssetInfo assetInfo = o as AssetInfo;
            if (assetInfo == null || !assetInfo.loaded) return;
            loadedAssetDict[assetInfo.name] = assetInfo;
            //Debug.Log(string.Format("Asset {0} has been load", assetInfo.name));
        }

        protected void OnAssetUnload(System.Object o)
        {
            AssetInfo assetInfo = o as AssetInfo;
            if (assetInfo == null || assetInfo.loaded) return;
            loadedAssetDict.Remove(assetInfo.name);
            //Debug.Log(string.Format("Asset {0} has been unload", assetInfo.name));
        }


        protected static AssetInfo GetAssetInfo(string name) { return instance.assetTable.GetAssetInfo(name); }

        protected static AssetBundleInfo GetAssetBundleInfo(string name) { return instance.assetTable.GetBundleInfo(name); }
   
    }

    

}
