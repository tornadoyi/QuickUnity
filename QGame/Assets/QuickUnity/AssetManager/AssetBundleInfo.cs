using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public class AssetBundleInfo
    {
        public AssetBundleInfo(
            string name, 
            int size, 
            string variant, 
            string relativePath, 
            long version, 
            string md5, 
            List<string> dependList,
            QConfig.Asset.AssetPathType pathType)
        {
            _name = name;
            _size = size;
            _variant = variant;
            _relativePath = relativePath;
            _version = version;
            _md5 = md5;
            _dependList = dependList;
            _pathType = pathType;
        }

        public bool AddAssetInfo(BundleAssetInfo asset)
        {
            if (asset == null)
            {
                Debug.LogError("Invalid asset");
                return false;
            }

            if (asset.bundleInfo != null)
            {
                Debug.LogError(string.Format("Asset {0} has save in bundle {1}", asset.name, asset.bundleInfo.name));
                return false;
            }

            if (_assetDict.ContainsKey(asset.name))
            {
                Debug.LogError(string.Format("Repeated asset {0} in bundle", asset.name, name));
                return false;
            }
            _assetDict.Add(asset.name, asset);

            return true;
        }

        public bool AddDependency(AssetBundleInfo bundle)
        {
            if (bundle == null)
            {
                Debug.LogError("Invalid bundle");
                return false;
            }

            if (_dependBundleDict.ContainsKey(bundle.name)) return true;

            _dependBundleDict.Add(bundle.name, bundle);

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
            loadTask.Finish((result) =>
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


        public void Retain() { ++_reference; }
        public void Release()
        {
            if (_reference == 0)
            {
                Debug.LogError("Invalid release, current reference is 0");
                return;
            }
            --_reference;
        }


        #region Base properties ==================================================================================

        public string name { get { return _name; } }
        protected string _name;

        public int size { get { return _size; } }
        protected int _size;

        public string relativePath { get { return _relativePath; } }
        protected string _relativePath = string.Empty;

        public string variant { get { return _variant; } }
        protected string _variant = string.Empty;

        public long version { get { return _version; } }
        protected long _version;
        public string md5 { get { return _md5; } }
        protected string _md5 = string.Empty;

        public QConfig.Asset.AssetPathType pathType { get { return _pathType; } }
        public QConfig.Asset.AssetPathType _pathType;

        public bool isBuiltin { get { return pathType == QConfig.Asset.AssetPathType.Builtin; } }
  
        public bool isExternal { get { return pathType == QConfig.Asset.AssetPathType.External; } }

        public List<string> dependList { get { return _dependList; } }
        protected List<string> _dependList = new List<string>();

        public string id { get { return FileManager.PathCombine(relativePath, name); } }

        public string fileCachePath
        {
            get
            {
                if (_pathType == QConfig.Asset.AssetPathType.Builtin)
                {
                    return string.Format(
                        "{0}.{1}.{2}",
                        FileManager.PathCombine(AssetManager.builtinAssetPath, relativePath, name),
                        version,
                        QConfig.Asset.assetBundleSuffix);
                }
                else
                {
                    return string.Format(
                        "{0}.{1}.{2}",
                        FileManager.PathCombine(AssetManager.externalAssetPath, relativePath, name),
                        version,
                        QConfig.Asset.assetBundleSuffix);
                }
            }
        }

        public string fileDownloadUrl
        {
            get
            {
                if (_pathType == QConfig.Asset.AssetPathType.Builtin) return string.Empty;
                return string.Format(
                    "{0}.{1}.{2}",
                    FileManager.PathCombine(AssetManager.downloadUrl, relativePath, name),
                    version,
                    QConfig.Asset.assetBundleSuffix);
            }
        }

        public Dictionary<string, AssetInfo> assetDict { get { return _assetDict; } }
        protected Dictionary<string, AssetInfo> _assetDict = new Dictionary<string, AssetInfo>();

        public Dictionary<string, AssetBundleInfo> dependBundleDict { get { return _dependBundleDict; } }
        protected Dictionary<string, AssetBundleInfo> _dependBundleDict = new Dictionary<string, AssetBundleInfo>();

        #endregion



        #region Runtime properties ==================================================================================

        public AssetBundle assetBundle;

        public bool unused { get { return (!keepTag && reference == 0 && assetLoadingCount == 0 && !loading); } }
        public bool loading { get { return loadTask != null; } }
        public bool loaded { get { return assetBundle != null; } }
        public bool downloading { get { return downloadTask != null; } }
        
        // Keep tag
        public bool keepTag = false;

        protected LoadOrDownloadAssetBundleTask loadTask;
        protected DownloadAssetBundleTask downloadTask;

        public int reference { get { return _reference; } }
        protected int _reference = 0;
        public int assetLoadingCount = 0;

        public bool fileExists
        {
            get
            {
                if (_pathType == QConfig.Asset.AssetPathType.Builtin) return true;
                return System.IO.File.Exists(fileCachePath);
            }
        }

        #endregion


        /// <summary>
        /// Event for bundle load and unload
        /// </summary>
        public static event QEventHandler1 bundleLoad;
        public static event QEventHandler1 bundleUnLoad;


        #region Task ==================================================================================

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
                    var task = new LoadAssetBundleTask(assetBundleInfo.fileCachePath, assetBundleInfo.pathType, assetBundleInfo.md5);
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
            public AssetBundle assetBundle { get { return _assetBundle; } }
            protected AssetBundle _assetBundle;

            public string path { get { return _path; } }
            protected string _path;

            public string md5 { get { return _md5; } }
            protected string _md5 = string.Empty;

            public QConfig.Asset.AssetPathType pathType { get { return _pathType; } }
            protected QConfig.Asset.AssetPathType _pathType;

            public string expectMD5 { get { return _expectMD5; } }
            protected string _expectMD5 = string.Empty;

            public LoadAssetBundleTask(string path, QConfig.Asset.AssetPathType pathType)
            {
                _path = path;
                _pathType = pathType;
            }

            public LoadAssetBundleTask(string path, QConfig.Asset.AssetPathType pathType, string expectMD5)
            {
                _path = path;
                _pathType = pathType;
                _expectMD5 = expectMD5;
            }

            protected override IEnumerator OnProcess()
            {
                // Load self
                byte[] bytes = null;
                switch (pathType)
                {
                    case QConfig.Asset.AssetPathType.External:
                        {
                            FileReadBytesTask task = new FileReadBytesTask(path);
                            yield return task.Start().WaitForFinish();
                            if (task.bytes == null)
                            {
                                SetFail(string.Format("Can not load bundle from path {0}", _path), task.error);
                                yield break;
                            }
                            bytes = task.bytes;

                        }
                        break;

                    case QConfig.Asset.AssetPathType.Builtin:
                        {
                            WWWReadBytesTask task = HttpManager.GetBytes(path, QConfig.Network.wwwReadFileTimeout);
                            yield return task.WaitForFinish();
                            if (task.bytes == null)
                            {
                                SetFail(string.Format("Can not load bundle from path {0}", _path), task.error);
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

                if (!string.IsNullOrEmpty(_expectMD5) && pathType == QConfig.Asset.AssetPathType.External)
                {
                    _md5 = Utility.MD5.Compute(bytes);
                    if (_md5 != _expectMD5)
                    {
                        SetFail(string.Format("{0} MD5 check failed, expected {1}, current {2}", _path, _expectMD5, _md5));
                        System.IO.File.Delete(path);
                        yield break;
                    }
                }
                AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(bytes);
                yield return request;
                _assetBundle = request.assetBundle;
                if (_assetBundle == null)
                {
                    SetFail(string.Format("Create bundle from memory failed, path:{0}", _path));
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
                if(assetBundleInfo.isExternal && !assetBundleInfo.fileExists)
                {
                    var task = HttpManager.Download(assetBundleInfo.fileDownloadUrl, assetBundleInfo.fileCachePath, assetBundleInfo.md5, assetBundleInfo.size);
                    yield return task.WaitForFinish();
                    if (task.fail)
                    {
                        SetFail(string.Format("Download AssetBundle failed, url:{0} ", assetBundleInfo.fileDownloadUrl), task.error);
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

        #endregion
    }
}

