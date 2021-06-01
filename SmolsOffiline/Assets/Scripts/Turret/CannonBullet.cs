using System;
using UnityEngine;

public class CannonBullet : MonoBehaviour {

    public float speed = 70f;
    public int damage = 10;

    [SerializeField]
    private GameObject _impactEffect;
    private Transform _target;

    public void Seek(Transform _obj) {
        _target = _obj;
    }

    private void Update() {
        if (_target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 _dir = _target.position - transform.position;
        float _distanceThisFrame = speed * Time.deltaTime;

        if (_dir.magnitude <= _distanceThisFrame) {
            HitTarget();
            return;
        }

        transform.Translate(_dir.normalized * _distanceThisFrame, Space.World);
    }

    private void HitTarget() {
        _target.GetComponent<EnemyHealthManager>().RecieveDamage(damage);
        GameObject _particles = Instantiate(_impactEffect, transform.position, transform.rotation);
        Destroy(_particles, 1);
        gameObject.SetActive(false);
    }
}
