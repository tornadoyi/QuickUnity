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

        yield return LuaEngine.StartAsync().WaitForDone();

        QConfig.Asset.loadAssetFromAssetBundle = false;
        LuaEngine.enableLuaComponent = true;
        LuaEngine.DoFile("Assets/_Assets/Lua/AppDelegate");

        yield return null;
    }
}
