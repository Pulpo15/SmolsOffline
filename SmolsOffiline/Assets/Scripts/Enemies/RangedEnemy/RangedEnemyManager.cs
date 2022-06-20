using UnityEngine;
using System;

public class RangedEnemyManager : MonoBehaviour {

    [Header("Values")]
    public float timeToImpact;
    public float fireRate = 5f;

    [Header("References")]
    public Rigidbody bulletPrefab;
    public Transform firePosition;
    public LayerMask layer;

    private int _triggerNum = 0;
    private bool _canShoot = false;
    private bool _walk = false;
    private float _curFireRate = 0f;

    private Transform _target;
    private ObjectPooler _objectPooler;

    private void Start() {
        _target = PlayerManager.instance.transform;
        _objectPooler = ObjectPooler.instance;
    }

    private void Update() {
            LaunchProjectile();
    }

    private void LaunchProjectile() {

        Vector3 newTarget = new Vector3(_target.position.x, _target.position.y - 1f, _target.position.z);

        Vector3 _Vo = CalculateVelocity(newTarget, firePosition.position, timeToImpact);

        Vector3 _dir = newTarget - transform.position;
        Quaternion _lookRotation = Quaternion.LookRotation(_dir);
        Vector3 _rotation = Quaternion.Lerp(transform.rotation, _lookRotation, 2f * Time.deltaTime).eulerAngles;
        firePosition.rotation = Quaternion.Euler(0f, _rotation.y, 0f);
        transform.rotation = Quaternion.Euler(0f, _rotation.y, 0f);

        //Shoot
        if(_curFireRate <= 0f && _canShoot) {
            Shoot(_Vo);
            _curFireRate = fireRate;
        }
        _curFireRate -= Time.deltaTime;
    }

    private void Shoot(Vector3 _Vo) {
        GameObject _obj = _objectPooler.SpawnFromPool("RangedEnemyBullet", firePosition.position, Quaternion.identity);
        Rigidbody _rb = _obj.GetComponent<Rigidbody>();
        _rb.AddTorque(0f, 0f, -100f);
        _rb.velocity = _Vo;
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

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            _triggerNum++;
            if (_triggerNum >= 2) {
                _canShoot = true;
                _walk = false;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            _triggerNum--;
            if(_triggerNum == 0) {
                _canShoot = false;
                _walk = true;
            }
        }
    }
}
