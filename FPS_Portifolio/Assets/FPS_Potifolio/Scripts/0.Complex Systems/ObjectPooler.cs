using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region - Singleton Pattern
    public static ObjectPooler Instance;
    private void Awake() => Instance = this;
    #endregion

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictonary;

    private void Start()
    {
        poolDictonary = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictonary.Add(pool.tag, objectPool);
        }
    }
    public GameObject SpawnFromPool(string poolTag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictonary.ContainsKey(poolTag))
        {
            Debug.LogWarning("Pool with tag " + poolTag + "Doesn't exist;"); return null;
        }

        GameObject objectToSpawn =  poolDictonary[poolTag].Dequeue();
        objectToSpawn.SetActive(true); objectToSpawn.transform.position = position; objectToSpawn.transform.rotation = rotation;

        poolDictonary[poolTag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}