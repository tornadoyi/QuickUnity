using UnityEngine;
using System.Collections;


namespace QuickUnity
{
    public class LocalAssetInfo : AssetInfo
    {
        public LocalAssetInfo(string name) : base(name) { }

        public override bool Load()
        {
            if (asset != null) return true;

#if UNITY_EDITOR
            if (!QConfig.Asset.loadAssetFromAssetBundle)
            {
                subAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(name);
                asset = UnityEditor.AssetDatabase.LoadAssetAtPath(name, typeof(UnityEngine.Object));
                return asset == null;
            }
#endif
            asset = Resources.Load(name);
            subAssets = Resources.LoadAll(name);
            return asset == null;
        }

        public override Task LoadAsync()
        {
            if (loaded)
            {
                var t = new LoadLocalAssetTask(this);
                t.Start();
                return t;
            }
            if (loading) return loadTask;


            // Create task
            loadTask = new LoadLocalAssetTask(this);
            loadTask.Start().Finish((_) =>
            {
                // Decrease load count
                var task = loadTask as LoadLocalAssetTask;
                loadTask = null;
                if (task.asset != null || (task.subAssets != null && task.subAssets.Length > 0))
                {
                    subAssets = task.subAssets;
                    asset = task.asset;
                    OnAssetLoad();
                }

            });

            return loadTask;
        }


        public class LoadLocalAssetTask : LoadAssetTask
        {
            public LocalAssetInfo assetInfo { get; private set; }
            public UnityEngine.Object asset { get; private set; }
            public UnityEngine.Object[] subAssets { get; private set; }

            public LoadLocalAssetTask(LocalAssetInfo assetInfo)
            {
                this.assetInfo = assetInfo;
            }

            protected override IEnumerator OnProcess()
            {
                if(string.IsNullOrEmpty(assetInfo.name))
                {
                    SetFail("Load asset fail, asset name is null");
                    yield break;
                }

                if(assetInfo.loaded)
                {
                    asset = assetInfo.asset;
                    subAssets = assetInfo.subAssets;
                    yield break;
                }

#if UNITY_EDITOR
                if (!QConfig.Asset.loadAssetFromAssetBundle)
                {
                    subAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(assetInfo.name);
                    asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetInfo.name, typeof(UnityEngine.Object));
                    if(asset == null)
                    {
                        SetFail(string.Format("[EDITOR] Can not load asset {0}", assetInfo.name));
                    }
                    yield break;
                }
#endif
                var request = Resources.LoadAsync(assetInfo.name);
                yield return request;
                asset = request.asset;

                if(asset == null)
                {
                    SetFail(string.Format("Can not load asset {0} from Resources", assetInfo.name));
                }
            }
        }
    }

    
}
