using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QuickUnity
{
    public partial class AssetManager : BaseManager<AssetManager>
    {
        public class DownloadAssetBundleTask : CoroutineTask
        {
            public string name { get; private set; }

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
            public UnityEngine.Object asset { get; private set; }
            public UnityEngine.Object[] subAssets { get; private set; }
            public string name { get; private set; }

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
            public string name { get; private set; }
            public UnityEngine.Object asset { get; private set; }
            public UnityEngine.Object[] subAssets { get; private set; }

            public LoadAssetTask(string name)
            {
                this.name = string.IsNullOrEmpty(name) ? string.Empty : name;
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


        public class LoadLocalAssetTableTask : CoroutineTask
        {
            public string assetTablePath { get; private set; }

            public LoadLocalAssetTableTask(string assetTablePath)
            {
                this.assetTablePath = assetTablePath;
            }

            protected override IEnumerator OnProcess()
            {
                var asseTtable = new AssetTable();
                var task = asseTtable.LoadAsync(assetTablePath, QConfig.Asset.AssetPathType.StreamingAssets);
                yield return task.WaitForFinish();
                if (task.fail)
                {
                    SetFail(task.error);
                }
                else
                {
                    AssetManager.instance.assetTable = AssetTable.Merge(AssetManager.instance.assetTable, asseTtable);
                    AssetManager.instance.assetTable.Analyse();
                    SetSuccess();
                }
                SetFinish();
            }
        }


        public class LoadServerAssetTableTask : CoroutineTask
        {
            public string assetTableUrl { get; private set; }
            public string assetTableCachePath { get; private set; }
            public bool download { get; private set; }

            public LoadServerAssetTableTask(string assetTableUrl, string assetTableCachePath, bool download)
            {
                this.assetTableUrl = assetTableUrl;
                this.assetTableCachePath = assetTableCachePath;
                this.download = download;
            }

            protected override IEnumerator OnProcess()
            {
                if (download)
                {
                    var downloadTask = HttpManager.Download(assetTableUrl, assetTableCachePath);
                    yield return downloadTask.WaitForFinish();
                    if (downloadTask.fail)
                    {
                        SetFail(downloadTask.error);
                        SetFinish();
                        yield break;
                    }
                }

                var asseTtable = new AssetTable();
                var task = asseTtable.LoadAsync(assetTableCachePath, QConfig.Asset.AssetPathType.AssetServer);
                yield return task.WaitForFinish();
                if (task.fail)
                {
                    SetFail(task.error);
                }
                else
                {
                    AssetManager.instance.assetTable = AssetTable.Merge(AssetManager.instance.assetTable, asseTtable);
                    AssetManager.instance.assetTable.Analyse();
                    SetSuccess();
                }
                SetFinish();
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
}


