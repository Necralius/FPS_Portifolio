using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ControllerModels
{
    #region - Player Settings

    public enum PlayerStance { Stand, Crouch, Prone };
    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSensitivity;
        public float ViewYSensitivity;

        public bool ViewXInverted;
        public bool ViewYInverted;

        public float AimingSensitivityEffector;

        [Header("Movment Settings")]
        public bool SpritingHold;
        public float MovmentSmoothing;

        [Header("Movment - Walking")]
        public float WalkingForwardSpeed;
        public float WalkingStrafeSpeed;
        public float WalkingBackwardSpeed;

        [Header("Movment - Running")]
        public float RunningForwardSpeed;
        public float RunningStrafeSpeed;

        [Header("Jumping")]
        public float JumpingHeight;
        public float JumpingFalloff;
        public float FallingSmoothing;

        [Header("Speed Effectors")]
        public float SpeedEffector = 1;
        public float CrouchSpeedEffector;
        public float ProneSpeedEffector;
        public float FallingSpeedEffector;
        public float AimSpeedEffector;
        [Space]
        public float AnimationEffectorMultiplier;
    }
    #endregion

    #region - Stance Model -
    [Serializable]
    public class CharacterStance
    {
        public float CameraHeight;
        public CapsuleCollider StanceCollider;
    }
    #endregion

    #region - Weapon Settings - 
    [Serializable]
    public class WeaponSettingsModel
    {
        [Header("Weapon Sway")]
        public float SwayAmount;
        public float SwaySmoothing;

        public float SwayResetSmoothing;
        public float SwayClampX;
        public float SwayClampY;
        [Space]
        public bool SwayYInverted;
        public bool SwayXInverted;

        [Header("Weapon Movment Sway")]
        public float MovmentSwayX;
        public float MovmentSwayY;
        public float MovmentSwaySmoothing;
        [Space]
        public bool MovmentSwayXInverted;
        public bool MovmentSwayYInverted;

        [Header("Aim Effectors")]
        public float IdleSwayEffector;
        public float RotationSwayEffector;
        public float AimMovmentSwayEffector;

        public float AimAnimationEffector;
    }
    #endregion
}