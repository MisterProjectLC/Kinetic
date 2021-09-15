using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Translocate : Ability
{
    [Header("Attributes")]
    public float Range = 100f;
    public LayerMask TargetLayers;

    PlayerCharacterController player;
    LoadoutManager abilities;

    public void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        abilities = GetComponentInParent<LoadoutManager>();
    }


    public override void Execute()
    {
        // Send Ray and get Info
        Ray ray = new Ray(player.PlayerCamera.transform.position, player.PlayerCamera.transform.forward);
        Physics.Raycast(ray, out RaycastHit hitInfo, Range, TargetLayers, QueryTriggerInteraction.Ignore);

        Collider collider = hitInfo.collider;

        if (collider == null)
            goto End;

        Enemy enemy = collider.GetComponentInParent<Enemy>();

        if (!enemy || !enemy.Movable)
            goto End;

        StartCoroutine(RunTranslocate(enemy.gameObject));
        return;

        End:
        ResetCooldown();
    }


    IEnumerator RunTranslocate(GameObject target)
    {
        Vector3 myCoords = new Vector3(player.PlayerCamera.transform.position.x, player.PlayerCamera.transform.position.y, 
            player.PlayerCamera.transform.position.z);
        Vector3 targetCoords = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        player.GetComponent<CharacterController>().enabled = false;
        player.MoveControlEnabled = false;
        abilities.AbilitiesEnabled = false;
        player.MoveVelocity = Vector3.zero;

        if (target.GetComponent<NavMeshAgent>())
            target.GetComponent<NavMeshAgent>().enabled = false;

        // Animation
        GetComponent<AudioSource>().Play();
        float lastTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        for (int i = 0; i < 10; i++)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetCoords, 0.04f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        Time.timeScale = lastTimeScale;


        // Translocate
        target.GetComponent<Enemy>().WarpPosition(new Vector3(0f, 1000f, 0f));
        player.transform.position = targetCoords;
        yield return new WaitForSecondsRealtime(0.001f);
        target.GetComponent<Enemy>().WarpPosition(myCoords);


        abilities.AbilitiesEnabled = true;
        player.MoveControlEnabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        if (target.GetComponent<NavMeshAgent>())
        {
            target.GetComponent<NavMeshAgent>().enabled = true;
            if (target.GetComponent<Enemy>().RayToGround().collider == null)
                target.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
