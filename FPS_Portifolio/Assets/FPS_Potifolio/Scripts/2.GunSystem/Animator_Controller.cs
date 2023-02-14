using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator_Controller : MonoBehaviour
{
    public void StopReload() => Controller_Character.PlayerIntance.StopReload();
    public void StopDrawWeapon() => Controller_Character.PlayerIntance.CurrentGun.GetComponent<GunItem>().FinishWeaponDraw();
}