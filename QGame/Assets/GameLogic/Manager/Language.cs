using UnityEngine;
using System.Collections;
using QuickUnity;
using System.Collections.Generic;

public class Language : Singleton<Language>
{
    public static SystemLanguage currentLanguage { get; private set; }
    public static List<SystemLanguage> supportLanguage { get { return Setting.supportLanguageList; } }

    public static void Start()
    {
        currentLanguage = (SystemLanguage)UserDefault.GetInt("game_language", (int)SystemLanguage.English);
        SymbolManager.CreateLibrary(QConfig.Symbol.textLibrary);
        LoadLocalLanguage();
    }

    public static Task SwitchLanguage(SystemLanguage language)
    {
        currentLanguage = language;
        UserDefault.SetInt("game_language", (int)currentLanguage);

        SymbolManager.DeleteLibrary(QConfig.Symbol.textLibrary);
        SymbolManager.CreateLibrary(QConfig.Symbol.textLibrary);
        LoadLocalLanguage();

        return DownloadLanguagePackage();
    }

    public static Task DownloadLanguagePackage()
    {
        var task = new CustomTask();
        task.Start((_) =>
        {
            Debug.Log("Start download language package");
            AssetManager.LoadTextAsync(string.Format("Assets/_Assets/Languages/{0}.txt", currentLanguage.ToString()))
            .Finish((loadtask) =>
            {
                if (loadtask.fail)
                {
                    task.SetFail(loadtask.error);
                    return;
                }
                var kv = new KVReader((loadtask as AssetManager.LoadTextTask).text);
                SymbolManager.AddSymbols(QConfig.Symbol.textLibrary, kv.ReadDictionary());
                task.SetSuccess();
            });
        });
        return task;
    }

    private static void LoadLocalLanguage()
    {
        var text = Resources.Load(string.Format("Languages/{0}", currentLanguage.ToString())) as TextAsset;
        var kv = new KVReader(text);
        SymbolManager.AddSymbols(QConfig.Symbol.textLibrary, kv.ReadDictionary());
    }
}
