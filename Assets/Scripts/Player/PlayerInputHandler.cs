using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Tooltip("Sensitivity multiplier for moving the camera around")]
    [SerializeField]
    private float LookSensitivity = 1f;

    [Header("Inverting Axis (and Allies lol)")]
    [Tooltip("Used to flip the vertical input axis")]
    [SerializeField]
    private bool InvertYAxis = false;

    [Tooltip("Used to flip the horizontal input axis")]
    [SerializeField]
    private bool InvertXAxis = false;

    private bool inputEnabled { get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetMoveInput()
    {
        if (inputEnabled)
        {
            Vector3 move = new Vector3(Input.GetAxisRaw(Constants.MoveHorizontal), 0f, Input.GetAxisRaw(Constants.MoveVertical));

            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseOrStickLookAxis(Constants.MouseHorizontal, Constants.JoystickHorizontal, InvertXAxis);
    }

    public float GetLookInputsVertical()
    {
        return GetMouseOrStickLookAxis(Constants.MouseVertical, Constants.JoystickVertical, !InvertYAxis);
    }


    public bool GetJump()
    {
        return Input.GetButtonDown(Constants.Jump);
    }

    public bool GetAbility(int number)
    {
        return Input.GetButtonDown(Constants.Ability + number.ToString());
    }


    float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName, bool inverter)
    {
        if (!inputEnabled)
            return 0f;

        // Check if this look input is coming from the mouse
        bool isGamepad = Input.GetAxis(stickInputName) != 0f;
        float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

        // handle inverting vertical input
        if (inverter)
            i *= -1f;

        // apply sensitivity multiplier
        i *= LookSensitivity;

        if (isGamepad)
            // since mouse input is already deltaTime-dependant, only scale input with frame time if it's coming from sticks
            i *= Time.deltaTime;
        else
        {
            // reduce mouse input amount to be equivalent to stick movement
            i *= 0.01f;
            #if UNITY_WEBGL
                // Mouse tends to be even more sensitive in WebGL due to mouse acceleration, so reduce it even more
                i *= WebglLookSensitivityMultiplier;
            #endif
        }

        return i;
    }
}
