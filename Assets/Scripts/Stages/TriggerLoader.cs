using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoader : MonoBehaviour
{
    [SerializeField]
    List<string> ScenesLoaded;

    [SerializeField]
    List<string> ScenesUnloaded;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<GameTrigger>().SubscribeToTriggerActivate(LoadScenes);
    }


    void LoadScenes()
    {
        foreach (string s in ScenesLoaded)
            MySceneManager.MSM.LoadScene(s);

        foreach (string s in ScenesUnloaded)
            MySceneManager.MSM.UnLoadScene(s);
    }
}
