using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawneableGroundManager : MonoBehaviour {

    private bool turretCanSpawn;

    private void Start() {
        turretCanSpawn = true;
    }

    public void SetTurretCanSpawn(bool _value) {
        turretCanSpawn = _value;
    }
    public bool GetTurretCanSpawn() {
        return turretCanSpawn;
    }
}
