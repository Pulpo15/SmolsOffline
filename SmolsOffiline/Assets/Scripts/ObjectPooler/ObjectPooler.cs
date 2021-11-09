using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler instance;

    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public GameObject parent;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("ObjectPooler is a singleton, can't be instantiated more than 1 times");
        } else
            instance = this;
    }

    private void Start() {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool _pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            _pool.parent = new GameObject();
            _pool.parent.name = _pool.tag;
            _pool.parent.gameObject.transform.parent = gameObject.transform;
            for (int i = 0; i < _pool.size; i++) {
                GameObject obj = Instantiate(_pool.prefab, _pool.parent.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(_pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string _tag, Vector3 _position, Quaternion _rotation) {
        if (!poolDictionary.ContainsKey(_tag)) {
            Debug.LogWarning("Pool with tag " + _tag + " doesn't exist.");
            return null;
        }

        //Get Object from the pool and move it to the position you selected
        GameObject _objToSpawn = poolDictionary[_tag].Dequeue();

        _objToSpawn.SetActive(true);
        _objToSpawn.transform.position = _position;
        _objToSpawn.transform.rotation = _rotation;

        //Call own start method using Interface
        IPooledObject _pooledObj = _objToSpawn.GetComponent<IPooledObject>();

        if (_pooledObj != null)
            _pooledObj.OnObjectSpawn();

        //Put back Object in dictionary so it can be used again
        poolDictionary[_tag].Enqueue(_objToSpawn);

        return _objToSpawn;
    }

}
