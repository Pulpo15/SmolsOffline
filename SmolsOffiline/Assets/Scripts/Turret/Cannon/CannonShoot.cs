using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShoot : MonoBehaviour {
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float fireRate = 1f;
    private float _fireCountdown = 0f;
    private TurretManager _turretManager;
    private ObjectPooler _objectPooler;

    private void Start() {
        _turretManager = gameObject.GetComponent<TurretManager>();
        _objectPooler = ObjectPooler.instance;
    }

    private void Update() {
        if (_turretManager.GetTarget() == null)
            return;
        if (_fireCountdown <= 0f) {
            Shoot();
            _fireCountdown = 1f / fireRate;
        }

        _fireCountdown -= Time.deltaTime;
    }

    void Shoot() {
        GameObject _bulletGO = _objectPooler.SpawnFromPool("CannonBullet", firePoint.position, firePoint.rotation);
        CannonBullet _bullet = _bulletGO.GetComponent<CannonBullet>();
        if (_bullet != null)
            _bullet.Seek(_turretManager.GetTarget());
    }
}
