using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IPooledObject {
    public float speed = 2f;
    public int totalPaths = 3;

    [HideInInspector]
    public bool spawn = false;
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public bool attack = false;
    [HideInInspector]
    public bool isWalking = false;
    [HideInInspector]
    public bool isOnObjective;

    private Transform _target;
    private int _wayPointIndex;
    private int _wayPointIndexParent;
    private EnemyAnimManager _enemyAnimManager;
    private EnemyHealthManager _enemyHealthManager;
    private float _speedRotation = 5f;

    private void Awake() {
        _enemyHealthManager = gameObject.GetComponent<EnemyHealthManager>();
        _enemyAnimManager = gameObject.GetComponentInChildren<EnemyAnimManager>();
    }

    public void OnObjectSpawn() {
        transform.rotation = Quaternion.LookRotation(Waypoints.waypoints[0,0].position - transform.position);
        _wayPointIndexParent = WaveManager.instance._spawnIndex;
        _wayPointIndex = 0;
        _enemyHealthManager.SetUp();
        SetFirstTarget();
        spawn = true;
        _target = null;
        isOnObjective = false;
    }

    public void SetFirstTarget() {
        _target = Waypoints.waypoints[_wayPointIndexParent, 0];
        isWalking = true;
    }

    private void Update() {
        if (spawn)
            return;
        if (dead)
            return;
        if (isOnObjective && !attack) {
            _enemyAnimManager.AttackAnimation();
        }
        Debug.Log(attack);
        Debug.Log(isOnObjective);
        if (!isWalking)
            return;
        Quaternion _aimPosition = Quaternion.LookRotation(_target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, _aimPosition, _speedRotation * Time.deltaTime);

        Vector3 dir = _target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, _target.position) <= 0.2f) {
            GetNextWaypoint();
        }

    }

    private void GetNextWaypoint() {
        if (_wayPointIndex == Waypoints.waypoints.Length / totalPaths - 1) {
            isOnObjective = true;
        }

        if (_wayPointIndex >= Waypoints.waypoints.Length / totalPaths - 1) {
            isWalking = false;
            return;
        }
        isWalking = true;
        _wayPointIndex++;
        _target = Waypoints.waypoints[_wayPointIndexParent, _wayPointIndex];
    }
}
