using UnityEngine;
using System.Collections;
using QuickUnity;

public class LuaLoaderHelper
{
    public static bool PushLuaLoader(string assetBundleName)
    {
        if (string.IsNullOrEmpty(assetBundleName)) { Debug.LogError("Invalid AssetBundle name"); return false; }

        AssetBundle bundle = null;
        if (Setting.loadLuaFromAssetBundle)
        {
            bundle = AssetManager.GetAssetBundle(assetBundleName);
            if (bundle == null)
            {
                Debug.LogErrorFormat("Can not find AssetBundle {0}", assetBundleName);
                return false;
            }
        }

        LuaEngine.PushLuaLoader((fn) =>
        {
            fn = fn.Replace(".", "/");
            fn += Setting.luaFileExtension;

            byte[] bytes = null;
            if (Setting.loadLuaFromAssetBundle)
            {
                var protoType = bundle.LoadAsset(fn);
                if (protoType != null)
                {
                    var text = protoType as TextAsset;
                    bytes = text.bytes;
                }
            }
            else
            {
                if (!System.IO.File.Exists(fn)) return null;
                bytes = FileManager.LoadBinaryFile(fn);
            }
            return bytes;
        });

        return true;
    }

    public static void PopLuaLoader()
    {
        LuaEngine.PopLuaLoader();
    }
}
