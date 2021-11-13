using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoader : MonoBehaviour
{
    [SerializeField]
    List<string> ScenesLoaded;

    [SerializeField]
    List<string> ScenesUnloaded;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<GameTrigger>().OnTriggerActivate += LoadScenes;
    }


    void LoadScenes()
    {
        foreach (string s in ScenesLoaded)
        {
            MySceneManager.MSM.LoadScene(s);
            MySceneManager.MSM.UnLoadScene('P' + s.Substring(1));
        }

        foreach (string s in ScenesUnloaded)
        {
            MySceneManager.MSM.LoadScene('P' + s.Substring(1));
            MySceneManager.MSM.UnLoadScene(s);
        }
    }
}
