using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour {
    public Camera cam;
    public Transform LHFirePoint, RHFirePoint;
    public float projectileSpeed;
    public float fireRate = 4;
    public float archRange = 1;

    private Vector3 _destination;
    private ObjectPooler _objectPooler;
    private bool _leftHand = false;
    private float _timeToFire;

    private void Start() {
        _objectPooler = ObjectPooler.instance;
    }

    private void Update() {
        if (PlayerManager.instance.turretSpawnig.activatePreBuy || CanvasManager.instance.GetStoreCanvasActive())
            return;
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= _timeToFire) {
            _timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }
    }

    public void ShootProjectile() {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit)) {
            _destination = hit.point;
        } else {
            _destination = ray.GetPoint(100000);
        }
        if (_leftHand) {
            _leftHand = false;
            SpawnProjectileFromPool(LHFirePoint);
        } else {
            _leftHand = true;
            SpawnProjectileFromPool(RHFirePoint);
        }
    }

    private void SpawnProjectileFromPool(Transform _firePoint) {
        GameObject projectileObj = _objectPooler.SpawnFromPool("DamageProjectile", _firePoint.position, Quaternion.identity);
        projectileObj.GetComponent<Rigidbody>().velocity = (_destination - _firePoint.position).normalized * projectileSpeed;

        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(-archRange, archRange), Random.Range(-archRange, archRange), 0), Random.Range(0.5f, 2));
    }
}
