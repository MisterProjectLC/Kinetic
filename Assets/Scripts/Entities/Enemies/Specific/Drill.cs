using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour
{
    Enemy enemy;
    Transform playerTransform;

    Vector3 rotation = new Vector3(0f, 0f, 1f);

    [SerializeField]
    GameObject spinningObject;
    [SerializeField]
    LayersConfig layers;
    [SerializeField]
    float RotationSpeed = 150f;
    [SerializeField]
    float ChargeSpeed = 200f;
    [SerializeField]
    float ChargeCooldown = 3f;
    float clock = 0f;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemy.OnActiveUpdate += ChargeUpdate;
        playerTransform = ActorsManager.AM.GetPlayer().transform;
    }

    // Update is called once per frame
    void ChargeUpdate()
    {
        if (!enemy.IsPlayerInView())
            return;

        clock += Time.deltaTime;
        if (clock > ChargeCooldown)
        {
            clock = Random.Range(0f, 1f);
            enemy.ReceiveKnockback(enemy.Model.transform.forward.normalized * ChargeSpeed);
            StartCoroutine(Attack());
        }

        spinningObject.transform.Rotate(rotation * RotationSpeed * (clock/ChargeCooldown) * Time.deltaTime, Space.Self);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.1f);
        Physics.SphereCast(enemy.Model.transform.position, 1f, enemy.Model.transform.forward, out RaycastHit hitInfo,
            5f, layers.layers, QueryTriggerInteraction.Collide);
        if (hitInfo.collider && hitInfo.collider.GetComponentInParent<Drill>() != this)
            GetComponent<Attack>().AttackTarget(hitInfo.collider.gameObject);

        if (enemy.GetMoveVelocity().magnitude >= 0.5f)
                StartCoroutine(Attack());
    }
}