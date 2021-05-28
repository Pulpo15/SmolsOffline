using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawnig : MonoBehaviour {

    [SerializeField]
    private string GroundLayerName = "Ground";
    [SerializeField]
    private GameObject _turretPrefab;

    private GameObject _turretGO;

    private void FixedUpdate() {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity)){
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            if (_turretGO == null)
                _turretGO = Instantiate(_turretPrefab, hit.point, Quaternion.identity);
            else
                _turretGO.transform.position = hit.point;
        } else {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100, Color.white);
            if (_turretGO != null)
                Destroy(_turretGO);
        }
    }

}
