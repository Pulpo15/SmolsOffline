using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public int damage;
    [HideInInspector]
    public bool canAttack = true;

    [SerializeField]
    private Animator _animator;

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



}
