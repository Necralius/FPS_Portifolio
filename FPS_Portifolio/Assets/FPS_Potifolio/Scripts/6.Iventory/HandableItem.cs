using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HandableItem : ItemBase
{
    [Header("Item Animation System")]
    [HideInInspector] public Animator handableAnim;
    [HideInInspector] public Animator modelAnimator;
    [HideInInspector] public float animatorSpeed;

    [Header("Gun States")]
    [HideInInspector] public bool InHands;
}