using System.Linq;
using UnityEngine;
using System;

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

    [SerializeField]
    float wallGravityDownForce = 20f;
    public float WallGravityDownForce { 
        get { return wallGravityDownForce; }
        set { wallGravityDownForce = value; wallGravityDownVector = new Vector3(0f, -wallGravityDownForce, 0f); }
    }

    [SerializeField]
    LayersConfig wallLayers;

    PlayerCharacterController player;
    Transform playerTransform;

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
    Vector3 wallGravityDownVector;
    float elapsedTimeSinceWallAttach = 0;
    float elapsedTimeSinceWallDetatch = 0;
    float currentVolumeValue = 0;
    float lastVolumeValue = 0;

    Action OnDetach;
    public void SubscribeToDetach(Action subscribee) { OnDetach += subscribee; }
    public void UnsubscribeToDetach(Action subscribee) { OnDetach -= subscribee; }


    void Start()
    {
        player = GetComponentInParent<PlayerCharacterController>();

        playerTransform = player.transform;
        wallGravityDownVector = new Vector3(0f, -wallGravityDownForce, 0f);

        player.OnJumpAir += OnJump;
        player.OnCollision += OnTouchWall;
    }

    private void OnDisable()
    {
        Debug.Log("DisableWallrun");
        player.SetRollAngle(0f);
    }


    void OnTouchWall(ControllerColliderHit hit)
    {
        OnTouchWall(hit.point, hit.normal);
    }


    #region Interface
    internal void OnJump()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (isWallRunning)
        {
            DetachJump();
            return;
        } 
        else if (CanWallRun())
        {
            RaycastHit[] hits = RegisterHits().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
            if (hits.Length > 0)
            {
                lastWallPosition = hits[0].point;
                lastWallNormal = hits[0].normal;
                AttachToWall();
            }
        }
    }

    internal void OnTouchWall(Vector3 point, Vector3 normal)
    {
        lastWallPosition = point;
        lastWallNormal = normal;
    }

    public void AttachToWall()
    {
        float d = Vector3.Dot(lastWallNormal, Vector3.up);
        if (d <= Mathf.Abs(normalizedAngleThreshold))
        {
            Vector3 alongWall = player.GetPlayerCamera().transform.TransformDirection(Vector3.forward);
            player.SetMoveVelocity(alongWall * (player.GroundWalkSpeed * wallSpeedMultiplier));
            player.GravityEnabled = false;
            isWallRunning = true;
        }
    }

    void DetachJump()
    {
        Debug.Log("DetachJump");

        Vector3 moveVelocity = GetWallJumpDirection();
        moveVelocity.x *= player.JumpForce;
        moveVelocity.y *= player.JumpForce;
        moveVelocity.z *= player.JumpForce;
        moveVelocity.x += player.MoveVelocity.x;
        moveVelocity.z += player.MoveVelocity.z;

        player.SetMoveVelocity(moveVelocity);
        DetachFromWall();
    }

    void DetachFromWall()
    {
        Debug.Log("DetachFromWall");
        isWallRunning = false;
        player.GravityEnabled = true;
        OnDetach?.Invoke();
    }
    #endregion


    void LateUpdate()
    {
        // Check if Wallrunning
        if (!CanWallRun())
        {
            DetachFromWall();
        }

        ManageCamera();
        HandleVolume();

        if (isWallRunning)
            UpdateWhenWallrunning();
        else
            UpdateWhenNotWallrunning();
    }

    #region Update Functions
    void UpdateWhenWallrunning()
    {
        elapsedTimeSinceWallDetatch = 0;
        if (elapsedTimeSinceWallAttach == 0)
            lastVolumeValue = currentVolumeValue;

        elapsedTimeSinceWallAttach += Time.deltaTime;
    }

    void UpdateWhenNotWallrunning()
    {
        elapsedTimeSinceWallAttach = 0;
        if (elapsedTimeSinceWallDetatch == 0)
            lastVolumeValue = currentVolumeValue;

        elapsedTimeSinceWallDetatch += Time.deltaTime;
    }

    void ManageCamera()
    {
        float dir = CalculateSide();
        float targetAngle = dir == 0 ? 0 : Mathf.Sign(dir) * maxAngleRoll;
        float cameraAngle = player.GetPlayerCamera().transform.eulerAngles.z;

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
    #endregion

    #region Helper Functions
    RaycastHit[] RegisterHits()
    {
        RaycastHit[] hits = new RaycastHit[DIRECTIONS.Length];
        for (int i = 0; i < DIRECTIONS.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(DIRECTIONS[i]);
            Physics.Raycast(playerTransform.position, dir, out hits[i], wallMaxDistance, wallLayers.layers, QueryTriggerInteraction.Ignore);
        }

        return hits;
    }

    bool CanWallRun()
    {
        return !player.IsGrounded /*&& input.GetMoveInput().x != 0 */
            && !Physics.Raycast(playerTransform.position, Vector3.down, minimumHeight, wallLayers.layers, QueryTriggerInteraction.Ignore);
    }

    float CalculateSide()
    {
        if (!isWallRunning)
            return 0;

        Vector3 heading = lastWallPosition - playerTransform.position;
        Vector3 perp = Vector3.Cross(playerTransform.forward, heading);
        return Vector3.Dot(perp, playerTransform.up);
    }
    #endregion

    #region Getters
    public bool IsWallRunning() => isWallRunning;

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
