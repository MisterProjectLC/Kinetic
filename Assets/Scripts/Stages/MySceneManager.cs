using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    [SerializeField]
    string sceneName = "";

    const string TRANSITION = "TransitionLayer";

    private void Awake()
    {
        if (sceneName != "")
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
}
