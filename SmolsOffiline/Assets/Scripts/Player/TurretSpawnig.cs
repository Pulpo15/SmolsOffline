using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawnig : MonoBehaviour {

    [SerializeField]
    private float _rotationSensibility = 50f;
    [SerializeField]
    private string GroundLayerName = "Ground";
    [SerializeField]
    private GameObject _spawneableTurretPrefab;
    [SerializeField]
    private GameObject _nonSpawneableTurretPrefab;
    [SerializeField]
    private GameObject _turretPrefab;

    private GameObject _spawneableTurretGO;
    private GameObject _nonSpawneableTurretGO;
    //private GameObject _turretGO;
    private float _rot;
    private int _nonSpawneablelayerMask = 1 << 8;
    private int _spawneablelayerMask = 1 << 9;

    private void Start() {
        _nonSpawneableTurretGO = Instantiate(_nonSpawneableTurretPrefab, Vector3.zero, Quaternion.identity);
        _spawneableTurretGO = Instantiate(_spawneableTurretPrefab, Vector3.zero, Quaternion.identity);
    }

    private void Update() {
        PreBuyTurret();
    }

    public void PreBuyTurret() {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)) {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            if (Input.GetKey(KeyCode.Q)) {
                _rot += _rotationSensibility * Time.fixedDeltaTime;
            } else if (Input.GetKey(KeyCode.E)) {
                _rot -= _rotationSensibility * Time.fixedDeltaTime;
            }

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                out hit, Mathf.Infinity, _spawneablelayerMask)) {

                TurretObjectManager(_spawneableTurretGO, hit);
                GameObjectManager(_nonSpawneableTurretGO, false);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                    BuyTurret(_spawneableTurretGO.transform, Quaternion.Euler(0, _rot, 0));

            } else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                  out hit, Mathf.Infinity, _nonSpawneablelayerMask)) {

                TurretObjectManager(_nonSpawneableTurretGO, hit);
                GameObjectManager(_spawneableTurretGO, false);

            }

        } else {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.white);
            GameObjectManager(_nonSpawneableTurretGO, false);
            GameObjectManager(_spawneableTurretGO, false);
        }
    }

    private void TurretObjectManager(GameObject _go, RaycastHit _hit) {
        if (!_go.activeSelf)
            _go.SetActive(true);
        else {
            _go.transform.position = _hit.point;
            _go.transform.rotation = Quaternion.Euler(0, _rot, 0);
        }
    }

    private void GameObjectManager(GameObject _go, bool _state) {
        if (_go.activeSelf != _state)
            _go.SetActive(_state);
    }

    private void BuyTurret(Transform _position, Quaternion _rotation) {
        Instantiate(_turretPrefab, _position.position, _rotation);
    }
}
