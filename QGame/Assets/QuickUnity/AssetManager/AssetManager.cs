using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace QuickUnity
{
    public partial class AssetManager : BaseManager<AssetManager>
    {
        public static string streamingAssetsPath { get; private set; }
        public static string serverAssetPath { get; private set;}
        public static string downloadUrl { get; private set; }

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

        public static Task Start(
            string streamingAssetsPath, 
            string serverAssetPath, 
            string downloadUrl,
            string streamingAssetsTablePath, 
            string serverTablePath)
        {
            AssetManager.streamingAssetsPath = streamingAssetsPath;
            AssetManager.serverAssetPath = serverAssetPath;
            AssetManager.downloadUrl = downloadUrl;

            AssetManagerInitTask task = new AssetManagerInitTask(streamingAssetsTablePath, serverTablePath);
            task.Start();
            return task;
        }

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


        public static void GetRuntimeInfo(out Dictionary<string, AssetBundleInfo> bundleDict, out Dictionary<string, AssetInfo> assetDict)
        {
            if(Application.isPlaying)
            {
                bundleDict = instance.loadedBundleDict;
                assetDict = instance.loadedAssetDict;
            }
            else
            {
                bundleDict = new Dictionary<string, AssetBundleInfo>();
                assetDict = new Dictionary<string, AssetInfo>();
            }
            
        }


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



        public class DownloadAssetBundleTask : CoroutineTask
        {
            public string name { get; private set;}

            public DownloadAssetBundleTask(string name)
            {
                this.name = name;
            }

            protected override IEnumerator OnProcess()
            {
                if (!QConfig.Asset.loadAssetFromAssetBundle)
                {
                    yield break;
                }

                AssetBundleInfo info = AssetManager.instance.assetTable.GetBundleInfo(name);
                if (info == null)
                {
                    SetFail(string.Format("Can not find AssetBundle {0}", name));
                    yield break;
                }

                Task task = info.Download();
                yield return task.WaitForFinish();
                if (task.fail) { SetFail(task.error); }
            }
        }

        public class LoadAssetBundleTask : CoroutineTask
        {
            public UnityEngine.Object asset { get; private set;}
            public UnityEngine.Object[] subAssets { get; private set;}
            public string name { get; private set;}

            public LoadAssetBundleTask(string name)
            {
                this.name = name;
            }

            protected override IEnumerator OnProcess()
            {
                if (!QConfig.Asset.loadAssetFromAssetBundle)
                {
                    yield break;
                }
                    
                AssetBundleInfo info = AssetManager.instance.assetTable.GetBundleInfo(name);
                if (info == null)
                {
                    SetFail(string.Format("Can not find AssetBundle {0}", name));
                    yield break;
                }

                Task task = info.LoadAsync();
                yield return task.WaitForFinish();
                if (task.fail) { SetFail(task.error); }
            }
        }

        public class LoadAssetTask : CoroutineTask
        {
            public string name { get; private set;}
            public UnityEngine.Object asset { get; private set; }
            public UnityEngine.Object[] subAssets { get; private set; }

            public LoadAssetTask(string name)
            {
                name = string.IsNullOrEmpty(name) ? string.Empty : name;
            }

            protected override IEnumerator OnProcess()
            {

                var info = AssetManager.GetAssetInfo(name);
                if (info == null)
                {
                    info = new LocalAssetInfo(name);
                    instance.assetTable.AddAssetInfo(info);
                }

                if (!info.loaded)
                {
                    var task = info.LoadAsync();
                    yield return task.WaitForFinish();
                    if (task.fail)
                    {
                        SetFail("Load asset failed", task.error);
                        yield break;
                    }
                }
                asset = info.asset;
                subAssets = info.subAssets;
            }
              
        }


        public class AssetManagerInitTask : CoroutineTask
        {
            public string builtInAssetTablePath { get; private set; }

            public string externalAssetTablePath { get; private set; }


            public AssetManagerInitTask(string builtInAssetTablePath, string externalAssetTablePath)
            {
                this.builtInAssetTablePath = builtInAssetTablePath;
                this.externalAssetTablePath = externalAssetTablePath;
            }

            protected override IEnumerator OnProcess()
            {
                if (!QConfig.Asset.loadAssetFromAssetBundle)
                {
                    AssetManager.instance.assetTable = new AssetTable();
                    yield break;
                }
                    
                AssetTable builtinTable = new AssetTable();
                {
                    Task task = builtinTable.LoadAsync(
                        builtInAssetTablePath, 
                        QConfig.Asset.AssetPathType.StreamingAssets);
                    yield return task.WaitForFinish();
                }

                AssetTable externalTable = new AssetTable();
                {
                    Task task = externalTable.LoadAsync(
                        externalAssetTablePath,
                        QConfig.Asset.AssetPathType.Server);
                    yield return task.WaitForFinish();
                }

                // merge table
                AssetTable assetTable = AssetTable.Merge(builtinTable, externalTable); 

                assetTable.Analyse();
                AssetManager.instance.assetTable = assetTable;
            }
                
        }


        public class AssetUnloadTask : CoroutineTask
        {
            public AssetUnloadLevel level { get; private set; }

            public AssetUnloadTask(AssetUnloadLevel level)
            {
                this.level = level;
            }

            protected override IEnumerator OnProcess()
            {
                if (!QConfig.Asset.loadAssetFromAssetBundle)
                {
                    Resources.UnloadUnusedAssets();
                    yield break;
                }

                if ((level & AssetUnloadLevel.AssetBundles) > 0) UnloadUnusedAssetBundles();
                if ((level & AssetUnloadLevel.Assets) > 0) UnloadUnusedAssets();

                var async = Resources.UnloadUnusedAssets();
                yield return async;
                System.GC.Collect();
                
            }

            protected static void UnloadUnusedAssets()
            {
                if (!QConfig.Asset.loadAssetFromAssetBundle)
                {
                    return;
                }

                List<AssetInfo> infos = new List<AssetInfo>();
                infos.AddRange(instance.loadedAssetDict.Values);
                for (int i = 0; i < infos.Count; ++i)
                {
                    AssetInfo info = infos[i];
                    if (!info.unused) continue;
                    info.Unload();
                }
            }


            protected static void UnloadUnusedAssetBundles()
            {
                if (!QConfig.Asset.loadAssetFromAssetBundle)
                {
                    return;
                }

                List<AssetBundleInfo> infos = new List<AssetBundleInfo>();
                infos.AddRange(instance.loadedBundleDict.Values);
                for (int i = 0; i < infos.Count; ++i)
                {
                    AssetBundleInfo info = infos[i];
                    if (!info.unused) continue;
                    info.Unload(true);
                }
            }

        }
            
    }

    public enum AssetUnloadLevel { AssetBundles = 1, Assets = 2, All = 3, Default = 3 }

}
