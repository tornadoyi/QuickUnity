using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public class AssetBundleInfo
    {
        public string name { get; private set;}
        public long size { get; private set;}
        public string variant { get; private set;}
        public long version { get; private set;}
        public string hash { get; private set;}
        public QConfig.Asset.AssetPathType pathType { get; private set;}
        public List<string> dependList { get; private set;}

        public bool fromStreamingAssets { get { return pathType == QConfig.Asset.AssetPathType.StreamingAssets; } }
        public bool fromAssetServer { get { return pathType == QConfig.Asset.AssetPathType.AssetServer; } }

        public string url
        {
            get
            {
                var rootPath = fromAssetServer ? AssetManager.assetServerUrl : AssetManager.streamingAssetsPath;
                string format = QConfig.Asset.useVersionAsFileName ? "{0}.{1}.{2}" : "{0}.{2}";
                return string.Format(
                    format,
                    FileManager.PathCombine(rootPath, name),
                    version,
                    QConfig.Asset.assetBundleSuffix);
            }
        }
        public string cachePath
        {
            get
            {
                string format = QConfig.Asset.useVersionAsFileName ? "{0}.{1}.{2}" : "{0}.{2}";
                return string.Format(
                    format,
                    FileManager.PathCombine(AssetManager.downloadCachePath, name),
                    version,
                    QConfig.Asset.assetBundleSuffix);
            }
        }
           
        public Dictionary<string, AssetInfo> assetDict { get; private set;}
        public Dictionary<string, AssetBundleInfo> dependBundleDict { get; private set;}

        public AssetBundle assetBundle { get; private set;}

        public bool unused { get { return (!keepTag && reference == 0 && assetLoadingCount == 0 && !loading); } }
        public bool loading { get { return loadTask != null; } }
        public bool loaded { get { return assetBundle != null; } }
        public bool downloading { get { return downloadTask != null; } }

        // Keep tag
        public bool keepTag = false;

        protected LoadOrDownloadAssetBundleTask loadTask;
        protected DownloadAssetBundleTask downloadTask;

        public int reference { get; private set;}
        public int assetLoadingCount = 0;

        public bool fileExists {get{ return fromStreamingAssets ? true : System.IO.File.Exists(cachePath);;}}

        /// <summary>
        /// Event for bundle load and unload
        /// </summary>
        public static event QEventHandler1 bundleLoad;
        public static event QEventHandler1 bundleUnLoad;



        public AssetBundleInfo(
            string name, 
            long size, 
            string variant, 
            long version, 
            string hash, 
            List<string> dependList,
            List<string> assetList,
            QConfig.Asset.AssetPathType pathType)
        {
            this.name = name;
            this.size = size;
            this.variant = variant;
            this.version = version;
            this.hash = hash;
            this.dependList = dependList;
            this.pathType = pathType;

            assetDict = new Dictionary<string, AssetInfo>();
            dependBundleDict = new Dictionary<string, AssetBundleInfo>();

            for (int i = 0; i < assetList.Count; ++i)
            {
                var asset = new BundleAssetInfo(assetList[i], this);
                AddAssetInfo(asset);
            }
        }

        public bool AddDependency(AssetBundleInfo bundle)
        {
            if (bundle == null)
            {
                Debug.LogError("Invalid bundle");
                return false;
            }

            if (dependBundleDict.ContainsKey(bundle.name)) return true;

            dependBundleDict.Add(bundle.name, bundle);

            return true;
        }

        public Task Download()
        {
            // Downloading
            if(downloading) return downloadTask;

            // Start download
            downloadTask = new DownloadAssetBundleTask(this, true);
            downloadTask.Finish(_ => downloadTask = null);
            downloadTask.Start();
            return downloadTask;
        }

        public Task LoadAsync()
        {
            // Loaded then return
            if (loaded)
            {
                var t = new LoadOrDownloadAssetBundleTask(this);
                t.Start();
                return t;
            }

            // Loading then wait
            if (loading) return loadTask;

            // Retain all depend bundle
            var e = dependBundleDict.GetEnumerator();
            while(e.MoveNext())
            {
                e.Current.Value.Retain();
            }

            // Create task
            loadTask = new LoadOrDownloadAssetBundleTask(this);
            loadTask.Finish((_) =>
            {
                var task = loadTask;
                loadTask = null;
                assetBundle = task.assetBundle;
                if (assetBundle != null) bundleLoad(this);
            });
            loadTask.Start();
            return loadTask;
        }

        public bool Unload(bool unloadDepend = false)
        {
            // Check
            if (!unused)
            {
                Debug.LogError(string.Format("Bundle {0} cannot unload, it is not unused", name));
                return false;
            }
            if (assetBundle == null) return true;

            // Unload bundle
            assetBundle.Unload(false);
            assetBundle = null;
            bundleUnLoad.Invoke(this);

            // Release all depend bundle
            if(unloadDepend)
            {
                Dictionary<string, AssetBundleInfo>.Enumerator e = dependBundleDict.GetEnumerator();
                while (e.MoveNext())
                {
                    AssetBundleInfo bundleInfo = e.Current.Value;
                    bundleInfo.Release();
                    if (bundleInfo.unused) bundleInfo.Unload(true);
                }
            }
            
            return true;
        }


        public void Retain() { ++reference; }
        public void Release()
        {
            if (reference == 0)
            {
                Debug.LogError("Invalid release, current reference is 0");
                return;
            }
            --reference;
        }

        private bool AddAssetInfo(BundleAssetInfo asset)
        {
            if (asset == null)
            {
                Debug.LogError("Invalid asset");
                return false;
            }

            if (asset.bundleInfo != this)
            {
                Debug.LogError(string.Format("Asset {0} has save in bundle {1}", asset.name, asset.bundleInfo.name));
                return false;
            }

            if (assetDict.ContainsKey(asset.name))
            {
                Debug.LogError(string.Format("Repeated asset {0} in bundle", asset.name, name));
                return false;
            }
            assetDict.Add(asset.name, asset);

            return true;
        }



        // =================================== Task =================================== //


        protected class LoadOrDownloadAssetBundleTask : CoroutineTask
        {
            public AssetBundleInfo assetBundleInfo { get; private set; }
            public AssetBundle assetBundle { get; private set; }

            public LoadOrDownloadAssetBundleTask(AssetBundleInfo assetBundleInfo)
            {
                this.assetBundleInfo = assetBundleInfo;
            }

            protected override IEnumerator OnProcess()
            {
                // Loaded then return
                if (assetBundleInfo.loaded)
                {
                    assetBundle = assetBundleInfo.assetBundle;
                    yield break;
                }

                // Download (self and dependencies)
                {
                    var task = assetBundleInfo.Download();
                    yield return task.WaitForFinish();
                    if (task.fail)
                    {
                        SetFail(string.Format("Load AssetBundle {0} fialed", assetBundleInfo.name), task.error);
                        yield break;
                    }
                }

                // Load dependencies
                {
                    // Load
                    var list = new List<Task>();
                    var e = assetBundleInfo.dependBundleDict.GetEnumerator();
                    while (e.MoveNext())
                    {
                        var task = e.Current.Value.LoadAsync();
                        list.Add(task);
                    }

                    // Wait load finished
                    for(int i=0; i<list.Count; ++i)
                    {
                        var task = list[i];
                        yield return task.WaitForFinish();
                        if (task.success) continue;
                        SetFail(string.Format("Load AssetBundle {0} fialed", assetBundleInfo.name), task.error);
                        yield break;
                    }
                }


                // Load self when all dependencies has loaded before
                {
                    var task = new LoadAssetBundleTask(assetBundleInfo.url, assetBundleInfo.pathType, assetBundleInfo.hash);
                    yield return task.Start().WaitForFinish();
                    if(task.fail)
                    {
                        SetFail(string.Format("Load AssetBundle {0} fialed", assetBundleInfo.name), task.error);
                        yield break;
                    }
                    assetBundle = task.assetBundle;
                }
            }
        }


        protected class LoadAssetBundleTask : CoroutineTask
        {
            public AssetBundle assetBundle { get; private set; }
            public string path { get; private set; }
            public string md5 { get; private set; }
            public QConfig.Asset.AssetPathType pathType { get; private set; }
            public string expectMD5 { get; private set; }
            public LoadAssetBundleTask(string path, QConfig.Asset.AssetPathType pathType)
            {
                this.path = path;
                this.pathType = pathType;
            }

            public LoadAssetBundleTask(string path, QConfig.Asset.AssetPathType pathType, string expectMD5)
            {
                this.path = path;
                this.pathType = pathType;
                this.expectMD5 = expectMD5;
            }

            protected override IEnumerator OnProcess()
            {
                // Load self
                byte[] bytes = null;
                switch (pathType)
                {
                    case QConfig.Asset.AssetPathType.AssetServer:
                        {
                            FileReadBytesTask task = new FileReadBytesTask(path);
                            yield return task.Start().WaitForFinish();
                            if (task.bytes == null)
                            {
                                SetFail(string.Format("Can not load {0} bundle from {1}", pathType.ToString(), path), task.error);
                                yield break;
                            }
                            bytes = task.bytes;

                        }
                        break;

                    case QConfig.Asset.AssetPathType.StreamingAssets:
                        {
                            WWWReadBytesTask task = HttpManager.GetBytes(path, QConfig.Network.wwwReadFileTimeout);
                            yield return task.WaitForFinish();
                            if (task.bytes == null)
                            {
                                SetFail(string.Format("Can not load {0} bundle from {1}", pathType.ToString(), path), task.error);
                                yield break;
                            }
                            bytes = task.bytes;
                        }
                        break;

                    default:
                        {
                            SetFail(string.Format("Invalid path type {0}", pathType.GetType().Name));
                        }
                        yield break;
                }

                if (bytes == null) yield break;

                if (!string.IsNullOrEmpty(expectMD5) && pathType == QConfig.Asset.AssetPathType.AssetServer)
                {
                    md5 = Utility.MD5.Compute(bytes);
                    if (md5 != expectMD5)
                    {
                        SetFail(string.Format("{0} MD5 check failed, expected {1}, current {2}", path, expectMD5, md5));
                        System.IO.File.Delete(path);
                        yield break;
                    }
                }
                AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(bytes);
                yield return request;
                assetBundle = request.assetBundle;
                if (assetBundle == null)
                {
                    SetFail(string.Format("Create bundle from memory failed, path:{0}", path));
                    yield break;
                }
            }
        }

        public class DownloadAssetBundleTask : CoroutineTask
        {
            public AssetBundleInfo assetBundleInfo { get; private set; }
            public bool withDepend { get; private set; }

            public DownloadAssetBundleTask(AssetBundleInfo info, bool withDepend)
            {
                this.assetBundleInfo = info;
                this.withDepend = withDepend;
            }

            protected override IEnumerator OnProcess()
            {
                // Download dependencies
                List<Task> dependTaskList = null;
                if(withDepend)
                {
                    dependTaskList = new List<Task>();
                    var e = assetBundleInfo.dependBundleDict.GetEnumerator();
                    while (e.MoveNext())
                    {
                        var task = e.Current.Value.Download();
                        dependTaskList.Add(task);
                    }
                }

                // Download self
                if(assetBundleInfo.fromAssetServer && !assetBundleInfo.fileExists)
                {
                    var task = HttpManager.Download(assetBundleInfo.url, assetBundleInfo.cachePath, assetBundleInfo.hash, assetBundleInfo.size);
                    yield return task.WaitForFinish();
                    if (task.fail)
                    {
                        SetFail(string.Format("Download AssetBundle failed, url:{0} ", assetBundleInfo.url), task.error);
                        yield break;
                    }
                }

                // Check dependencies download finished
                if (withDepend)
                {
                    for(int i=0; i<dependTaskList.Count; ++i)
                    {
                        var task = dependTaskList[i];
                        yield return task.WaitForFinish();
                        if (task.success) continue;
                        SetFail("Depend AssetBundle download failed ", task.error);
                        yield break;
                    }
                }
            }
        }
            
    }
}

