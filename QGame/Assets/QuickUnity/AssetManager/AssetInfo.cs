using UnityEngine;
using System.Collections;
using System.Text;

namespace QuickUnity
{
    public class AssetInfo
    {
        public AssetInfo(string name)
        {
            _name = name;
        }

        public bool Load()
        {
            if (asset != null) return true;
            if (!bundleInfo.loaded)
            {
                Debug.LogError(string.Format("{0} cannot load, need to load asset bundle first", name));
                return false;
            }

            subAssets = bundleInfo.assetBundle.LoadAssetWithSubAssets(name);
            asset = subAssets.Length > 0 ? subAssets[0] : asset;
            
            
            if (!loaded)
            {
                Debug.Log(string.Format("{0} load failed", name));
                return false;
            }
            assetLoad.Invoke(this);
            return true;
        }

        public Task LoadAsync()
        {
            if (loaded)
            {
                var t = new LoadAssetTask(this);
                t.Start();
                return t;
            }
            if (loading) return loadTask;
            
            // Increase load count
            ++bundleInfo.assetLoadingCount;

            // Create task
            loadTask = new LoadAssetTask(this);
            loadTask.Finish((result) =>
            {
                // Decrease load count
                --bundleInfo.assetLoadingCount;
                var task = loadTask;
                loadTask = null;
                if (task.asset != null || task.subAssets != null)
                {
                    subAssets = task.subAssets;
                    asset = task.asset;
                    assetLoad.Invoke(this);
                }

            });
            loadTask.Start();

            return loadTask;
        }


        public bool Unload()
        {
            if (!unused)
            {
                Debug.LogError(string.Format("Cannot unload {0}, loding ...", name));
                return false;
            }
            if (!loaded) return true;
            asset = null;
            subAssets = null;
            assetUnLoad.Invoke(this);
            return true;
        }

        // Base property
        public string name { get { return _name; } }
        protected string _name;

        // Bundle and loaded asset 
        public AssetBundleInfo bundleInfo;
        public UnityEngine.Object asset;
        public UnityEngine.Object[] subAssets;


        // Load state for asset
        public bool unused { get { return !keepTag && !loading; } }
        public bool loaded { get { return (asset != null || subAssets != null); } }
        public bool loading { get { return loadTask != null; } }
        protected LoadAssetTask loadTask;

        // Keep tag
        public bool keepTag = false;
        
        // Static load and unload for monitor
        public static event QEventHandler1 assetLoad;
        public static event QEventHandler1 assetUnLoad;


        #region Task

        public class LoadAssetTask : CoroutineTask
        {
            public LoadAssetTask(AssetInfo assetInfo)
            {
                _assetInfo = assetInfo;
            }

            protected override IEnumerator OnProcess()
            {
                // Check has loaded
                if(_assetInfo.loaded)
                {
                    _asset = _assetInfo.asset;
                    _subAssets = _assetInfo.subAssets;
                    yield break;
                }

                // Load bundle first
                AssetBundleInfo bundleInfo = _assetInfo.bundleInfo;
                if (!bundleInfo.loaded)
                {
                    Task task = bundleInfo.LoadAsync();
                    yield return task.WaitForFinish();
                    if (bundleInfo.assetBundle == null)
                    {
                        SetFail(string.Format("Load bundle failed, {0}", task.error));
                        yield break;
                    }
                }

                // Load asset
                AssetBundleRequest request = bundleInfo.assetBundle.LoadAssetAsync(assetInfo.name);
                yield return request;

                _subAssets = request.allAssets;
                _asset = request.asset;

                if (_asset == null && _subAssets == null)
                {
                    SetFail(string.Format("Load sub asset failed, asset name:{0}", assetInfo.name));
                    yield break;
                }
            }

            public UnityEngine.Object asset { get { return _asset; } }
            protected UnityEngine.Object _asset;

            public UnityEngine.Object[] subAssets { get { return _subAssets; } }
            protected UnityEngine.Object[] _subAssets;

            public AssetInfo assetInfo { get{ return _assetInfo; } }
            protected AssetInfo _assetInfo;
        }

        #endregion
    }
}

