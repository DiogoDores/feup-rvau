using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPooler Instance;
    
    private void Awake() {
        Instance = this;
    }
    #endregion Singleton
    
    private void Start() {
        this.poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in this.pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            this.poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {
        if (!this.poolDictionary.ContainsKey(tag)) {
            //Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject obj = this.poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        this.poolDictionary[tag].Enqueue(obj);

        return obj;
    }

    public int GetActiveObjectsInPool() {
        int actives = 0;

        foreach (Pool pool in this.pools) {
            foreach (GameObject obj in this.poolDictionary[pool.tag]) {
                if (obj.activeSelf) actives++;
            }
        }

        return actives;
    }
}
