using UnityEngine;
using System.Collections;

public class StartPage : MonoBehaviour {

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
}
