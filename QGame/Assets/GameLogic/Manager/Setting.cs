using UnityEngine;
using System.Collections;
using QuickUnity;
using System;
using System.Collections.Generic;
using SLua;

public class Setting : Singleton<Setting>
{
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
        instance._loginServerUrl = yaml.GetString("login_server_url");
        instance._cdnUrl = FileManager.PathCombine(yaml.GetString("cdn_url"), platformName);
        instance._appVersion = yaml.GetString("app_version");
        instance._assetBundleLevel = (AssetBundleLevel)yaml.GetInt("asset_bundle_level");
        instance._defaultLanguage = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), yaml.GetString("defualt_language"));
        var languageList = yaml.GetString("support_language").Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        for(int i=0; i<languageList.Length; ++i)
        {
            var language = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), languageList[i]);
            instance._supportLanguageList.Add(language);
        }
        
        return true;
    }

    public static string GetDebugInfo()
    {
        var builder = new System.Text.StringBuilder();
        builder.AppendLine("========== Setting ==========");
        builder.AppendFormat("builtInAssetPath: {0}\n", builtInAssetPath);
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
    public static readonly string builtInAssetPath = Application.streamingAssetsPath;

    public static readonly string downloadCachePath = FileManager.PathCombine(Application.persistentDataPath, platformName);

    public static readonly string settingFilePath = "Setting/Setting";

    public static readonly string localVersionFilePath = FileManager.PathCombine(downloadCachePath, "spiral.version");

    public static string latestVersionFileUrl { get { return FileManager.PathCombine(loginServerUrl, platformName, "spiral.version"); } }

    public static string assetTableFileName = "asset_table.json";

    public static string languageFilePath = "Setting/Language";

    public static readonly string builtInAssetTableFilePath = FileManager.PathCombine(builtInAssetPath, assetTableFileName);

    public static readonly string externalAssetTableFilePath = FileManager.PathCombine(downloadCachePath, assetTableFileName);

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
    public static string loginServerUrl { get { return instance._loginServerUrl; } }
    protected string _loginServerUrl = string.Empty;

    public static string cdnUrl { get { return instance._cdnUrl; } }
    protected string _cdnUrl = string.Empty;

    public static string appVersion { get { return instance._appVersion; } }
    protected string _appVersion = string.Empty;

    public static AssetBundleLevel assetBundleLevel { get { return instance._assetBundleLevel; } }
    protected AssetBundleLevel _assetBundleLevel = AssetBundleLevel.All;

    public static SystemLanguage defaultLanguage { get { return instance._defaultLanguage; } }
    protected SystemLanguage _defaultLanguage;

    public static List<SystemLanguage> supportLanguageList { get { return instance._supportLanguageList; } }
    protected List<SystemLanguage> _supportLanguageList = new List<SystemLanguage>();


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

    public enum AssetBundleLevel
    {
        All = 0,
        AssetWithoutLua = 1,
        None = 2,
    }
}
