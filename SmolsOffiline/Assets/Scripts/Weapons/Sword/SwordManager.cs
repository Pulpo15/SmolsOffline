using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : Weapon {

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            this.PrepareAttack();
        } 
    }
}
