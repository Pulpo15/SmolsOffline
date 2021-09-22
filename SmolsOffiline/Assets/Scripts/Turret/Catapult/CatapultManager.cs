using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManager : MonoBehaviour {
    
    [Header("Values")]
    public float range = 15f;
    public float timeToImpact;

    [Header("References")]
    public Rigidbody bulletPrefab;
    public GameObject impactZone;
    public Transform firePosition;
    public LayerMask layer;

    private Camera _cam;
    private Transform _target;
    private float _Noffset = -5f;
    private float _Poffset = 5f;

    private void Start() {
        _cam = Camera.main;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update() {
        LaunchProjectile();
    }

    void LaunchProjectile() {
        if (_target == null) {
            impactZone.SetActive(false);
            return;
        }

        impactZone.SetActive(true);
        _target.GetComponent<EnemyManager>().catapultPoint.gameObject.SetActive(true);

        Vector3 _newTarget = _target.GetComponent<EnemyManager>().catapultPoint.position;

        //Vector3 _newTarget = new Vector3();

        //if (_target.eulerAngles.y <= 180f || _target.eulerAngles.y >= 90.01f)
        //    _newTarget = _target.position + Vector3.forward * (_Noffset * timeToImpact);
        //else if (_target.eulerAngles.y <= 90f || _target.eulerAngles.y >= 0.01f)
        //    Debug.Log("asd");
        //_newTarget = _target.position + Vector3.left * (_Noffset * timeToImpact);

        impactZone.transform.position = _newTarget + Vector3.up * 0.1f;

        Vector3 _Vo = CalculateVelocity(_newTarget, firePosition.position, timeToImpact);

        Vector3 _dir = _newTarget - transform.position;
        Quaternion _lookRotation = Quaternion.LookRotation(_dir);
        Vector3 _rotation = Quaternion.Lerp(transform.rotation, _lookRotation, 2f * Time.deltaTime).eulerAngles;
        firePosition.rotation = Quaternion.Euler(0f, _rotation.y, 0f);
        transform.rotation = Quaternion.Euler(0f, _rotation.y, 0f);

        if (Input.GetKeyDown(KeyCode.G)) {
            Rigidbody _obj = Instantiate(bulletPrefab, firePosition.position, Quaternion.identity);
            _obj.velocity = _Vo;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    Vector3 CalculateVelocity(Vector3 _target, Vector3 _origin, float _time) {
        //Define the distance x and y first
        Vector3 _distance = _target - _origin;
        Vector3 _distanceXZ = _distance;
        _distanceXZ.Normalize();
        _distanceXZ.y = 0f;

        //Create a float that represents our distance
        float _Sy = _distance.y;
        float _Sxz = _distance.magnitude;

        float _Vxz = _Sxz / _time;
        float _Vy = _Sy / _time + 0.5f * Mathf.Abs(Physics.gravity.y) * _time;

        Vector3 _result = _distanceXZ.normalized;
        _result *= _Vxz;
        _result.y = _Vy;

        return _result;
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
            if (_target != null)
                _target.GetComponent<EnemyManager>().catapultPoint.gameObject.SetActive(false);
            _target = _nearestEnemy.transform;
        } else {
            if (_target != null)
                _target.GetComponent<EnemyManager>().catapultPoint.gameObject.SetActive(false);
            _target = null;
        }
    }
}
