using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemBase))]
public class ItemBase_CI : Editor
{
    ItemBase Target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Target = (ItemBase)target;

        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Item Icon Sprite", EditorStyles.boldLabel, GUILayout.Width(100f));
        Target.ItemSprite = (Sprite)EditorGUILayout.ObjectField(Target.ItemSprite, typeof(Sprite), GUILayout.Width(70f), GUILayout.Height(70f));
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Item Name", GUILayout.MaxWidth(100f));
        Target.ItemName = (string)EditorGUILayout.TextArea(Target.ItemName, GUILayout.MaxWidth(100f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Item Description", GUILayout.MaxWidth(100f), GUILayout.MaxHeight(25f));
        Target.ItemDescription = (string)EditorGUILayout.TextArea(Target.ItemDescription, GUILayout.MaxWidth(200f), GUILayout.MaxHeight(45f));
        GUILayout.EndHorizontal();

        if (Target.itemType != ItemType.Handable)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Item Quantity", GUILayout.MaxWidth(100f));
            Target.ItemQuantity = (int)EditorGUILayout.IntField(Target.ItemQuantity, GUILayout.MaxWidth(32f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Item Max Quantity", GUILayout.MaxWidth(100f));
            Target.ItemMaxQuantity = (int)EditorGUILayout.IntField(Target.ItemMaxQuantity, GUILayout.MaxWidth(32f));
            GUILayout.EndHorizontal();

            if (Target.ItemQuantity > Target.ItemMaxQuantity) Target.ItemQuantity = Target.ItemMaxQuantity;
            else if (Target.ItemQuantity <= 0) Target.ItemQuantity = 1;
        }
    }
}