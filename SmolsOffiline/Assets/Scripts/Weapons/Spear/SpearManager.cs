using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearManager : Weapon {
    public Rigidbody rb;

    private float _attackTimer = 0.25f;
    private float _curAttackTimer;
    private bool _timeCompleted;
    private bool _secondaryAttack = false;

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

        if (Input.GetKey(KeyCode.Mouse1)){
            this.PrepareSecondaryAttack();
            if (_curAttackTimer <= 0) {
                _timeCompleted = true;
            }
            _curAttackTimer -= Time.deltaTime;
        } else if (Input.GetKeyUp(KeyCode.Mouse1)) {
            if (_timeCompleted) {
                SecondaryAttack();
                _timeCompleted = false;
                _curAttackTimer = _attackTimer;
            } else if (!_timeCompleted) {
                CancelPrepareSecondaryAttack();
                _curAttackTimer = _attackTimer;
            }
        }
         if (_secondaryAttack)
            transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    public void SecondaryAttack() {
        base.SecondaryAttack();
        gameObject.transform.parent = null;
        _animator.enabled = false;
        rb.useGravity = true;
        _secondaryAttack = true;
        rb.velocity = transform.forward * 25f;
        transform.rotation = Quaternion.LookRotation(rb.velocity);
    }
}
