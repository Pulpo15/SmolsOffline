using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour {
    public void BuyTurret1() {
        PlayerManager.instance.turretSpawnig.activatePreBuy = true;
        CanvasManager.instance.SetStoreCanvas(false);
        InGameMenuManager.instance.Resume();
    }
}
