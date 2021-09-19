using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaManager : MonoBehaviour {
    public GameObject LightingPrefab;
    public Transform LightingSpawn;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            Instantiate(LightingPrefab, LightingSpawn.position, Quaternion.identity);
        }
    }
}
