using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public int damage;
    [HideInInspector]
    public bool canAttack = true;

    [SerializeField]
    private Animator _animator;

    protected bool _isAttacking = false;

    //Function to activate the animation
    public void PrepareAttack() {
        if (canAttack) {
            _animator.SetBool("PrepareAttack", true);
            canAttack = false;
        }
    }
    public void Attack() {
        _animator.SetBool("PrepareAttack", false);
        _animator.SetBool("Attack", true);
        
    }
    //Function to end the animation
    public void EndAttack() {
        _animator.SetBool("Attack", false);
        _animator.SetBool("PrepareAttack", false);
    }
    public void SetUpIdle() {
        canAttack = true;
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
}
