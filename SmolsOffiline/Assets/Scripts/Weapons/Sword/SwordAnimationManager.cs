using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimationManager : MonoBehaviour {
    [SerializeField]
    private Animator _swordAnimator;

    private SwordManager _swordManager;

    private void Start() {
        _swordManager = gameObject.GetComponent<SwordManager>();
    }

    public void SwordAttack() {
        _swordAnimator.SetBool("Attack", true);
    }

    public void EndSwordAttack() {
        _swordAnimator.SetBool("Attack", false);
        _swordManager.SetAttack(false);
    }

}
