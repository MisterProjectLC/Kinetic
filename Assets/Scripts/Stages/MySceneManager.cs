using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager MSM;
    [SerializeField]
    string firstScene = "";

    public enum Lifetime
    {
        ReturnOnGameover,
        ReturnOnQuit,
        Permanent
    }


    private void Awake()
    {
        MSM = this;

        if (Hermes.newGame)
        {
            Hermes.newGame = false;
            foreach (ScenePartLoader loader in transform.GetComponentsInChildren<ScenePartLoader>())
                SaveManager.Save(new List<string>(), loader.gameObject.name);
        }
    }

    private void Start()
    {
        if (firstScene != "")
            LoadFirst(firstScene);
        else
            LoadFirst(Hermes.SpawnAreas[0]);
    }


    void LoadFirst(string scene)
    {
        LoadScene(scene);
        GetComponentInChildren<Animator>().SetTrigger("Open");
    }


    public void LoadScene(string scene)
    {
        Debug.Log("Loading scene: " + scene);

        Transform loader = null;
        loader = transform.Find(scene);

        if (loader)
        {
            foreach (string sceneName in loader.GetComponent<ScenePartLoader>().GetRequiredScenes())
                LoadScene(sceneName);

            UnLoadScene('P' + scene.Substring(1));
            loader.GetComponent<ScenePartLoader>().LoadScene();
        }
        else
        {
            // Cancel if already loaded
            for (int i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).name.Substring(1) == scene.Substring(1))
                {
                    Debug.Log("Already loaded, " + scene);
                    return;
                }

            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }


    public void UnLoadScene(string scene)
    {
        Debug.Log("Unloading " + scene);

        Transform loader = null;
        loader = transform.Find(scene);

        if (loader)
        {
            loader.GetComponent<ScenePartLoader>().UnLoadScene();
            StartCoroutine(LoadPrototypeLevel(scene));
        }

        else
            for (int i = 0; i < SceneManager.sceneCount; i++)
                if (SceneManager.GetSceneAt(i).name == scene)
                {
                    SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                    break;
                }
    }

    IEnumerator LoadPrototypeLevel(string scene)
    {
        yield return new WaitForSecondsRealtime(0.05f);
        LoadScene('P' + scene.Substring(1));
    }


    public void RegisterObject(string id, string scene, Lifetime lifetime)
    {
        Transform loader = transform.Find(scene);

        if (loader)
            loader.GetComponent<ScenePartLoader>().RegisterObject(id, lifetime);
    }

    public void UnRegisterObject(string id, string scene, Lifetime lifetime)
    {
        Transform loader = transform.Find(scene);

        if (loader)
            loader.GetComponent<ScenePartLoader>().UnRegisterObject(id, lifetime);
    }


    public bool ObjectRegistered(string id, string scene)
    {
        Transform loader = transform.Find(scene);

        if (loader)
            return loader.GetComponent<ScenePartLoader>().CheckObject(id);
        else
            return false;
    }

    public ScenePartLoader GetPartLoader(string scene)
    {
        return transform.Find(scene).GetComponent<ScenePartLoader>();
    }
}
