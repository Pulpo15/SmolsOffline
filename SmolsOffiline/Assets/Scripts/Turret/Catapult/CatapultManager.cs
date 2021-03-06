using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManager : MonoBehaviour, IPooledObject {
    
    [Header("Values")]
    public float range = 15f;
    public float timeToImpact;
    public float fireRate = 5f;

    [Header("References")]
    public Rigidbody bulletPrefab;
    public GameObject impactZone;
    public Transform firePosition;
    public LayerMask layer;

    private Transform _target;
    private ObjectPooler _objectPooler;
    private float _curFireRate = 0f;
    private bool _canShoot = false;
    private Animator _animator;
    //private float _Noffset = -5f;
    //private float _Poffset = 5f;

    public void OnObjectSpawn() {
        _curFireRate = fireRate;
        _canShoot = false;
    }

    private void Start() {
        _objectPooler = ObjectPooler.instance;
        _animator = gameObject.GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update() {
        LaunchProjectile();
    }

    void LaunchProjectile() {
        if (_target == null) {
            //impactZone.SetActive(false);
            return;
        }

        //impactZone.SetActive(true);
        _target.GetComponent<EnemyManager>().catapultPoint.gameObject.SetActive(true);

        Vector3 _newTarget = _target.GetComponent<EnemyManager>().catapultPoint.position;

        //Catapult Rotation
        Vector3 _Vo = CalculateVelocity(_newTarget, firePosition.position, timeToImpact);

        Vector3 _dir = _newTarget - transform.position;
        Quaternion _lookRotation = Quaternion.LookRotation(_dir);
        Vector3 _rotation = Quaternion.Lerp(transform.rotation, _lookRotation, 2f * Time.deltaTime).eulerAngles;
        firePosition.rotation = Quaternion.Euler(0f, _rotation.y, 0f);
        transform.rotation = Quaternion.Euler(0f, _rotation.y, 0f);

        //Shoot
        if (_curFireRate <= 0f) {
            _animator.SetBool("Shoot", true);
            _curFireRate = fireRate;
        }
        if (_canShoot) {
            Shoot(_Vo);
            _canShoot = false;
        }
        _curFireRate -= Time.deltaTime;
    }

    public void CanShoot() {
        _canShoot = true;
    }

    public void ResetShoot() {
        _animator.SetBool("Shoot", false);
    }


    private void Shoot(Vector3 _Vo) {
        GameObject _obj = _objectPooler.SpawnFromPool("CatapultBullet", firePosition.position, Quaternion.identity);
        Rigidbody _rb = _obj.GetComponent<Rigidbody>();
        _rb.AddTorque(0f, 0f, -100f);
        _rb.velocity = _Vo;
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
