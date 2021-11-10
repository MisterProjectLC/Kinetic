using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoader : MonoBehaviour
{
    [SerializeField]
    Material disabledMaterial;
    [SerializeField]
    Material enabledMaterial;

    public List<GameTrigger> blockers;
    int blockerCount = 0;

    [SerializeField]
    List<string> ScenesLoaded;

    [SerializeField]
    List<string> ScenesUnloaded;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<GameTrigger>().OnTriggerActivate += LoadScenes;

        foreach (GameTrigger gameTrigger in blockers)
        {
            gameObject.GetComponentInChildren<Renderer>().material = disabledMaterial;
            gameTrigger.OnTriggerDestroy += RemoveBlocker;
            blockerCount++;
        }
    }

    void RemoveBlocker()
    {
        blockerCount--;
        if (blockerCount <= 0)
            gameObject.GetComponentInChildren<Renderer>().material = enabledMaterial;
    }

    void LoadScenes()
    {
        if (blockerCount > 0)
            return;

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
