﻿using UnityEngine;
using System.Collections;
using QuickUnity;
using System;
using System.Collections.Generic;
using SLua;

public class Setting : Singleton<Setting>
{
    public enum AssetBundleLevel
    {
        None = 0,
        Asset = 1,
        Lua = 2,
        All = Asset | Lua,
    }

    
    /// <summary>
    /// Basic config
    /// </summary>
#if UNITY_IPHONE
    public const string platformName = "iOS";
#else
    public const string platformName = "Android";
#endif


    /// <summary>
    /// All static config
    /// </summary>
    public static readonly string streamingAssetsAssetPath = Application.streamingAssetsPath;

    public static readonly string downloadCachePath = FileManager.PathCombine(Application.persistentDataPath, platformName);

    public static readonly string settingFilePath = "Setting/Setting";

    public static readonly string localVersionFilePath = FileManager.PathCombine(downloadCachePath, "spiral.version");

    public static string latestVersionFileUrl { get { return FileManager.PathCombine(loginServerUrl, platformName, "spiral.version"); } }

    public static string assetTableFileName = "asset_table.yml";

    public static string languageFilePath = "Setting/Language";

    public static readonly string streamingAssetsTableFilePath = FileManager.PathCombine(streamingAssetsAssetPath, assetTableFileName);

    public static readonly string serverTableFilePath = FileManager.PathCombine(downloadCachePath, assetTableFileName);

    public static readonly string luaAssetBundleName = "Lua";

    public const string luaFileExtension = ".lua";


    /// <summary>
    /// Symbol settings
    /// </summary>
    public static readonly string assetSymbolLibraryName = "Resource";

    public static readonly string languageSymbolLibraryName = "Language";

    public static readonly string languageSymbolFileName = "Language.txt";

    public static readonly string assetSymbolFileName = "Resource.txt";

    public static readonly string builtinSymbolPath = "Setting";

    public static readonly string externalSymbolPath = "Assets/DynamicAssets/Data";


    /// <summary>
    /// Load from setting file below
    /// </summary>
    public static string loginServerUrl { get; private set;}
    public static string cdnUrl { get; private set;}
    public static string appVersion { get; private set;}
    public static AssetBundleLevel assetBundleLevel { get; private set;}
    public static SystemLanguage defaultLanguage { get; private set;}
    public static List<SystemLanguage> supportLanguageList { get; private set;}


    public static bool loadLuaFromAssetBundle
    {
        get
        {
#if UNITY_EDITOR
            return assetBundleLevel == AssetBundleLevel.All;
#else
            return true;
#endif
        }
    }


    [DoNotToLua]
    public static bool Load()
    {
        var textAsset = (TextAsset)Resources.Load(settingFilePath);
        if (textAsset == null)
        {
            Debug.LogErrorFormat("Load setting file {0} failed", settingFilePath);
            return false;
        }
        var content = textAsset.text;
        Resources.UnloadAsset(textAsset);

        var yaml = new Yaml(content);
        loginServerUrl = yaml.GetString("login_server_url");
        cdnUrl = FileManager.PathCombine(yaml.GetString("cdn_url"), platformName);
        appVersion = yaml.GetString("app_version");
        assetBundleLevel = (AssetBundleLevel)yaml.GetInt("asset_bundle_level");
        defaultLanguage = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), yaml.GetString("defualt_language"));
        var languageList = yaml.GetString("support_language").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        supportLanguageList = new List<SystemLanguage>();
        for(int i=0; i<languageList.Length; ++i)
        {
            var language = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), languageList[i]);
            supportLanguageList.Add(language);
        }

        return true;
    }

    public static string GetDebugInfo()
    {
        var builder = new System.Text.StringBuilder();
        builder.AppendLine("========== Setting ==========");
        builder.AppendFormat("builtInAssetPath: {0}\n", streamingAssetsAssetPath);
        builder.AppendFormat("downloadCachePath: {0}\n", downloadCachePath);
        builder.AppendFormat("settingFilePath: {0}\n", settingFilePath);
        builder.AppendFormat("localVersionFilePath: {0}\n", localVersionFilePath);
        builder.AppendFormat("latestVersionFileUrl: {0}\n", latestVersionFileUrl);
        builder.AppendFormat("loginServerUrl: {0}\n", loginServerUrl);
        builder.AppendFormat("cdnUrl: {0}\n", cdnUrl);
        builder.AppendFormat("appVersion: {0}\n", appVersion);
        builder.AppendLine("=============================");
        return builder.ToString();
    }

}
