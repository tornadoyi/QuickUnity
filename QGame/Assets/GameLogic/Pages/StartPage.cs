using UnityEngine;
using System.Collections;

public class StartPage : MonoBehaviour {

    bool changeLanguage = false;

    public Initializer initializer
    {
        get { return GameObject.Find("Initializer").GetComponent<Initializer>(); }
    }


    public void TestAsset()
    {
        if (!initializer.initFinished) return;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Show");
    }

    public void TestNetwork()
    {
        if (!initializer.initFinished) return;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Network");
    }

    public void SwitchLanguage()
    {
        if (changeLanguage) return;
        changeLanguage = true;

        var currentLanguage = Language.currentLanguage;
        SystemLanguage nextLanguage = currentLanguage;
        switch (currentLanguage)
        {
            case SystemLanguage.English:
                nextLanguage = SystemLanguage.ChineseSimplified;
                break;
            case SystemLanguage.ChineseSimplified:
                nextLanguage = SystemLanguage.ChineseTraditional;
                break;
            default:
                nextLanguage = SystemLanguage.English;
                break;
        }

        Language.SwitchLanguage(nextLanguage).Finish((_)=> 
        {
            changeLanguage = false;
            initializer.Restart();
        });
    }
}
