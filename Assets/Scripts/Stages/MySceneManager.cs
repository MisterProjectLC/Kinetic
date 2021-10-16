using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MySceneManager : MonoBehaviour
{
    [SerializeField]
    string sceneName = "";

    public static MySceneManager MSM;

    private void Awake()
    {
        if (sceneName != "")
            StartCoroutine(LoadFirst(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive)));

        MSM = this;
    }


    IEnumerator LoadFirst(AsyncOperation loadingScene)
    {
        while (!loadingScene.isDone) 
            yield return null;

        GetComponentInChildren<Animator>().SetTrigger("Open");
    }


    public void LoadScene(string scene)
    {
        transform.Find(scene).GetComponent<ScenePartLoader>().LoadScene();
    }
}
