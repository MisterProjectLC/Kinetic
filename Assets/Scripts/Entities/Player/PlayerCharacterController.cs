using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerCharacterController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the main camera used for the player")]
    public Camera PlayerCamera;

    [Header("Ground")]
    [Tooltip("distance from the bottom of the character controller capsule to test for grounded")]
    public float GroundCheckDistance = 0.1f;

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

    [Header ("Airborne")]
    [Tooltip("Vertical movement on jumping")]
    public float JumpForce = 10f;

    [Tooltip("Max movement speed airborne")]
    public float GravityMultiplier = 1f;

    [Tooltip("Max movement speed airborne")]
    public float AirborneMaxSpeed = 8f;


    [Tooltip("Sharpness for the movement when airborne")]
    public float AirborneAcceleration = 8f;

    [Header("Rotation")]
    [Tooltip("Rotation speed for moving the camera")]
    public float RotationSpeed = 200f;


    public float RotationMultiplier
    {
        get
        {
            return 1f;
        }
    }


    public Vector3 MoveVelocity { get; set; }
    public bool IsGrounded { get; private set; } = true;
    CharacterController m_Controller;
    PlayerInputHandler m_InputHandler;
    Vector3 m_CharacterVelocity;
    Vector3 m_LatestImpactSpeed;
    Vector3 m_GroundNormal;
    float m_LastTimeJumped = 0f;
    float m_CameraVerticalAngle = 0f;

    const float k_JumpGroundingPreventionTime = 0.2f;
    const float k_GroundCheckDistanceInAir = 0.07f;


    // Start is called before the first frame update
    void Start()
    {
        MoveVelocity = new Vector3(0f, 0f, 0f);
        m_Controller = GetComponent<CharacterController>();
        m_InputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        CharacterMovement();
    }


    public void CameraMovement()
    {
        if (!MoveControlEnabled)
            return;

        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * RotationSpeed * RotationMultiplier), 0f), Space.Self);
        }

        // vertical camera rotation
        {
            // add vertical inputs to the camera's vertical angle
            m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * RotationSpeed * RotationMultiplier;

            // limit the camera's vertical angle to min/max
            m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            PlayerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
        }
    }


    public void CharacterMovement()
    {
        GroundCheck();
        Vector3 moveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

        if (MoveControlEnabled)
        {
            // Ground Movement
            if (IsGrounded)
            {
                Vector3 targetVelocity = moveInput * GroundWalkSpeed;
                targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, m_GroundNormal) * targetVelocity.magnitude;
                MoveVelocity = Vector3.Lerp(MoveVelocity, targetVelocity, GroundAcceleration * Time.deltaTime);

                if (m_InputHandler.GetJump())
                    Jump();

                // Air Movement
            }
            else
            {
                MoveVelocity += moveInput * AirborneAcceleration * Time.deltaTime;

                // limit air speed to a maximum, but only horizontally
                float verticalVelocity = MoveVelocity.y;
                Vector3 horizontalVelocity = Vector3.ProjectOnPlane(MoveVelocity, Vector3.up);
                horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, AirborneMaxSpeed);
                MoveVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

                // Gravity
                MoveVelocity += Vector3.down * Constants.Gravity * GravityMultiplier * Time.deltaTime;
            }
        }

        m_Controller.Move(MoveVelocity * Time.deltaTime);
    }


    public void Jump()
    {
        MoveVelocity = new Vector3(MoveVelocity.x, JumpForce, MoveVelocity.z);
        m_LastTimeJumped = Time.time;
        IsGrounded = false;
    }


    public void GroundCheck()
    {
        // Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
        float chosenGroundCheckDistance = IsGrounded ? (m_Controller.skinWidth + GroundCheckDistance) : k_GroundCheckDistanceInAir;

        // reset values before the ground check
        IsGrounded = false;
        m_GroundNormal = Vector3.up;

        // only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
        if (Time.time >= m_LastTimeJumped + k_JumpGroundingPreventionTime)
        {
            // if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
            if (Physics.SphereCast(GetCapsuleBottomHemisphere(), m_Controller.radius - Physics.defaultContactOffset, Vector3.down, out RaycastHit hit,
                chosenGroundCheckDistance, GroundCheckLayers, QueryTriggerInteraction.Ignore))
            {
                // storing the upward direction for the surface found
                m_GroundNormal = hit.normal;

                // Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
                // and if the slope angle is lower than the character controller's limit
                if (Vector3.Dot(hit.normal, transform.up) > 0f && IsNormalUnderSlopeLimit(m_GroundNormal))
                {
                    IsGrounded = true;

                    // handle snapping to the ground
                    if (hit.distance > m_Controller.skinWidth)
                        m_Controller.Move(Vector3.down * hit.distance);
                }
            }
        }
    }


    // Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
    bool IsNormalUnderSlopeLimit(Vector3 normal)
    {
        return Vector3.Angle(transform.up, normal) <= m_Controller.slopeLimit;
    }


    // Gets a reoriented direction that is tangent to a given slope
    public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
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

    void OnDrawGizmos()
    {
        m_Controller = GetComponent<CharacterController>();
        Gizmos.DrawSphere(GetCapsuleBottomHemisphere(), m_Controller.radius);
        Gizmos.DrawSphere(GetCapsuleTopHemisphere(), m_Controller.radius);
    }
}