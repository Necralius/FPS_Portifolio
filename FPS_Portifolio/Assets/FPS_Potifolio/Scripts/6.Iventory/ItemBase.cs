using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Handable, Consumable, Ammunation, Resource};
public class ItemBase : MonoBehaviour
{
    
    [HideInInspector] public int ItemQuantity;
    [HideInInspector] public Sprite ItemSprite;
    [HideInInspector] public string ItemName;
    [HideInInspector] public string ItemDescription;
    [HideInInspector] public int ItemMaxQuantity;
    public ItemType itemType;

    public void CheckItem()
    {
        if (itemType == ItemType.Handable) ItemQuantity = 1;
    }
}