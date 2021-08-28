using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    [SerializeField]
    string sceneName = "";

    private void Awake()
    {
        if (sceneName != "")
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
