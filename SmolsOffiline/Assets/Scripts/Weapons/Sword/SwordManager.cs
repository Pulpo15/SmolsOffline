using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : Weapon {

    public float Range;

    private GameObject _target;
    private bool _isAttacking = false;


    public void PrepareAttack(GameObject _enemy) {
        base.PrepareAttack();
        _target = _enemy;
    }

    public void MakeDamage() {
        _isAttacking = true;
        //if (_target != null)
        //    Hit();
    }

    public void EndMakeDamage() {
        _isAttacking = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy" && _isAttacking) {
            other.GetComponent<EnemyHealthManager>().RecieveDamage(damage);
        }
    }

    //private void Hit() {
    //    Debug.Log("attack");
    //    _target.GetComponent<EnemyHealthManager>().RecieveDamage(damage);
    //}
}
