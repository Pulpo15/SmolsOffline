using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour, IPooledObject {
    public float speed = 2f;

    private Transform _target;
    private int _wayPointIndex;

    public void OnObjectSpawn() {
        _target = Waypoints.waypoints[0];
    }

    private void Update() {
        Quaternion _aimPosition = Quaternion.LookRotation(_target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, _aimPosition, 5f * Time.deltaTime);

        Vector3 dir = _target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, _target.position) <= 0.2f) {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint() {
        if (_wayPointIndex >= Waypoints.waypoints.Length - 1)
            return;
        _wayPointIndex++;
        _target = Waypoints.waypoints[_wayPointIndex];
    }
}
