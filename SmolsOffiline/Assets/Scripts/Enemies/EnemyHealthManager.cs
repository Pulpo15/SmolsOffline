using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    private int _heal = 100;

    public int GetHeal() {
        return _heal;
    }
    public void SetHeal(int _value) {
        _heal = _value;
    }
    public void RecieveDamage(int _value) {
        if (_heal <= 0) {
            return;
        }
        _heal -= _value;
        CheckStatus();
    }

    private void CheckStatus() {
        if (_heal <= 0) {
            gameObject.SetActive(false);
            WaveManager.instance._enemyList.Remove(gameObject);
        }
    }

}
