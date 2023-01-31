using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDestroyer : MonoBehaviour
{
    public bool DestroyOnStart = true;
    public float DestroyTime = 2f;
    private void Start() => Destroy(gameObject, DestroyTime);
}