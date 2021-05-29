using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour {

    public int Damage;

    private bool _attack;

    public bool GetAttack() {
        return _attack;
    }
    public void SetAttack(bool _value) {
        if (_attack != _value)
            _attack = _value;
    }

    public void SwordHit() {
        _attack = true;
    }

    private void Update() {
        RaycastHit hit;

    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Enemy" && _attack) {
            Debug.Log("Attack");
            other.gameObject.GetComponent<EnemyHealthManager>().RecieveDamage(Damage);
            _attack = false;
        }
    }
}
