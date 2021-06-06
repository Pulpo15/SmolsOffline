using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearManager : Weapon {
    public Rigidbody rb;

    [SerializeField]
    private Camera cam;
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
        if (_secondaryAttack && rb.velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

     void SecondaryAttack() {
        base.SecondaryAttack();

        _animator.enabled = false;
        _secondaryAttack = true;

        transform.parent = null;
        rb.isKinematic = false;

        rb.AddForce(cam.transform.forward * 80f, ForceMode.Impulse);

        if (rb.velocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy" && _isAttacking) {
            other.GetComponent<EnemyHealthManager>().RecieveDamage(damage);
        }
        Stick();
    }

    private void Stick() {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
