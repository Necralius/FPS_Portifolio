using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public HandableItem storedItem;
    public bool isSelected;

    public Color selectedColor;
    public Color defaultColor;
    private Color targetColor;
    public Image imageAsset;
    private float colorChangeTime = 3f;

    private void Update()
    {
        targetColor = isSelected ? selectedColor : defaultColor;
        imageAsset.material.color = Color.Lerp(imageAsset.material.color, targetColor, colorChangeTime * Time.deltaTime);
    }
}