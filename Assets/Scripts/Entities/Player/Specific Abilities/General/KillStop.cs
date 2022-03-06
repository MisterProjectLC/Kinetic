using System.Collections;
using UnityEngine;

public class KillStop : MonoBehaviour
{
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponentInParent<Health>();

        foreach (Attack attack in GetComponentInParent<PlayerCharacterController>().GetComponentsInChildren<Attack>())
        {
            attack.OnKill += (Attack a, GameObject g, bool b) => { StartCoroutine(Freeze()); };
        }
    }

    IEnumerator Freeze()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
    }

}
