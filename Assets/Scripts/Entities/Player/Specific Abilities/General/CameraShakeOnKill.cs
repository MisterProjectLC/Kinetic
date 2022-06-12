using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeOnKill : MonoBehaviour
{
    [SerializeField]
    float intensity = 0.5f;
    [SerializeField]
    float duration = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        CameraShakeController shaker = GetComponent<CameraShakeController>();

        foreach (Attack attack in GetComponentInParent<PlayerCharacterController>().GetComponentsInChildren<Attack>())
        {
            attack.OnAttack += (GameObject g, float f, int i) => shaker.Shake(intensity, duration);
            attack.OnKill += (Attack a, GameObject g, bool b) => shaker.Shake(intensity*3, duration*2);
        }
    }
}
