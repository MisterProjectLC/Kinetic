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
            attack.OnKill += Heal;
        }
    }


    void Heal()
    {
        health.Heal(1);
    }


}
