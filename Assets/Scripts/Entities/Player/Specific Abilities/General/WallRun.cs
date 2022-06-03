using System.Linq;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    public float wallMaxDistance = 1;
    public float wallSpeedMultiplier = 1.2f;
    public float minimumHeight = 1.2f;
    public float maxAngleRoll = 20;
    [Range(0.0f, 1.0f)]
    public float normalizedAngleThreshold = 0.1f;
    
    public float jumpDuration = 1;
    public float wallBouncing = 3;
    public float cameraTransitionDuration = 1;

    public float wallGravityDownForce = 20f;

    [SerializeField]
    LayersConfig wallLayers;

    PlayerCharacterController player;
    Transform playerTransform;
    PlayerInputHandler input;

    Vector3[] DIRECTIONS = new Vector3[5]{
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.forward,
            Vector3.left + Vector3.forward,
            Vector3.left
        };

    bool isWallRunning = false;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;
    float elapsedTimeSinceJump = 0;
    float elapsedTimeSinceWallAttach = 0;
    float elapsedTimeSinceWallDetatch = 0;
    float currentVolumeValue = 0;
    float lastVolumeValue = 0;
    float noiseAmplitude;


    void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();
        playerTransform = player.transform;
        input = GetComponentInParent<PlayerInputHandler>();
    }


    public void LateUpdate()
    {  
        // Check if Wallrunning
        isWallRunning = false;
        bool canAttach = CanAttachCheck();
        if (canAttach && CanWallRun())
        {
           RaycastHit[] hits = RegisterHits().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
            if(hits.Length > 0)
            {
                AttachToWall(hits[0]);
                lastWallPosition = hits[0].point;
                lastWallNormal = hits[0].normal;
            }
        }

        if (isWallRunning)
            UpdateWhenWallrunning();
        else
            UpdateWhenNotWallrunning();

        ManageCamera();
        HandleVolume();
    }

    RaycastHit[] RegisterHits()
    {
        RaycastHit[] hits = new RaycastHit[DIRECTIONS.Length];
        for (int i = 0; i < DIRECTIONS.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(DIRECTIONS[i]);
            Physics.Raycast(playerTransform.position, dir, out hits[i], wallMaxDistance, wallLayers.layers, QueryTriggerInteraction.Ignore);
#if UNITY_EDITOR
            if (!hits[i].collider)
                Debug.DrawRay(playerTransform.position, dir * hits[i].distance, Color.green);
            else
                Debug.DrawRay(playerTransform.position, dir * wallMaxDistance, Color.red);
#endif
        }

        return hits;
    }

    void UpdateWhenWallrunning()
    {
        elapsedTimeSinceWallDetatch = 0;
        if (elapsedTimeSinceWallAttach == 0)
            lastVolumeValue = currentVolumeValue;

        elapsedTimeSinceWallAttach += Time.deltaTime;
        //player.ReceiveForce(alongWall * vertical * player.GroundWalkSpeed * wallSpeedMultiplier);
        player.ReceiveForce(Vector3.down * wallGravityDownForce * Time.deltaTime);

        if (input.GetJump())
            DetachJump();
    }

    void UpdateWhenNotWallrunning()
    {
        elapsedTimeSinceWallAttach = 0;
        if (elapsedTimeSinceWallDetatch == 0)
            lastVolumeValue = currentVolumeValue;

        elapsedTimeSinceWallDetatch += Time.deltaTime;
    }

    void DetachJump()
    {
        Vector3 moveVelocity = GetWallJumpDirection();
        moveVelocity.x *= player.JumpForce;
        moveVelocity.y *= player.JumpForce;
        moveVelocity.z *= player.JumpForce;
        moveVelocity.x += player.MoveVelocity.x;
        moveVelocity.z += player.MoveVelocity.z;

        player.SetMoveVelocity(moveVelocity);
    }

    public void ManageCamera()
    {
        float dir = CalculateSide();
        float cameraAngle = player.GetPlayerCamera().transform.eulerAngles.z;
        float targetAngle = dir == 0 ? 0 : Mathf.Sign(dir) * maxAngleRoll;

        player.SetRollAngle(Mathf.LerpAngle(cameraAngle, targetAngle,
            Mathf.Max(elapsedTimeSinceWallAttach, elapsedTimeSinceWallDetatch) / cameraTransitionDuration));
    }

    void HandleVolume()
    {
        float w = 0;
        if (isWallRunning)
            w = Mathf.Lerp(lastVolumeValue, 1, elapsedTimeSinceWallAttach / cameraTransitionDuration);
        else
            w = Mathf.Lerp(lastVolumeValue, 0, elapsedTimeSinceWallDetatch / cameraTransitionDuration);

        SetVolumeWeight(w);
    }


    bool jumping = false;
    bool CanAttachCheck()
    {
        // Detect Jump
        if (input.GetJump())
            jumping = true;

        if (!jumping)
            return true;

        elapsedTimeSinceJump += Time.deltaTime;
        if(elapsedTimeSinceJump > jumpDuration)
        {
            elapsedTimeSinceJump = 0;
            jumping = false;
        }
        return false;
    }

    void AttachToWall(RaycastHit hit) {
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if (d <= Mathf.Abs(normalizedAngleThreshold))
        {
            Debug.Log("AttachToWall");

            float vertical = input.GetMoveInput().z;
            Vector3 alongWall = player.GetPlayerCamera().transform.TransformDirection(Vector3.forward);
            player.SetMoveVelocity(alongWall * vertical * player.GroundWalkSpeed * wallSpeedMultiplier);
            isWallRunning = true;

#if UNITY_EDITOR
            Debug.DrawRay(transform.position, alongWall.normalized * 10, Color.green);
            Debug.DrawRay(transform.position, lastWallNormal * 10, Color.magenta);
#endif
        }
    }

    #region Getters
    public bool IsWallRunning() => isWallRunning;

    bool CanWallRun()
    {
        float forwardInput = input.GetMoveInput().z;
        return !player.IsGrounded && forwardInput > 0 && VerticalCheck();
    }

    bool VerticalCheck()
    {
        return !Physics.Raycast(playerTransform.position, Vector3.down, minimumHeight);
    }

    float CalculateSide()
    {
        if (!isWallRunning)
            return 0;

        Vector3 heading = lastWallPosition - transform.position;
        Vector3 perp = Vector3.Cross(transform.forward, heading);
        return Vector3.Dot(perp, transform.up);
    }

    public Vector3 GetWallJumpDirection()
    {
        Vector3 v = lastWallNormal;
        v.x *= wallBouncing;
        v.y *= wallBouncing;
        v.z *= wallBouncing;
        v.y += 1;

        return isWallRunning ? v : Vector3.zero;
    }
    #endregion

    #region Setters
    void SetVolumeWeight(float weight)
    {
        currentVolumeValue = weight;
    }
    #endregion
}
