using UnityEngine;

public class Killheal : MonoBehaviour
{
    Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponentInParent<Health>();

        foreach(Attack attack in GetComponentInParent<PlayerCharacterController>().GetComponentsInChildren<Attack>())
        {
            attack.OnKill += (Attack a, GameObject g, bool b) => Heal();
        }
    }


    void Heal()
    {
        Debug.Log("Killheal");
        health.Heal(1);
    }


}
