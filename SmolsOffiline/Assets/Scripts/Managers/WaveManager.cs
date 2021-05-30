using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public static WaveManager instance;

    public Transform enemyPrefab;
    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float _countDown = 2f;

    public int enemiesPerRound = 2;

    [HideInInspector]
    public int _waveIndex = 0;

    private ObjectPooler _objectPooler;

    [HideInInspector]
    public List<GameObject> _enemyList;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("WaveManager is a singleton, can't be instantiated more than 1 times");
        } else
            instance = this;
    }

    private void Start() {
        _objectPooler = ObjectPooler.instance;
        _enemyList = new List<GameObject>();
    }

    private void Update() {
        if (_enemyList.Count <= 0) {
            StartCoroutine(SpawnWave());
        }

        //if (_countDown <= 0f) {
            
        //    _countDown = timeBetweenWaves;
        //}
        //_countDown -= Time.deltaTime;
    }

    private IEnumerator SpawnWave() {
        _waveIndex++;

        for (int i = 0; i < enemiesPerRound; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Wave " + _waveIndex);
    }

    private void SpawnEnemy() {
        GameObject _obj = _objectPooler.SpawnFromPool("Skeleton", spawnPoint.position, spawnPoint.rotation);
        _enemyList.Add(_obj);
    }

    public int GetWaveIndex() {
        return _waveIndex;
    }
}
