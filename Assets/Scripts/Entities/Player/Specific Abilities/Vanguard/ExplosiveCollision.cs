using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCollision : MonoBehaviour
{
    PlayerCharacterController player;
    Attack attack;

    [SerializeField]
    GameObject explosion;

    [SerializeField]
    float minimumVelocity = 20f;

    [SerializeField]
    float cooldown = 1f;

    float clock = 1f;

    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<Attack>();

        player = GetComponentInParent<PlayerCharacterController>();
        player.OnCollision += OnCollision;
        StartCoroutine(AutoDisable());
    }

    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(0.01f);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (clock > 0f)
            clock -= Time.deltaTime;
    }

    // Update is called once per frame
    void OnCollision(ControllerColliderHit hit)
    {
        if (clock > 0f)
            return;

        if (player.MoveVelocity.magnitude < minimumVelocity)
            return;

        clock = cooldown;
        Debug.Log(player.MoveVelocity.magnitude);
        GameObject instance = ObjectManager.OM.SpawnObjectFromPool(explosion.GetComponent<Poolable>().Type, explosion);
        instance.transform.position = transform.position;
        attack.SetupClone(instance.GetComponent<Attack>());
        instance.GetComponent<AttackDamage>().Damage = Mathf.Min((int)player.MoveVelocity.magnitude / 4, 20);
        instance.GetComponent<Explosion>().Radius = Mathf.Min((int)player.MoveVelocity.magnitude / 3, 18);
        instance.transform.localScale = Vector3.one * Mathf.Min((int)player.MoveVelocity.magnitude / 20, 3);
    }
}
