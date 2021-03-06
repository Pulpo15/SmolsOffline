using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    public static WaveManager instance;

    public Transform enemyPrefab;
    public Transform[] spawnPoint;
    public int wayPointPath = 0;
    public int _spawnIndex = 0;

    public float timeBetweenWaves = 5f;
    private float _countDown = 2f;

    public int enemiesPerRound = 2;

    public bool roundActive = false;

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
        if (Input.GetKeyDown(KeyCode.R) && !roundActive) {
            StartCoroutine(SpawnWave());
        }

        if (_enemyList.Count <= 0 && roundActive) {
            StartCoroutine(NextRound());
        }
    }

    private IEnumerator SpawnWave() {
        if (!roundActive)
            roundActive = true;

        _waveIndex++;

        for (int i = 0; i < enemiesPerRound; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator NextRound() {
        yield return new WaitForSeconds(2f);
        roundActive = false;

    }

    private void SpawnEnemy() {
        GameObject _obj = _objectPooler.SpawnFromPool("Skeleton", spawnPoint[_spawnIndex].position, spawnPoint[_spawnIndex].rotation);
        _enemyList.Add(_obj);
        _spawnIndex++;
        if (_spawnIndex >= spawnPoint.Length)
            _spawnIndex = 0;
    }

    public int GetWaveIndex() {
        return _waveIndex;
    }
}
