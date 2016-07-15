using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SimpleJson;

namespace QuickUnity
{
    public class AssetTable
    {
        public static AssetTable Merge(AssetTable t1, AssetTable t2)
        {
            AssetTable t = new AssetTable();

            {// Copy t1 to t
                var e = t1.bundleDict.GetEnumerator();
                while (e.MoveNext())
                {
                    t.bundleDict.Add(e.Current.Key, e.Current.Value);
                }
            }

            {// Copy t2 to t
                var e = t2.bundleDict.GetEnumerator();
                while (e.MoveNext())
                {
                    AssetBundleInfo info = null;
                    if (t.bundleDict.TryGetValue(e.Current.Key, out info))
                    {
                        if (info.version > e.Current.Value.version) continue;
                        t.bundleDict.Remove(e.Current.Key);

                    }
                    t.bundleDict.Add(e.Current.Key, e.Current.Value);
                }
            }
            return t;
        }

        public void AddBundleInfos(params AssetBundleInfo[] bundles)
        {
            if(bundles == null || bundles.Length == 0)
            {
                Debug.LogError("Invalid bundles");
                return;
            }

            for(int i=0; i<bundles.Length; ++i)
            {
                AssetBundleInfo bundle = bundles[i];
                if(bundleDict.ContainsKey(bundle.id))
                {
                    Debug.LogError(string.Format("Repeated bundle {0}", bundle.name));
                    continue;
                }
                bundleDict.Add(bundle.id, bundle);
            }
        }


        public void Analyse()
        {
            // Clear
            assetDict.Clear();

            // Analyse bundle 
            var e_bundle = bundleDict.GetEnumerator();
            while (e_bundle.MoveNext())
            {
                var bundleInfo = e_bundle.Current.Value;

                // Collect assets
                var e_asset = bundleInfo.assetDict.GetEnumerator();
                while (e_asset.MoveNext())
                {
                    assetDict[e_asset.Current.Key] = e_asset.Current.Value;
                }

                // Analyse dependencies
                for(int i=0; i<bundleInfo.dependList.Count; ++i)
                {
                    var dependName = bundleInfo.dependList[i];
                    var dependBundle = GetBundleInfo(dependName);
                    if(dependBundle == null)
                    {
                        Debug.LogError(string.Format("Can not find depend bundle {0} from depend list of {1}", dependName, bundleInfo.id));
                        continue;
                    }
                    bundleInfo.AddDependency(dependBundle);
                }
            }
        }

       
        public AssetBundleInfo GetBundleInfo(string name)
        { 
            AssetBundleInfo info = null;
            bundleDict.TryGetValue(name, out info);
            return info;
        }

        public AssetInfo GetAssetInfo(string name)
        {
            AssetInfo info = null;
            assetDict.TryGetValue(name, out info);
            return info;
        }

        public Task LoadAsync(string filePath, QConfig.Asset.AssetPathType assetPathType)
        {
            Task task = new LoadAssetTableTask(this, filePath, assetPathType);
            task.Start();
            return task;
        }


        public static AssetTable CreateFromJson(string json, QConfig.Asset.AssetPathType assetPathType)
        {
            // Check
            var table = new AssetTable();
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Invalid json string");
                return table;
            }

            // Parse
            table.FromJson(json, assetPathType);
            return table;
        }

        public bool FromJson(string json, QConfig.Asset.AssetPathType assetPathType)
        {
            // Check
            if(string.IsNullOrEmpty(json))
            {
                Debug.LogError("Invalid json string");
                return false;
            }

            // Clear previous data
            bundleDict.Clear();
            assetDict.Clear();

            // Parse
            var jtable = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(json);
            var jbundles = jtable.GetArray("bundles");
            var bundle_enum = jbundles.GetEnumerator();
            while (bundle_enum.MoveNext())
            {
                // Bundle
                var jbundle = bundle_enum.Current as JsonObject;

                // Depends
                var dependList = new List<string>();
                var jdpends = jbundle.GetArray("depends");
                for (int i = 0; i < jdpends.Count; ++i)
                {
                    dependList.Add(jdpends[i] as string);
                }

                // Properties
                AssetBundleInfo bundle = new AssetBundleInfo(
                    jbundle.GetString("name"),
                    jbundle.GetInt("size"),
                    jbundle.GetString("variant"),
                    jbundle.GetString("relative_path"),
                    jbundle.GetInt("version"),
                    jbundle.GetString("md5"),
                    dependList,
                    assetPathType);

                // Assets
                var jassets = jbundle.GetArray("assets");
                for (int i = 0; i < jassets.Count; ++i)
                {
                    AssetInfo asset = new AssetInfo(jassets[i] as string);
                    bundle.AddAssetInfo(asset);
                }
                AddBundleInfos(bundle);
            }
            return true;
        }

        /// <summary>
        /// Generate by json parse
        /// </summary>
        public Dictionary<string, AssetBundleInfo> bundleDict = new Dictionary<string, AssetBundleInfo>();

        /// <summary>
        /// Generate by analyze
        /// </summary>
        public Dictionary<string, AssetInfo> assetDict = new Dictionary<string, AssetInfo>();



        #region Task ==================================================================================

        protected class LoadAssetTableTask : CoroutineTask
        {
            public LoadAssetTableTask(AssetTable table, string filePath, QConfig.Asset.AssetPathType assetPathType)
            {
                this.table = table;
                this.filePath = filePath;
                this.assetPathType = assetPathType;
            }

            protected override IEnumerator OnProcess()
            {
                switch (assetPathType)
                {
                    case QConfig.Asset.AssetPathType.Builtin:
                        {
                            WWWReadTextTask task = new WWWReadTextTask(filePath);
                            yield return task.StartAndWaitForDone();
                            if (string.IsNullOrEmpty(task.text)) yield break;
                            table.FromJson(task.text, assetPathType);

                        } break;
                    case QConfig.Asset.AssetPathType.External:
                        {
                            FileReadTextTask task = new FileReadTextTask(filePath);
                            yield return task.StartAndWaitForDone();
                            if (string.IsNullOrEmpty(task.text)) yield break;
                            table.FromJson(task.text, assetPathType);

                        } break;
                    default:
                        {
                            Debug.Log(string.Format("Invalid path type {0}", assetPathType.GetType().Name));
                            yield break;
                        }
                }
            }

            public AssetTable table { get; private set; }
            protected string filePath;
            protected QConfig.Asset.AssetPathType assetPathType;
        }

        #endregion
    }


}

