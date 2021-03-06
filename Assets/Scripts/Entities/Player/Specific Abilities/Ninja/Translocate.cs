using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Translocate : SecondaryAbility
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


    public override void Execute(Input input)
    {
        // Send Ray and get Info
        Camera playerCamera = player.GetPlayerCamera();
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Physics.Raycast(ray, out RaycastHit hitInfo, Range, TargetLayers, QueryTriggerInteraction.Collide);

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
        Camera playerCamera = player.GetPlayerCamera();
        Vector3 myCoords = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y,
            playerCamera.transform.position.z);
        Vector3 targetCoords = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        player.GetComponent<CharacterController>().enabled = false;
        player.MoveControlEnabled = false;
        abilities.AbilitiesEnabled = false;
        player.SetMoveVelocity(Vector3.zero);

        if (target.GetComponent<NavMeshAgent>())
            target.GetComponent<NavMeshAgent>().enabled = false;

        // Animation
        GetComponent<AudioSource>().Play();
        float lastTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        int framesToWait = Mathf.Min(10, (int)(player.transform.position - targetCoords).magnitude / 3);
        for (int i = 0; i < framesToWait; i++)
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

        // Restore movement
        abilities.AbilitiesEnabled = true;
        player.MoveControlEnabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        if (target.GetComponent<NavMeshAgent>())
        {
            target.GetComponent<NavMeshAgent>().enabled = false;
            Debug.Log("Translocate: " + target.GetComponent<NavMeshAgent>().enabled);
        }
    }
}
