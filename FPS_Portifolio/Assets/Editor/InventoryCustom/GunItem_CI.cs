using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GunItem))]
public class GunItem_CI : Editor
{
    GunItem Target;

    public override void OnInspectorGUI()
    {
        Target = (GunItem)target;

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

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Hanbale Animator", GUILayout.MaxWidth(102f), GUILayout.MaxHeight(25f));
        Target.handableAnim = (Animator)EditorGUILayout.ObjectField(Target.handableAnim, typeof(Animator), GUILayout.MaxWidth(130f), GUILayout.MaxHeight(25f));
        EditorGUILayout.LabelField("Model Animator", GUILayout.MaxWidth(90f), GUILayout.MaxHeight(25f));
        Target.modelAnimator = (Animator)EditorGUILayout.ObjectField(Target.modelAnimator, typeof(Animator), GUILayout.MaxWidth(130f), GUILayout.MaxHeight(25f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("On Hands", GUILayout.MaxWidth(60f), GUILayout.MaxHeight(25f));
        Target.InHands = (bool)EditorGUILayout.Toggle(Target.InHands, GUILayout.MaxWidth(25f), GUILayout.MaxHeight(25f));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}