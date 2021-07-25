using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour {

    public float range = 15f;
    public Transform partToRotate;

    private Transform _target;
    private float _speedRotation = 5f;

    private void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget() {
        List<GameObject> _enemies = WaveManager.instance._enemyList;
        float _shortestsDistance = Mathf.Infinity;
        GameObject _nearestEnemy = null;

        foreach (GameObject _enemy in _enemies) {
            float _distanceToEnemy = Vector3.Distance(transform.position, _enemy.transform.position);
            if (_distanceToEnemy < _shortestsDistance) {
                _shortestsDistance = _distanceToEnemy;
                _nearestEnemy = _enemy;
            }
        }

        if (_nearestEnemy != null && _shortestsDistance <= range) {
            _target = _nearestEnemy.transform;
        } else {
            _target = null;
        }
    }

    private void Update() {
        if (_target == null)
            return;

        Vector3 _dir = _target.position - transform.position;
        Quaternion _lookRotation = Quaternion.LookRotation(_dir);
        Vector3 _rotation = Quaternion.Lerp(partToRotate.rotation, _lookRotation, _speedRotation * Time.deltaTime).eulerAngles;

        partToRotate.rotation = Quaternion.Euler(0f, _rotation.y, 0f);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public Transform GetTarget() {
        return _target;
    }
}
