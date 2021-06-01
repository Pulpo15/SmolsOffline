using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour {

    public const int maxHeal = 100;
    private int _heal = 100;

    private EnemyAnimManager _enemyAnimManager;

    private void Start() {
        _enemyAnimManager = GetComponentInChildren<EnemyAnimManager>();
    }

    public int GetHeal() {
        return _heal;
    }
    public void SetHeal(int _value) {
        _heal = _value;
    }
    public void RecieveDamage(int _value) {
        if (_heal <= 0) {
            CheckStatus();
            return;
        }
        _heal -= _value;
        CheckStatus();
    }

    private void CheckStatus() {
        if (_heal <= 0) {
            _enemyAnimManager.DeathAnimation();
            WaveManager.instance._enemyList.Remove(gameObject);
        }
    }

}
