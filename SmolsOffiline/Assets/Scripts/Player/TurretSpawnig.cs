using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawnig : MonoBehaviour {

    [Header("Values")]
    [SerializeField]
    private float _rotationSensibility = 50f;
    [SerializeField]
    private string GroundLayerName = "Ground";

    [Header("Turrets Object References")]
    [SerializeField]
    private GameObject _spawneableTurretPrefab;
    [SerializeField]
    private GameObject _nonSpawneableTurretPrefab;
    [SerializeField]
    private GameObject[] _turretPrefabs;


    [HideInInspector]
    public bool activatePreBuy = false;

    public enum TurretType {
        Cannon,
        MoneyMultiplier
    }

    public TurretType turretType;

    private GameObject _spawneableTurretGO;
    private GameObject _nonSpawneableTurretGO;
    public GameObject[] _turretGO;
    private float _rot;
    private int _nonSpawneablelayerMask = 1 << 8;
    private int _spawneablelayerMask = 1 << 9;
    private ObjectPooler _objectPooler;

    private void Start() {
        //_nonSpawneableTurretGO = Instantiate(_nonSpawneableTurretPrefab, Vector3.zero, Quaternion.identity);
        //_spawneableTurretGO = Instantiate(_spawneableTurretPrefab, Vector3.zero, Quaternion.identity);

        _turretGO = new GameObject[_turretPrefabs.Length];

        for (int i = 0; i < _turretPrefabs.Length; i++) {
            _turretGO[i] = Instantiate(_turretPrefabs[i], Vector3.zero, Quaternion.identity);
            _turretGO[i].SetActive(false);
        }

        turretType = TurretType.Cannon;

        SwitchTurretType();

        _objectPooler = ObjectPooler.instance;
    }

    private void Update() {
        //Can't buy turrets during rounds
        if (WaveManager.instance.roundActive)
            return;
        //Enables the pre place to put turrets
        if (activatePreBuy) {
            PreBuyTurret();
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1))
                activatePreBuy = false;
        } else if (!activatePreBuy) {
            GameObjectManager(_nonSpawneableTurretGO, false);
            GameObjectManager(_spawneableTurretGO, false);
        }
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

                if (EconomyManager.instance.money >= 100) {
                    TurretObjectManager(_spawneableTurretGO, hit);
                    GameObjectManager(_nonSpawneableTurretGO, false);

                    if (Input.GetKeyDown(KeyCode.Mouse0)) {
                        BuyTurret(_spawneableTurretGO.transform, Quaternion.Euler(0, _rot, 0));
                        EconomyManager.instance.AddMoney(-100);
                    }
                } else {
                    TurretObjectManager(_nonSpawneableTurretGO, hit);
                    GameObjectManager(_spawneableTurretGO, false);
                }


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

    public void SwitchTurretType() {
        switch (turretType) {
            case TurretType.Cannon:
            _spawneableTurretGO = _turretGO[0];
            _nonSpawneableTurretGO = _turretGO[1];
            break;
            case TurretType.MoneyMultiplier:
            _spawneableTurretGO = _turretGO[2];
            _nonSpawneableTurretGO = _turretGO[3];
            break;
            default:
            break;
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
        if (_go == null)
            return;
        if (_go.activeSelf != _state)
            _go.SetActive(_state);
    }

    private void BuyTurret(Transform _position, Quaternion _rotation) {
        switch (turretType) {
            case TurretType.Cannon:
            _objectPooler.SpawnFromPool("CannonTurret", _position.position, _rotation);
            break;
            case TurretType.MoneyMultiplier:
            _objectPooler.SpawnFromPool("MoneyMultiplierTurret", _position.position, _rotation);
            EconomyManager.instance.moneyMultiplier += 1;
            break;
            default:
            break;
        }
    }
}
