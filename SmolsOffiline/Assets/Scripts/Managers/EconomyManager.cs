using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour {
    public static EconomyManager instance;

    public int money = 0;
    public int moneyMultiplier = 1;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("GameManager is a singleton, can't be instantiated more than 1 times");
        } else
            instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return))
            AddMoney(100);
        if (Input.GetKeyDown(KeyCode.Delete))
            AddMoney(-100);
    }

    public void AddMoney(int _money) {
        money += _money;
        CanvasManager.instance.SetMoneyTag(money);
    }

}
