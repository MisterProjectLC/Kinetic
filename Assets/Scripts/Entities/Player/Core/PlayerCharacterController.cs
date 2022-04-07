using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerCharacterController : MonoBehaviour, Entity
{
    struct Force
    {
        public Vector3 force;
        public bool sticky;

        public Force(Vector3 force, bool sticky)
        {
            this.force = force;
            this.sticky = sticky;
        }
    }

    [Header("References")]
    [Header("Ground")]
    [Tooltip("distance from the bottom of the character controller capsule to test for grounded")]
    public float GroundCheckDistance = 0.1f;
    [Tooltip("distance from the bottom of the character controller capsule to test for grounded, airborne")]
    [SerializeField]
    float GroundCheckDistanceInAir = 0.07f;

    [Tooltip("distance from the bottom of the character controller capsule to test for grounded")]
    public float SideGroundCheckDistance = 0.1f;

    [Tooltip("Physic layers checked to consider the player grounded")]
    public LayerMask GroundCheckLayers = -1;

    [Header("Movement")]
    [Tooltip("Maximum walk speed on the ground")]
    public float GroundWalkSpeed = 10f;

    [Tooltip("Movement acceleration when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
    public float GroundAcceleration = 15f;

    [HideInInspector]
    public bool MoveControlEnabled = true;
    [HideInInspector]
    public float SpeedMultiplier = 1f;
    Dictionary<string, float> groundSlowdowns = new Dictionary<string, float>();

    [Header ("Airborne")]
    [Tooltip("Vertical movement on jumping")]
    public float JumpForce = 10f;

    float AirMultiplier = 1f;
    Dictionary<string, float> airSlowdowns = new Dictionary<string, float>();

    [Tooltip("Enable or disable gravity")]
    public bool GravityEnabled = true;

    [Tooltip("Max movement speed airborne")]
    public float AirborneMaxStrafeSpeed = 8f;

    [Tooltip("Air movement speed hard cap")]
    public float AirborneMaxSpeed = 75f;

    [Tooltip("Sharpness for the movement when airborne")]
    public float AirborneAcceleration = 8f;

    [Header("Rotation")]
    [Tooltip("Rotation speed for moving the camera")]
    public float RotationSpeed = 400f;

    public float HoverHeight = 1f;
    public float GravityMultiplier = 1f;
    protected float lastGravityMultiplier = 1f;
    public float AirDesacceleration = 0f;
    public float Weight = 1f;
    public bool IsGrounded = true;
    Queue<Force> Forces;

    protected Vector3 moveVelocity = Vector3.zero;
    public Vector3 MoveVelocity { get { return moveVelocity; } }

    Camera m_PlayerCamera;
    CharacterController m_Controller;
    PlayerInputHandler m_InputHandler;
    IFallHandler m_FallHandler;

    Vector3 m_GroundNormal;
    float m_LastTimeJumped = 0f;
    float m_CameraVerticalAngle = 0f;
    RaycastHit[] hits;

    const float k_JumpGroundingPreventionTime = 0.2f;

    // Events
    [HideInInspector]
    public UnityAction OnJumpAir;
    [HideInInspector]
    public UnityAction<ControllerColliderHit> OnCollision;
    [HideInInspector]
    public UnityAction<Collider> OnTrigger;


    protected void Awake()
    {
        m_PlayerCamera = GetComponentInChildren<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hits = new RaycastHit[1];
        Forces = new Queue<Force>();
        moveVelocity = new Vector3(0f, 0f, 0f);
        MoveControlEnabled = true;
        m_Controller = GetComponent<CharacterController>();
        m_InputHandler = GetComponent<PlayerInputHandler>();
        m_FallHandler = GetComponentInChildren<IFallHandler>();
        OnTrigger += StopOnTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        CharacterMovement();
    }

    void CameraMovement()
    {
        m_PlayerCamera.fieldOfView = Hermes.GetFloat(Hermes.Properties.FOV);

        if (!MoveControlEnabled)
            return;

        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * RotationSpeed /** 90f * Time.deltaTime*/), 
                0f), Space.Self);
        }

        // vertical camera rotation
        {
            // add vertical inputs to the camera's vertical angle
            m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * RotationSpeed /** 90f * Time.deltaTime*/;

            // limit the camera's vertical angle to min/max
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            m_PlayerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
        }
    }

    void CharacterMovement()
    {
        GroundCheck();
        Vector3 moveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

        if (MoveControlEnabled)
        {
            // Ground Movement
            if (IsGrounded)
            {
                Vector3 targetVelocity = moveInput * GroundWalkSpeed * SpeedMultiplier;
                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;
                moveVelocity = Vector3.Lerp(moveVelocity, targetVelocity, GroundAcceleration * Time.deltaTime);

                if (m_InputHandler.GetJump())
                    Jump();
            }
            // Air Movement
            else
            {
                // Horizontal air movement
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);

                // Set when normal velocity
                if (horizontalVelocity.magnitude <= AirborneMaxStrafeSpeed + 1)
                {
                    Vector3 targetVelocity = moveInput * AirborneMaxStrafeSpeed * AirMultiplier + (moveVelocity.y * Vector3.up);
                    if (targetVelocity.x != 0 || targetVelocity.z != 0)
                        moveVelocity = Vector3.Lerp(moveVelocity, targetVelocity, AirborneAcceleration/10 * Time.deltaTime);
                }
                // Adjust when super fast
                else
                {
                    Vector3 newHorizontalVelocity = horizontalVelocity + (moveInput * AirborneAcceleration * AirMultiplier * Time.deltaTime);
                    if (/*horizontalVelocity.magnitude < AirborneMaxStrafeSpeed || */newHorizontalVelocity.magnitude < horizontalVelocity.magnitude)
                        moveVelocity = newHorizontalVelocity + (moveVelocity.y * Vector3.up);
                }

                // limit air speed to a maximum, but only horizontally
                /*
                float verticalVelocity = MoveVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(MoveVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, AirborneMaxSpeed);
                MoveVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);
                */

                // Wall checks
                if (m_InputHandler.GetJump())
                    OnJumpAir?.Invoke();

                // Gravity
                if (GravityEnabled)
                    moveVelocity += Vector3.down * Constants.Gravity * GravityMultiplier * (moveVelocity.y < 0f ? AirMultiplier:1f) * Time.deltaTime;

                moveVelocity = Vector3.ClampMagnitude(moveVelocity, AirborneMaxSpeed);
            }
        }

        //Vector3 saved2 = Vector3.zero;
        while (Forces.Count > 0)
        {
            Force force = Forces.Dequeue();

            if (force.sticky && !IsGrounded)
                continue;

            moveVelocity += force.force;
            if (force.force.y > 1f && !force.sticky)
                m_LastTimeJumped = Time.time;
        }

        m_Controller.Move(moveVelocity * Time.deltaTime);
    }


    public void SetMoveVelocity(Vector3 velocity)
    {
        moveVelocity = velocity;
    }

    public void WarpPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void ReceiveForce(Vector3 Force, bool Sticky = false)
    {
        Forces.Enqueue(new Force(Force, Sticky));
    }

    public void ReceiveMotion(Vector3 Force)
    {
        if (!IsGrounded)
            return;

        Forces.Enqueue(new Force(Force, false));
    }

    public void Translate(Vector3 movement)
    {
        m_Controller.Move(movement);
    }


    public void Jump()
    {
        GetComponent<AudioSource>().Play();
        moveVelocity = Vector3.ClampMagnitude(moveVelocity, AirborneMaxStrafeSpeed);
        moveVelocity = new Vector3(moveVelocity.x, JumpForce, moveVelocity.z);
        m_LastTimeJumped = Time.time;
        IsGrounded = false;
    }


    public void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
        float chosenGroundCheckDistance = IsGrounded ? (m_Controller.skinWidth + GroundCheckDistance) : GroundCheckDistanceInAir;

        // reset values before the ground check
        IsGrounded = false;
        m_GroundNormal = Vector3.up;

        // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (Physics.SphereCastNonAlloc(GetCapsuleBottomHemisphere() - SideGroundCheckDistance * transform.up, 
                m_Controller.radius - SideGroundCheckDistance, Vector3.down, hits,
                chosenGroundCheckDistance, GroundCheckLayers, QueryTriggerInteraction.Ignore) > 0)
            {
                RaycastHit hit = hits[0];

                // storing the upward direction for the surface found
                m_GroundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f && IsNormalUnderSlopeLimit(m_GroundNormal))
                {
                    IsGrounded = true;

                    // handle snapping to the ground
                    if (hit.distance > m_Controller.skinWidth && MoveControlEnabled)
                        m_Controller.Move(Vector3.down * hit.distance);
                }
            }
        }
    }


    public RaycastHit? WallCheck()
    {
        if (Physics.CapsuleCastNonAlloc(GetCapsuleTopHemisphere(), GetCapsuleBottomHemisphere(), m_Controller.radius - Physics.defaultContactOffset,
            moveVelocity.normalized, hits, GroundCheckDistanceInAir, GroundCheckLayers) > 0)
            return hits[0];

        return null;
    }

    public void SetSlowdown(float slow, string name, bool ground = true)
    {
        bool found = false;
        float slowest = slow;
        foreach(string slowdown in (ground ? groundSlowdowns : airSlowdowns).Keys)
        {
            if (slowdown == name)
                found = true;
                
            if (slow < slowest)
                slowest = slow;
        }

        if (found)
            (ground ? groundSlowdowns : airSlowdowns)[name] = slow;
        else
            (ground ? groundSlowdowns : airSlowdowns).Add(name, slow);

        if (ground)
            SpeedMultiplier = slowest;
        else
            AirMultiplier = slowest;
    }


    void StopOnCollision(ControllerColliderHit hit)
    {
        if (Vector3.Dot(hit.moveDirection, hit.normal) <= 0)
        {
            moveVelocity = Vector3.ProjectOnPlane(moveVelocity, hit.normal);
            // Ricochet off the ceiling
            if (Vector3.Dot(Vector3.down, hit.normal) >= 0.5)
                moveVelocity += Vector3.down;
        }
        
    }

    public void StopOnTrigger(Collider other)
    {
        //MoveVelocity = Vector3.ProjectOnPlane(MoveVelocity, other.ClosestPoint(transform.position));
        //Debug.Log(print + ", " + MoveVelocity);
    }


    // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= m_Controller.slopeLimit;
    }

    public void FallFatal(float VerticalLimit, Transform FallRespawnPoint)
    {
        m_FallHandler.FallFatal(VerticalLimit, FallRespawnPoint);
    }


    #region Getters
    // Gets a reoriented direction that is tangent to a given slope
    Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
    {
        Vector3 directionRight = Vector3.Cross(direction, transform.up);
        return Vector3.Cross(slopeNormal, directionRight).normalized;
    }


    // Gets the center point of the bottom hemisphere of the character controller capsule    
    Vector3 GetCapsuleBottomHemisphere()
    {
        return transform.position + (transform.up * m_Controller.radius);
    }

    // Gets the center point of the top hemisphere of the character controller capsule    
    Vector3 GetCapsuleTopHemisphere()
    {
        return transform.position + (transform.up * (m_Controller.height - m_Controller.radius));
    }

    public Camera GetPlayerCamera()
    {
        return m_PlayerCamera;
    }

    public Vector3 GetMoveVelocity()
    {
        return moveVelocity;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    #endregion

    #region UnityMessages
    void OnDrawGizmos()
    {
        m_Controller = GetComponent<CharacterController>();
        Gizmos.DrawSphere(GetCapsuleBottomHemisphere(), m_Controller.radius);
        Gizmos.DrawSphere(GetCapsuleTopHemisphere(), m_Controller.radius);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        OnCollision?.Invoke(hit);
        StopOnCollision(hit);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(other);
    }
    #endregion

    #region StatusEffects
    public void ReceiveStatusEffect(StatusEffect statusEffect, float duration)
    {
        return;
    }

    public bool HasStatusEffect(StatusEffect statusEffect)
    {
        return false;
    }

    public bool HasAnyOfTheseStatusEffects(List<StatusEffect> statusEffects)
    {
        return false;
    }
    #endregion
}