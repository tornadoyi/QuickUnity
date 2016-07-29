using UnityEngine;
using System.Collections;
using QuickUnity;

public class Initializer : MonoBehaviour {

    [HideInInspector]
    public bool initFinished { get; private set; }

    IEnumerator Start()
    {
        if (!QuickManager.isInit)
        {
            QuickManager.Start();
        }

        // Load setting
        Setting.Load();
        QConfig.Asset.loadAssetFromAssetBundle = Setting.loadAssetFromAssetBundle;
        QConfig.Asset.useVersionAsFileName = true;

        // Start language
        Language.Start();

        // Start asset manager
        AssetManager.Start(
                Setting.streamingAssetsPath,
                Setting.cdnUrl,
                Setting.downloadCachePath,
                FileManager.PathCombine(Setting.streamingAssetsPath, Setting.assetTableFileName),
                FileManager.PathCombine(Setting.cdnUrl, Setting.assetTableFileName),
                FileManager.PathCombine(Setting.downloadCachePath, Setting.assetTableFileName));

        yield return AssetManager.LoadLocalAssetTable().WaitForFinish();

        // Init UI
        {
            var task = AssetManager.LoadGameObjectAsync("Assets/_Assets/Pages/StartPage.prefab");
            yield return task.WaitForFinish();
            if(task.fail) Debug.LogError(task.error);
            var page = GameObject.Instantiate(task.gameObject);
            page.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }

        StartCoroutine(Initialize());
    }

    

    IEnumerator Initialize()
    {
        // Start lua engine
        yield return LuaEngine.Start().WaitForFinish();

        // Download asset config
        do
        {
            var task = AssetManager.LoadServerAssetTable(true);
            yield return task.WaitForFinish();
            if (task.success)
            {
                Debug.LogFormat("Load server asset table success");
                break;
            }
            else
            {
                Debug.LogFormat("Load server asset table fail, ready to retry");
            }
        }
        while (true);



        // Load lua
        {
            var task = AssetManager.LoadAssetBundle(Setting.luaAssetBundleName);
            yield return task.WaitForFinish();
            if (task.success)
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

        initFinished = true;

    }

}
