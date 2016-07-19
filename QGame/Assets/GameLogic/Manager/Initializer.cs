using UnityEngine;
using System.Collections;
using QuickUnity;

public class Initializer : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Initialize());
	}
	
	IEnumerator Initialize()
    {
        if(!QuickManager.isInit)
        {
            QuickManager.Start();
        }

        // Load setting
        Setting.Load();

        // Start lua engine
        yield return LuaEngine.Start().WaitForFinish();

        // Download asset config
        {
            var task = HttpManager.Download(
                FileManager.PathCombine(Setting.cdnUrl, Setting.assetTableFileName),
                FileManager.PathCombine(Setting.downloadCachePath, Setting.assetTableFileName));
            yield return task.WaitForFinish();
            if(task.fail)
            {
                Debug.LogError("Download asset config fail");
            }
        }

        // Download lua
        {
            var task = AssetManager.Start(
                Setting.streamingAssetsPath,
                Setting.downloadCachePath,
                Setting.cdnUrl,
                string.Empty,
                Setting.serverTableFilePath);
            yield return task.WaitForFinish();
        }

        LuaLoaderHelper.PushLuaLoader("ss");

        QConfig.Asset.loadAssetFromAssetBundle = false;
        LuaEngine.enableLuaComponent = true;
        LuaEngine.DoFile("Assets/_Assets/Lua/AppDelegate");

        LuaLoaderHelper.PopLuaLoader();

        yield return null;
    }
}
