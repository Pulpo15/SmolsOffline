using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : Weapon {

    private float _attackTimer = 0.25f;
    private float _curAttackTimer;
    private bool _timeCompleted;

    private void Start() {
        _curAttackTimer = _attackTimer;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            this.PrepareAttack();
            if (_curAttackTimer <= 0) {
                _timeCompleted = true;
            }
            _curAttackTimer -= Time.deltaTime;
        } else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            if (_timeCompleted) {
                Attack();
                _timeCompleted = false;
                _curAttackTimer = _attackTimer;
            } else if (!_timeCompleted) {
                CancelPrepareAttack();
                _curAttackTimer = _attackTimer;
            }
        }
    }


}
