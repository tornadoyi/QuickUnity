using UnityEngine;
using System.Collections;
using System.Text;

namespace QuickUnity
{
    public class AssetInfo
    {
        public string name { get; private set; }
        public UnityEngine.Object asset { get; protected set; }
        public UnityEngine.Object[] subAssets { get; protected set; }

        public bool unused { get { return !keepTag && !loading; } }
        public bool loaded { get { return (asset != null || subAssets != null); } }
        public bool loading { get { return loadTask != null; } }
        protected LoadAssetTask loadTask;

        public bool keepTag = false;

        public AssetInfo(string name)
        {
            this.name = name;
        }

        public virtual bool Load() { throw new System.InvalidOperationException("Not Implement"); }
        public virtual Task LoadAsync() { throw new System.InvalidOperationException("Not Implement"); }


        public bool Unload()
        {
            if (!unused)
            {
                Debug.LogErrorFormat("Cannot unload {0}, loding ...", name);
                return false;
            }
            if (!loaded) return true;
            asset = null;
            subAssets = null;
            assetUnLoad.Invoke(this);
            return true;
        }

        protected void OnAssetLoad() { if (assetLoad != null) assetLoad(this); }
        protected void OnAssetUnLoad() { if (assetUnLoad != null) assetUnLoad(this); }


        // Static load and unload for monitor
        public static event QEventHandler1 assetLoad;
        public static event QEventHandler1 assetUnLoad;


        public class LoadAssetTask : CoroutineTask { }

    }
}

