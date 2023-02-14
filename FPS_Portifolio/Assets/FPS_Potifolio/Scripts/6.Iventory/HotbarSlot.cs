using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public HandableItem storedItem;
    public bool isSelected;
    public bool hasItem;

    public Color selectedColor;
    public Color defaultColor;
    private Color targetColor;
    private float colorChangeTime = 3f;


    public TextMeshProUGUI quantityText;
    public Image imageAsset;

    private void Update()
    {
        targetColor = isSelected ? selectedColor : defaultColor;
        imageAsset.material.color = Color.Lerp(imageAsset.material.color, targetColor, colorChangeTime * Time.deltaTime);
    }
    private void OnValidate()
    {
        if (hasItem)
        {
            imageAsset.sprite = storedItem.ItemSprite;
        }
    }
}