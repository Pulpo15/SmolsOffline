using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimManager : MonoBehaviour {

    public Animator _animator;
    private EnemyManager _enemyManager;
    private EnemyHealthManager _enemyHealthManager;

    private void Start() {
        _enemyManager = gameObject.GetComponentInParent<EnemyManager>();
        _enemyHealthManager = gameObject.GetComponentInParent<EnemyHealthManager>();
    }

    private void Update() {

        if (_animator == null) {
            Debug.LogError("Animator requiered in  " + this);
            return;
        }

        if (_enemyManager.spawn) {
            _animator.SetBool("Spawn", true);
        }

        if (_enemyManager.isWalking) {
            _animator.SetBool("Run", true);
        } else if (!_enemyManager.isWalking) {
            _animator.SetBool("Run", false);
        }
    }

    public void EndSpawnAnimation() {
        _enemyManager.spawn = false;
        _animator.SetBool("Spawn", false);
        _enemyManager.SetFirstTarget();
    }

    public void DeathAnimation() {
        _animator.SetBool("Die", true);
        _enemyManager.dead = true;
    }

    public void EndDeathAnimation() {
        StartCoroutine(UnableEnemy());
    }

    IEnumerator UnableEnemy() {
        yield return new WaitForSeconds(2f);
        transform.parent.gameObject.SetActive(false);
        _animator.SetBool("Die", false);
        _enemyManager.dead = false;
    }

}
