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
    public bool isWalking = false;

    private Transform _target;
    private int _wayPointIndex;
    private int _wayPointIndexParent;
    private EnemyAnimManager _enemyAnimManager;
    private float _speedRotation = 5f;

    private void Awake() {

    }

    public void OnObjectSpawn() {
        transform.rotation = Quaternion.LookRotation(Waypoints.waypoints[0,0].position - transform.position);
        _wayPointIndexParent = WaveManager.instance._spawnIndex;
        spawn = true;
        _target = null;
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
        if (_wayPointIndex >= Waypoints.waypoints.Length / totalPaths - 1) {
            isWalking = false;
            return;
        }
        isWalking = true;
        _wayPointIndex++;
        _target = Waypoints.waypoints[_wayPointIndexParent, _wayPointIndex];
    }
}
