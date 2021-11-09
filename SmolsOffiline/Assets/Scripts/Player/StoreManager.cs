using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour {
    public void BuyTurret1() {
        PlayerManager.instance.turretSpawnig.activatePreBuy = true;
        PlayerManager.instance.turretSpawnig.turretType = TurretSpawnig.TurretType.Cannon;
        PlayerManager.instance.turretSpawnig.SwitchTurretType();
        CanvasManager.instance.SetStoreCanvas(false);
        InGameMenuManager.instance.Resume();
    }
    public void BuyTurret2() {
        PlayerManager.instance.turretSpawnig.activatePreBuy = true;
        PlayerManager.instance.turretSpawnig.turretType = TurretSpawnig.TurretType.MoneyMultiplier;
        PlayerManager.instance.turretSpawnig.SwitchTurretType();
        CanvasManager.instance.SetStoreCanvas(false);
        InGameMenuManager.instance.Resume();
    }

    public void BuyTurret3() {
        PlayerManager.instance.turretSpawnig.activatePreBuy = true;
        PlayerManager.instance.turretSpawnig.turretType = TurretSpawnig.TurretType.Catapult;
        PlayerManager.instance.turretSpawnig.SwitchTurretType();
        CanvasManager.instance.SetStoreCanvas(false);
        InGameMenuManager.instance.Resume();
    }
}
