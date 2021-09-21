using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultManager : MonoBehaviour {

    public Rigidbody bulletPrefab;
    public GameObject cursor;
    public Transform firePosition;
    public LayerMask layer;

    private Camera _cam;

    private void Start() {
        _cam = Camera.main;
    }

    private void Update() {
        LaunchProjectile();
    }

    void LaunchProjectile() {
        Ray _camRay = _cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit _hit;
        if (Physics.Raycast(_camRay, out _hit, 100f, layer)) {
            cursor.SetActive(true);
            cursor.transform.position = _hit.point + Vector3.up * 0.1f;

            Vector3 _Vo = CalculateVelocity(_hit.point, firePosition.position, 3f);

            //Vector3 _dir = _hit.point - firePosition.position;
            Quaternion _lookRotation = Quaternion.LookRotation(_Vo);

            firePosition.rotation = _lookRotation;

            transform.eulerAngles = new Vector3(0f, firePosition.eulerAngles.y, 0f);
            // new Quaternion(0f, _lookRotation.y, 0f, 0f);

            if (Input.GetKeyDown(KeyCode.G)) {
                Rigidbody _obj = Instantiate(bulletPrefab, firePosition.position, Quaternion.identity);
                _obj.velocity = _Vo;
            }
        } else {
            cursor.SetActive(false);
        }
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
}
