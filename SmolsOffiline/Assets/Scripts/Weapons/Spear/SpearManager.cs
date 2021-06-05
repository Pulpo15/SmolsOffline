using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearManager : Weapon {
    private void Update() {
        if (Input.GetKey(KeyCode.Mouse0)) {
            this.PrepareAttack();
        } else if (Input.GetKeyUp(KeyCode.Mouse0)) {
            Attack();
        }
    }
}
