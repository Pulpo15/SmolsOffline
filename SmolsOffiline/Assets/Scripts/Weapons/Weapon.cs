using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public int damage;
    public bool canAttack = true;

    [SerializeField]
    private Animator _animator;

    //Function to activate the animation
    public void Attack() {
        _animator.SetBool("Attack", true);
        canAttack = false;
    }
    //Function to end the animation
    public void EndAttack() {
        _animator.SetBool("Attack", false);
        canAttack = true;
    }


}
