using UnityEngine;
using System.Collections;


namespace QuickUnity
{
    public class BundleAssetInfo : AssetInfo
    {
        public AssetBundleInfo bundleInfo { get; private set; }

        public BundleAssetInfo(string name, AssetBundleInfo bundleInfo) : base(name)
        {
            this.bundleInfo = bundleInfo;
        }

        public override bool Load()
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

            OnAssetLoad();
            return true;
        }

        public override Task LoadAsync()
        {
            if (loaded)
            {
                var t = new LoadBundleAssetTask(this);
                t.Start();
                return t;
            }
            if (loading) return loadTask;

            // Increase load count
            ++bundleInfo.assetLoadingCount;

            // Create task
            loadTask = new LoadBundleAssetTask(this);
            loadTask.Start().Finish((_) =>
            {
                // Decrease load count
                --bundleInfo.assetLoadingCount;
                var task = loadTask as LoadBundleAssetTask;
                loadTask = null;
                if (task.asset != null || task.subAssets != null)
                {
                    subAssets = task.subAssets;
                    asset = task.asset;
                    OnAssetLoad();
                }

            });

            return loadTask;
        }




        public class LoadBundleAssetTask : LoadAssetTask
        {
            public UnityEngine.Object asset { get; private set; }
            public UnityEngine.Object[] subAssets { get; private set; }
            public BundleAssetInfo assetInfo { get; private set; }

            public LoadBundleAssetTask(BundleAssetInfo assetInfo)
            {
                this.assetInfo = assetInfo;
            }

            protected override IEnumerator OnProcess()
            {
                // Check has loaded
                if (assetInfo.loaded)
                {
                    asset = assetInfo.asset;
                    subAssets = assetInfo.subAssets;
                    yield break;
                }

                // Load bundle first
                AssetBundleInfo bundleInfo = assetInfo.bundleInfo;
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

                subAssets = request.allAssets;
                asset = request.asset;

                if (asset == null && subAssets == null)
                {
                    SetFail(string.Format("Load sub asset failed, asset name:{0}", assetInfo.name));
                    yield break;
                }
            }
        }


    }
}
