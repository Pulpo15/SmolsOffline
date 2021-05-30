using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : Weapon {

    public float Range;

    public void Attack(GameObject _enemy) {
        base.Attack();
        Hit(_enemy);
    }
    private void Hit(GameObject _enemy) {
        Debug.Log("attack");
        _enemy.GetComponent<EnemyHealthManager>().RecieveDamage(damage);
    }
}
