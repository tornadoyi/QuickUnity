using UnityEngine;
using System.Collections;
using QuickUnity;

public class Initializer : MonoBehaviour {

    bool init = false;


    public void StartWithLocal()
    {
        StartCoroutine(Initialize(false));
    }

    public void StartWithDownload()
    {
        StartCoroutine(Initialize(true));
    }

    IEnumerator Initialize(bool useServerAssetTable)
    {
        if (init) yield break ;
        init = true;

        if (!QuickManager.isInit)
        {
            QuickManager.Start();
        }

        // Load setting
        Setting.Load();

        QConfig.Asset.useVersionAsFileName = false;

        // Start lua engine
        yield return LuaEngine.Start().WaitForFinish();

        // Download asset config

        if(useServerAssetTable)
        {
            var task = HttpManager.Download(
                FileManager.PathCombine(Setting.cdnUrl, Setting.assetTableFileName),
                FileManager.PathCombine(Setting.downloadCachePath, Setting.assetTableFileName));
            yield return task.WaitForFinish();
            useServerAssetTable = task.success;
            Debug.LogFormat("Download asset config {0}", task.fail ? "fail" : "success");
        }
        

        // Download asset table
        {
            var task = AssetManager.Start(
                Setting.streamingAssetsPath,
                Setting.cdnUrl,
                Setting.downloadCachePath,
                FileManager.PathCombine(Setting.streamingAssetsPath, Setting.assetTableFileName),
                !useServerAssetTable ? string.Empty : FileManager.PathCombine(Setting.downloadCachePath, Setting.assetTableFileName));
            yield return task.WaitForFinish();
        }

        // Load lua
        {
            var task = AssetManager.LoadAssetBundle(Setting.luaAssetBundleName);
            yield return task.WaitForFinish();
            if(task.success)
            {
                Debug.Log("Load lua success");
            }
            else
            {
                Debug.LogErrorFormat("Load lua fail, error: {0}", task.error);
                yield break;
            }
        }


        LuaLoaderHelper.PushLuaLoader(Setting.luaAssetBundleName);

        QConfig.Asset.loadAssetFromAssetBundle = false;
        LuaEngine.enableLuaComponent = true;
        LuaEngine.DoFile("Assets/_Assets/Lua/AppDelegate");

        LuaLoaderHelper.PopLuaLoader();

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Show");

        yield return null;
    }
}
