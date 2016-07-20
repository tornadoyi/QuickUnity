using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SimpleJson;
using YamlDotNet.RepresentationModel;
using System;

namespace QuickUnity
{
    public class AssetTable
    {
        /// <summary>
        /// Generate by config parse
        /// </summary>
        public Dictionary<string, AssetBundleInfo> bundleDict { get; private set; }

        /// <summary>
        /// Generate by analyze
        /// </summary>
        public Dictionary<string, AssetInfo> assetDict { get; private set; }

        public AssetTable()
        {
            bundleDict = new Dictionary<string, AssetBundleInfo>();
            assetDict = new Dictionary<string, AssetInfo>();
        }

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

        public void AddAssetBundleInfo(AssetBundleInfo info)
        {
            if (info == null)
            {
                Debug.LogError("Invalid asset bundle info");
                return;
            }

            if(bundleDict.ContainsKey(info.name))
            {
                Debug.LogError(string.Format("Repeated bundle {0}", info.name));
                return;
            }
            bundleDict.Add(info.name, info);
        }

        public void AddAssetInfo(AssetInfo info)
        {
            if (info == null)
            {
                Debug.LogError("Invalid asset info");
                return;
            }

            if(bundleDict.ContainsKey(info.name))
            {
                Debug.LogError(string.Format("Repeated asset {0}", info.name));
                return;
            }
            assetDict.Add(info.name, info);
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
                        Debug.LogError(string.Format("Can not find depend bundle {0} from depend list of {1}", dependName, bundleInfo.name));
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

        public bool FromYaml(string yml, QConfig.Asset.AssetPathType assetPathType)
        {
            // Check
            if(string.IsNullOrEmpty(yml))
            {
                Debug.LogError("Invalid json string");
                return false;
            }

            var input = new StringReader(yml);
            var yaml = new YamlStream();
            yaml.Load(input);
            var root = (YamlMappingNode)yaml.Documents[0].RootNode;
            var assetBundlesNode = (YamlSequenceNode)root.Children[new YamlScalarNode("asset_bundles")];
            for (int i=0; i<assetBundlesNode.Children.Count; ++i)
            {
                var node = (YamlMappingNode)assetBundlesNode.Children[i];

                // Depends
                var depends = new List<string>();
                var dependNode = (YamlSequenceNode)node.Children[new YamlScalarNode("dependencies")];
                for (int j = 0; j < dependNode.Children.Count; ++j)
                {
                    depends.Add(dependNode.Children[j].ToString());

                }

                // Assets
                var assets = new List<string>();
                var assetNode = (YamlSequenceNode)node.Children[new YamlScalarNode("assets")];
                for (int j = 0; j < assetNode.Children.Count; ++j)
                {
                    assets.Add(assetNode.Children[j].ToString());
                }

                // Bundle
                AssetBundleInfo bundle = new AssetBundleInfo(
                    node.Children[new YamlScalarNode("name")].ToString(), 
                    Convert.ToInt64(node.Children[new YamlScalarNode("size")].ToString()), 
                    node.Children[new YamlScalarNode("variant")].ToString(), 
                    Convert.ToInt64(node.Children[new YamlScalarNode("version")].ToString()),
                    node.Children[new YamlScalarNode("md5")].ToString(),
                    depends,
                    assets,
                    assetPathType);

                // Save
                AddAssetBundleInfo(bundle);
                    
            }

            return true;
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

                // Assets
                var assets = new List<string>();
                var jassets = jbundle.GetArray("assets");
                for (int i = 0; i < jassets.Count; ++i)
                {
                    assets.Add(jassets[i] as string);
                }

                // Properties
                AssetBundleInfo bundle = new AssetBundleInfo(
                    jbundle.GetString("name"),
                    jbundle.GetLong("size"),
                    jbundle.GetString("variant"),
                    jbundle.GetInt("version"),
                    jbundle.GetString("md5"),
                    dependList,
                    assets,
                    assetPathType);

                // Save
                AddAssetBundleInfo(bundle);
            }
            return true;
        }

        



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
                    case QConfig.Asset.AssetPathType.StreamingAssets:
                        {
                            WWWReadTextTask task = new WWWReadTextTask(filePath);
                            yield return task.Start().WaitForFinish();
                            if (string.IsNullOrEmpty(task.text)) yield break;
                            table.FromYaml(task.text, assetPathType);

                        } break;
                    case QConfig.Asset.AssetPathType.AssetServer:
                        {
                            FileReadTextTask task = new FileReadTextTask(filePath);
                            yield return task.Start().WaitForFinish();
                            if (string.IsNullOrEmpty(task.text)) yield break;
                            table.FromYaml(task.text, assetPathType);

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

    }


}

