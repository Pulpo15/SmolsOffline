using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : Weapon {

    private void Update() {
        if (PlayerManager.instance.turretSpawnig.activatePreBuy || CanvasManager.instance.GetStoreCanvasActive())
            return;
        if (Input.GetKey(KeyCode.Mouse0)) {
            this.PrepareAttack();
        } 
    }
}
