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
        Transform loader = transform.Find(scene);

        if (loader)
            loader.GetComponent<ScenePartLoader>().LoadScene();
        else
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }


    public void UnLoadScene(string scene)
    {
        Transform loader = transform.Find(scene);

        if (loader)
            loader.GetComponent<ScenePartLoader>().UnLoadScene();
        else
            SceneManager.UnloadSceneAsync(scene);
    }
}
