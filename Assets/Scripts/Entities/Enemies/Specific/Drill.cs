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
    float RotationSpeed = 5f;
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
        StartCoroutine(Attack());
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
            enemy.ReceiveKnockback((playerTransform.position - transform.position).normalized * ChargeSpeed);
        }

        spinningObject.transform.Rotate(rotation * RotationSpeed * Time.deltaTime, Space.Self);
    }

    IEnumerator Attack()
    {
        Debug.Log("Attack " + Time.time);

        Physics.SphereCast(enemy.Model.transform.position, 2f, enemy.Model.transform.forward, out RaycastHit hitInfo,
            1f, layers.layers, QueryTriggerInteraction.Collide);
        if (hitInfo.collider)
            GetComponent<Attack>().AttackTarget(hitInfo.collider.gameObject);

        yield return new WaitForSeconds(0.05f);
        StartCoroutine(Attack());
    }
}
