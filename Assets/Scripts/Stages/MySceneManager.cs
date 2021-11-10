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
            StartCoroutine(LoadFirst(sceneName));

        MSM = this;
    }


    IEnumerator LoadFirst(string scene)
    {
        AsyncOperation loadingScene = LoadScene(scene);
        if (loadingScene != null)
            while (!loadingScene.isDone) 
                yield return null;

        GetComponentInChildren<Animator>().SetTrigger("Open");
    }


    public AsyncOperation LoadScene(string scene)
    {
        Transform loader = transform.Find(scene);

        if (loader)
            return loader.GetComponent<ScenePartLoader>().LoadScene();
        else
            return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }


    public void UnLoadScene(string scene)
    {
        Transform loader = transform.Find(scene);

        if (loader)
            loader.GetComponent<ScenePartLoader>().UnLoadScene();
        else
            for (int i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).name == scene)
                {
                    SceneManager.UnloadSceneAsync(scene);
                    break;
                }
    }


    public void RegisterObject(string id, string scene)
    {
        Transform loader = transform.Find(scene);

        if (loader)
            loader.GetComponent<ScenePartLoader>().RegisterObject(id);
    }


    public bool ObjectRegistered(string id, string scene)
    {
        Transform loader = transform.Find(scene);

        if (loader)
            return loader.GetComponent<ScenePartLoader>().CheckObject(id);
        else
            return false;
    }
}
