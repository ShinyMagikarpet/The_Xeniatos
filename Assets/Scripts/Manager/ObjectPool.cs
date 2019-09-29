using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool {

        public string name;
        public GameObject prefab;
        public int size;

    }

    public static ObjectPool Instance;

    private void Awake() {
        Instance = this;
    }

    public List<Pool> objectPools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    void Start() {

        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in objectPools) {

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++) {

                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDict.Add(pool.name, objectPool);
        }

    }

    public GameObject SpawnFromPool(string name) {

        if (!poolDict.ContainsKey(name)) {
            Debug.LogWarning("Pool with name " + name + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDict[name].Dequeue();

        objectToSpawn.SetActive(true);

        poolDict[name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
