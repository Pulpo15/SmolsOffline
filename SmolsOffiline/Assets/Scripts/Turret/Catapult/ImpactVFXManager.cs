using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactVFXManager : MonoBehaviour, IPooledObject {
    public float _time = 5f;
    private float _curTime;
    
    public void OnObjectSpawn() {
        _curTime = _time;
    }

    private void Update() {
        _curTime -= Time.deltaTime;
        if (_curTime <= 0f) 
            gameObject.SetActive(false);
    }
}
