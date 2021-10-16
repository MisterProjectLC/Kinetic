using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoader : MonoBehaviour
{
    [SerializeField]
    List<string> ScenesLoaded;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<GameTrigger>().OnTriggerActivate += LoadScenes;
    }

    void LoadScenes()
    {
        foreach (string s in ScenesLoaded)
        {
            MySceneManager.MSM.LoadScene(s);
        }
    }
}
