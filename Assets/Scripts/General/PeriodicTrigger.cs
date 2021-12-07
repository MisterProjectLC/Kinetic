using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicTrigger : MonoBehaviour
{
    [SerializeField]
    float Cooldown = 1f;
    float clock = 0f;

    GameTrigger gameTrigger;

    private void Start()
    {
        gameTrigger = GetComponent<GameTrigger>();
    }

    // Update is called once per frame
    void Update()
    {

        clock += Time.deltaTime;
        if (clock > Cooldown)
        {
            clock = 0f;
            gameTrigger.Activate();
        }
    }
}
