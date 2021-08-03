using UnityEngine;
using UnityEngine.AI;

public class FaceTarget : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("How much time it takes to update the player's supposed position")]
    [SerializeField]
    private float updateCooldown = 0.2f;

    [Tooltip("How fast does the object rotate to face the target")]
    [SerializeField]
    private float turnSpeed = 10f;

    [SerializeField]
    private bool turnVertical = true;

    private Quaternion currentPosition;
    private Vector3 newTargetPosition;
    private float clock = 0f;

    // Start is called before the first frame update
    void Start()
    {
        newTargetPosition = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform.position;
        GetComponent<Enemy>().OnActiveUpdate += OnActiveUpdate;
    }

    // Update is called once per frame
    void OnActiveUpdate()
    {
        // Constant rotation
        transform.rotation = Quaternion.RotateTowards(currentPosition, Quaternion.LookRotation(newTargetPosition - transform.position),
            turnSpeed*Time.deltaTime);
        currentPosition = transform.rotation;

        // Update targetting
        clock += Time.deltaTime;
        if (clock > updateCooldown)
        {
            clock = 0f;
            newTargetPosition = ActorsManager.Player.GetComponent<PlayerCharacterController>().PlayerCamera.transform.position;
            if (!turnVertical)
                newTargetPosition = new Vector3(newTargetPosition.x, transform.position.y, newTargetPosition.z);
        }
    }
}
