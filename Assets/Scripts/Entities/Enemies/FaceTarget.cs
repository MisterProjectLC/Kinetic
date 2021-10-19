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

    [SerializeField]
    [Tooltip("If null, takes the enemy's Model.")]
    GameObject partThatMoves;

    private Quaternion currentPosition;
    private Vector3 newTargetPosition;
    Transform playerPosition;
    private float clock = 0f;

    Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        playerPosition = ActorsManager.Player.GetComponentInChildren<Camera>().transform;
        newTargetPosition = playerPosition.position;
        enemy = GetComponent<Enemy>();
        enemy.OnActiveUpdate += OnActiveUpdate;

        if (!partThatMoves)
            partThatMoves = enemy.Model;
    }

    // Update is called once per frame
    void OnActiveUpdate()
    {
        // Constant rotation
        partThatMoves.transform.rotation = Quaternion.RotateTowards(currentPosition, 
            Quaternion.LookRotation(newTargetPosition - partThatMoves.transform.position), turnSpeed*Time.deltaTime);
        currentPosition = partThatMoves.transform.rotation;

        // Update targetting
        clock += Time.deltaTime;
        if (clock > updateCooldown)
        {
            clock = 0f;
            newTargetPosition = playerPosition.position;
            if (!turnVertical)
                newTargetPosition = new Vector3(newTargetPosition.x, partThatMoves.transform.position.y, newTargetPosition.z);
        }
    }
}
