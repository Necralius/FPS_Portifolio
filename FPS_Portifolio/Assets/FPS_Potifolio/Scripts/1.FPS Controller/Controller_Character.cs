using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static ControllerModels;

public class Controller_Character : MonoBehaviour
{
    public static Controller_Character PlayerIntance;

    public HandableItem ItemOnHands;

    private CharacterController characterController;

    public GameObject CurrentGun;

    [Header("Main Refereces")]
    public PlayerSettingsModel playerSettings;
    public Transform cameraHolder;
    public Transform feetTransform;

    [Space]

    #region - Movment and View Inputs -
    [HideInInspector] public Vector2 input_Movment;
    [HideInInspector] public Vector2 input_View;
    #endregion

    #region - Movment Changes Vectors -
    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    private Vector3 newMovmentSpeed;
    private Vector3 newMovmentSpeedVelocity;

    [Header("Jump")]
    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    #endregion

    [Space]

    #region - Camera Clamp -
    private float viewClampYMin = -70;
    private float viewClampYMax = 80;
    #endregion

    #region - Gravity Values -
    [Header("Gravity")]
    public float gravityAmount;
    public float gravityMin;
    private float playerGravity;
    public LayerMask playerMask;
    #endregion

    #region - Debug/Use State Booleans -
    [Space]
    [Header("Player States Booleans")]
    public bool isWalking;
    public bool isGrounded;
    public bool isSprinting;
    public bool isAiming;
    #endregion

    #region - Stance System -
    [Header("Stance")]
    public PlayerStance playerStance;
    public float playerStanceSmoothing;

    public CharacterStance playerStandStance;
    public CharacterStance playerCrouchStance;
    public CharacterStance playerProneStance;

    public float stanceCheckErrorMargin = 0.02f;

    private float cameraHeight;
    private float cameraHeightVelocity;

    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleHeightVelocity;
    #endregion

    public float handableAnimationSpeed;

    private void Awake()
    {
        if (PlayerIntance != null) Destroy(PlayerIntance);
        PlayerIntance = this;

        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;
        Application.targetFrameRate = 244;

        cameraHeight = cameraHolder.localPosition.y;
    }
    private void Update()
    {
        GetInputs();            //Input capture

        CalculateView();        //Camera view calculations methods
        CalculateMovment();     //Movment and physics calculations methods
        CalculateJump();        //Jump calculations
        CalculateStance();      //Stance checks and set
    }
    private void GetInputs()
    {
        input_View = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        input_Movment = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (Input.GetButtonDown("Jump")) Jump();
        if (Input.GetKeyDown(KeyCode.LeftControl)) Crouch();
        if (Input.GetKeyDown(KeyCode.Z)) Prone();

        isAiming = Input.GetMouseButton(1) && !isSprinting;
        isWalking = isGrounded && input_Movment != Vector2.zero;
        isGrounded = characterController.isGrounded;

        Sprint();
        CalculateAiming();
    }
    private void CalculateAiming()
    {
        if (!ItemOnHands) return;

        ItemOnHands.GetComponent<GunItem>().isAiming = isAiming;
    }
    private void CalculateView()
    {
        newCharacterRotation.y += (isAiming ? playerSettings.ViewXSensitivity * playerSettings.AimingSensitivityEffector : playerSettings.ViewXSensitivity) * (playerSettings.ViewXInverted ? -input_View.x : input_View.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        newCameraRotation.x += (isAiming ? playerSettings.ViewYSensitivity * playerSettings.AimingSensitivityEffector : playerSettings.ViewYSensitivity) * (playerSettings.ViewYInverted ? input_View.y : -input_View.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }
    private void CalculateMovment()
    {
        if (input_Movment.y <= 0.2f) isSprinting = false;

        float verticalSpeed = isSprinting ? playerSettings.RunningForwardSpeed : playerSettings.WalkingForwardSpeed;
        float horizontalSpeed = isSprinting ? playerSettings.RunningStrafeSpeed : playerSettings.WalkingStrafeSpeed;


        if (!isGrounded) playerSettings.SpeedEffector = playerSettings.FallingSpeedEffector;
        else if (playerStance == PlayerStance.Crouch) playerSettings.SpeedEffector = playerSettings.CrouchSpeedEffector;
        else if (playerStance == PlayerStance.Prone) playerSettings.SpeedEffector = playerSettings.ProneSpeedEffector;
        else if (isAiming) playerSettings.SpeedEffector = playerSettings.AimSpeedEffector;
        else playerSettings.SpeedEffector = 1;

        //handableAnimationSpeed = characterController.velocity.magnitude / ((playerSettings.WalkingForwardSpeed * playerSettings.AnimationEffectorMultiplier) * playerSettings.SpeedEffector);

        if (isAiming) handableAnimationSpeed = characterController.velocity.magnitude * CurrentGun.GetComponent<GunItem>().gunSettings.AimAnimationEffector;
        else if (!isAiming) handableAnimationSpeed = (characterController.velocity.magnitude / (playerSettings.WalkingForwardSpeed * playerSettings.SpeedEffector)) / 1.3f;

        if (handableAnimationSpeed > 1) handableAnimationSpeed = 1;

        CurrentGun.GetComponent<HandableItem>().animatorSpeed = handableAnimationSpeed;

        verticalSpeed *= playerSettings.SpeedEffector;
        horizontalSpeed *= playerSettings.SpeedEffector;

        newMovmentSpeed = Vector3.SmoothDamp(newMovmentSpeed, new Vector3(horizontalSpeed * input_Movment.x * Time.deltaTime, 0, verticalSpeed * input_Movment.y * Time.deltaTime), ref newMovmentSpeedVelocity, isGrounded ? playerSettings.MovmentSmoothing : playerSettings.FallingSmoothing);
        Vector3 MovmentSpeed = transform.TransformDirection(newMovmentSpeed);

        if (playerGravity > gravityMin) playerGravity -= gravityAmount * Time.deltaTime;

        if (playerGravity < -0.1f && isGrounded) playerGravity = -0.1f;

        MovmentSpeed.y += playerGravity;
        MovmentSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(MovmentSpeed);
    }
    private void CalculateJump() => jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);
    private void CalculateStance()
    {
        CharacterStance currentStance = playerStandStance;

        if (playerStance == PlayerStance.Crouch) currentStance = playerCrouchStance;
        else if (playerStance == PlayerStance.Prone) currentStance = playerProneStance;

        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, currentStance.CameraHeight, ref cameraHeightVelocity, playerStanceSmoothing);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.StanceCollider.height, ref stanceCapsuleHeightVelocity, playerStanceSmoothing);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.StanceCollider.center, ref stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }
    private void Jump()
    {
        if (!isGrounded) return;

        if (playerStance == PlayerStance.Crouch || playerStance == PlayerStance.Prone)
        {
            if (StanceCheck(playerStandStance.StanceCollider.height)) return;
            playerStance = PlayerStance.Stand;
            return;
        }

        jumpingForce = Vector3.up * playerSettings.JumpingHeight;
        playerGravity = 0;
    }
    private void Crouch()
    {
        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(playerStandStance.StanceCollider.height)) return;

            playerStance = PlayerStance.Stand;
            return;
        }
        if (StanceCheck(playerCrouchStance.StanceCollider.height)) return;

        playerStance = PlayerStance.Crouch;
    }
    private void Prone()
    {
        if (playerStance == PlayerStance.Prone)
        {
            if (StanceCheck(playerStandStance.StanceCollider.height)) return;

            playerStance = PlayerStance.Stand;
            return;
        }
        if (StanceCheck(playerStandStance.StanceCollider.height)) return;

        playerStance = PlayerStance.Prone;
    }
    private void Sprint() => isSprinting = Input.GetKey(KeyCode.LeftShift) && (isGrounded && isWalking) && playerStance == PlayerStance.Stand ? true : false;
    private bool StanceCheck(float StanceCheckHeight)
    {
        Vector3 start = new Vector3(feetTransform.position.x, feetTransform.position.y + characterController.radius + stanceCheckErrorMargin, feetTransform.position.z);
        Vector3 end = new Vector3(feetTransform.position.x, feetTransform.position.y - characterController.radius - stanceCheckErrorMargin + StanceCheckHeight, feetTransform.position.z);

        return Physics.CheckCapsule(start, end, characterController.radius, playerMask);
    }
}