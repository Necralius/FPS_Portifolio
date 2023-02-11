using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static ControllerModels;

public class GunItem : HandableItem
{
    #region - Default Weapon States -

    public bool isWalking = false;
    public bool isSprinting = false;
    public bool isReloading = false;
    public bool isAiming = false;
    public bool isHolsted = false;
    #endregion

    private Controller_Character playerController;
    private Camera playerCam => playerController.cameraHolder.GetComponentInChildren<Camera>();

    [Header("Gun Control Settings")]
    public WeaponSettingsModel gunSettings;

    public GunShooter ShootAsset;

    #region - General Smooth Variables -
    Vector3 newWeaponRotation;
    Vector3 newWeaponRotationVelocity;

    Vector3 targetWeaponRotation;
    Vector3 targetWeaponRotationVelocity;

    Vector3 newWeaponMovmentRotation;
    Vector3 newWeaponMovmentRotationVelocity;

    Vector3 targetWeaponMovmentRotation;
    Vector3 targetWeaponMovmentRotationVelocity;
    #endregion

    [Space]

    #region - Idle Sway -
    [Header("Idle Weapon Sway")]
    public Transform weaponSwayObject;

    public float idleSwayAmountA = 1;
    public float idleSwayAmountB = 2;
    public float swayScale = 600;
    public float swayLerpSpeed = 14;

    float swayTime;

    private Vector3 swayPosition;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;
    #endregion

    [Space]

    #region - Aim System -
    [Header("Aiming Sights")]
    public Transform sightTarget;
    public float sightOffset;
    public float defaultSightOffset;
    public float reloadingSightOffset;
    public float aimingTime;
    public float aimingSacaleFactor = 4;

    public float defaultFov;
    public float aimFov;
    private float currentFov;
    public float aimTime;

    #endregion

    public Transform shootPoint;

    #region - Ammo System -
    [Header("Ammo")]
    public int currentMagAmmo;
    public int magMaxAmmo;

    public int inventoryAmmo;
    public int inventoryMaxAmmo;
    #endregion

    public bool canShoot = true;

    public LayerMask HitInteractionLayer;
    public GunProceduralRecoil recoilAsset => GetComponent<GunProceduralRecoil>();

    public Transform objectToDebug;
    public Vector3 PosDebugger;

    [Header("Audio System")]
    public AudioClip shootSound;

    private void Awake()
    {
        inventoryAmmo = inventoryMaxAmmo;
        playerController = Controller_Character.PlayerIntance;
        CheckItem();
    }
    private void Start() => newWeaponRotation = transform.localRotation.eulerAngles;
    private void Update()
    {
        if (objectToDebug != null) PosDebugger = objectToDebug.transform.localPosition;
        
        PlayerStateData();
        AnimationCalculations();
        CalculateWeaponRotation();
        CalculateWeaponSway();
        CalculateAimingIn();
        ShootInput();
        AnimationsStates();
        GetInputs();
        UpdateTexts();
    }
    private void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentMagAmmo < magMaxAmmo) Reload();
    }
    private void CalculateAimingIn()
    {
        currentFov = isAiming ? aimFov : defaultFov;

        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, currentFov, aimTime * Time.deltaTime);

        sightOffset = isReloading && isAiming ? reloadingSightOffset : defaultSightOffset;

        Vector3 targetPosition = transform.position;

        if (isAiming) targetPosition = playerController.cameraHolder.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + (playerController.cameraHolder.transform.forward * sightOffset);

        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingTime);
        weaponSwayObject.transform.position = weaponSwayPosition + swayPosition;
    }
    private void PlayerStateData()
    {
        isWalking = playerController.isWalking; isSprinting = playerController.isSprinting;
    }
    private void AnimationCalculations()
    { 
        if (handableAnim != null)
        {
            handableAnim.SetBool("IsWalking", isWalking); handableAnim.SetBool("IsRunning", isSprinting);
        }
        else return;
    }
    private void CalculateWeaponRotation()
    {
        handableAnim.speed = this.animatorSpeed;
        modelAnimator.speed = this.animatorSpeed;

        targetWeaponRotation.y +=  (isAiming ? gunSettings.SwayAmount / gunSettings.RotationSwayEffector : gunSettings.SwayAmount) * (gunSettings.SwayXInverted ? playerController.input_View.x : -playerController.input_View.x) * Time.deltaTime;
        targetWeaponRotation.x +=  (isAiming ? gunSettings.SwayAmount / gunSettings.RotationSwayEffector : gunSettings.SwayAmount) * (gunSettings.SwayYInverted ? playerController.input_View.y : -playerController.input_View.y) * Time.deltaTime;

        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -gunSettings.SwayClampX, gunSettings.SwayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -gunSettings.SwayClampY, gunSettings.SwayClampY);

        targetWeaponRotation.z = isAiming ? 0 : targetWeaponRotation.y;

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, gunSettings.SwayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, gunSettings.SwaySmoothing);

        targetWeaponMovmentRotation.z = (isAiming ? gunSettings.MovmentSwayX / gunSettings.AimMovmentSwayEffector : gunSettings.MovmentSwayX) * (gunSettings.MovmentSwayXInverted ? playerController.input_Movment.x : -playerController.input_Movment.x);
        targetWeaponMovmentRotation.x = (isAiming ? gunSettings.MovmentSwayY / gunSettings.AimMovmentSwayEffector : gunSettings.MovmentSwayY) * (gunSettings.MovmentSwayYInverted ? playerController.input_Movment.y : -playerController.input_Movment.y);

        targetWeaponMovmentRotation = Vector3.SmoothDamp(targetWeaponMovmentRotation, Vector3.zero, ref targetWeaponMovmentRotationVelocity, gunSettings.MovmentSwaySmoothing);
        newWeaponMovmentRotation = Vector3.SmoothDamp(newWeaponMovmentRotation, targetWeaponMovmentRotation, ref newWeaponMovmentRotationVelocity, gunSettings.MovmentSwaySmoothing);

        weaponSwayObject.transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovmentRotation);
    }
    private void CalculateWeaponSway()
    {
        Vector3 targetPosition = LissajousCurve(swayTime, idleSwayAmountA, idleSwayAmountB) / (isAiming ? swayScale * gunSettings.IdleSwayEffector : swayScale);

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;

        if (swayTime > 6.3f) swayTime = 0;
    }
    private Vector3 LissajousCurve(float Time, float A, float B) => new Vector3(Mathf.Sin(Time), A * Mathf.Sin(B * Time + Mathf.PI));
    private void ShootInput()
    {
        if (Input.GetMouseButton(0) && canShoot && !(currentMagAmmo <= 0) && !isReloading && !isSprinting)
        {
            recoilAsset.RecoilFire(isAiming);
            canShoot = false;
            currentMagAmmo--;
            StartCoroutine(ShootAction());
        }
    }
    IEnumerator ShootAction()
    {
        RaycastShoot(playerController.cameraHolder.GetComponentInChildren<Camera>().transform.forward, ShootAsset.shootType);
        GameManager.Instance.PlayShootSound(shootSound, 0.8f, 1f);
        ShootAsset.InstatiateParticles("MuzzleFlash", shootPoint.transform.position, shootPoint.transform.eulerAngles);

        yield return new WaitForSeconds(ShootAsset.RateOfFire);
        canShoot = true;
    }
    private void RaycastShoot(Vector3 Direction, ShootType Type)
    {
        Debug.Log("Shooted!");

        if (Type == ShootType.AutoOrSemi)
        {
            if (Physics.Raycast(playerController.cameraHolder.GetComponentInChildren<Camera>().transform.position, Direction, out ShootAsset.hit, ShootAsset.ShootRange, HitInteractionLayer)) ShootAsset.InstantiateSelectedParticles(ShootAsset.hit.transform.tag, ShootAsset.hit.point, ShootAsset.hit.normal);
        }
        else if (Type == ShootType.BurstShot)
        {
            for (int i = 0; i < ShootAsset.ShootsPerTap; i++) if (Physics.Raycast(playerController.cameraHolder.GetComponentInChildren<Camera>().transform.position, Direction, out ShootAsset.hit, ShootAsset.ShootRange, HitInteractionLayer)) ShootAsset.InstantiateSelectedParticles(ShootAsset.hit.transform.tag, ShootAsset.hit.point, ShootAsset.hit.normal);
        }
        else if (Type == ShootType.Shotgun)
        {
            float X = Random.Range(-ShootAsset.ShootSpread, ShootAsset.ShootSpread);
            float Y = Random.Range(-ShootAsset.ShootSpread, ShootAsset.ShootSpread);

            Vector3 direction = Direction + new Vector3(X, Y, 0);

            for (int i = 0; i < ShootAsset.ShootsPerTap; i++) if (Physics.Raycast(playerController.cameraHolder.GetComponentInChildren<Camera>().transform.position, direction, out ShootAsset.hit, ShootAsset.ShootRange, HitInteractionLayer)) ShootAsset.InstantiateSelectedParticles(ShootAsset.hit.transform.tag, ShootAsset.hit.point, ShootAsset.hit.normal);
        }
    }
    private void UpdateTexts() => playerController.ammoText.text = string.Format("{0}/{1}", currentMagAmmo, inventoryAmmo);
    private void AnimationsStates() => modelAnimator.SetBool("Reloading", isReloading);
    private void Reload() => isReloading = true;
    public void EndReload()//Need remodeling
    {
        isReloading = false;
        int neededAmmo = magMaxAmmo - currentMagAmmo;
        if (neededAmmo > inventoryAmmo)
        {
            currentMagAmmo = inventoryAmmo;
            inventoryAmmo = 0;
        }
        else if (neededAmmo < inventoryAmmo)
        {
            currentMagAmmo = magMaxAmmo;
            inventoryAmmo -= magMaxAmmo;
        }
    }
}