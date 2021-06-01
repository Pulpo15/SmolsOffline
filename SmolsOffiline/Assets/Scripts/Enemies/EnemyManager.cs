using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IPooledObject {
    public float speed = 2f;

    [HideInInspector]
    public bool spawn = false;
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public bool isWalking = false;

    private Transform _target;
    private int _wayPointIndex;
    private EnemyAnimManager _enemyAnimManager;

    private void Awake() {

    }

    public void OnObjectSpawn() {
        transform.rotation = Quaternion.LookRotation(Waypoints.waypoints[0].position - transform.position);
        spawn = true;
    }

    public void SetFirstTarget() {
        _target = Waypoints.waypoints[0];
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
        transform.rotation = Quaternion.Lerp(transform.rotation, _aimPosition, 5f * Time.deltaTime);

        Vector3 dir = _target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, _target.position) <= 0.2f) {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint() {
        if (_wayPointIndex >= Waypoints.waypoints.Length - 1) {
            isWalking = false;
            return;
        }
        isWalking = true;
        _wayPointIndex++;
        _target = Waypoints.waypoints[_wayPointIndex];
    }
}
