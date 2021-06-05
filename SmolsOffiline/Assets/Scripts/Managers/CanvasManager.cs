using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager instance;

    [SerializeField]
    private GameObject _storeCanvas;

    [SerializeField]
    private Text _moneyTag;
    private int _money;
    [SerializeField]
    private Text _waveTag;
    private WaveManager _waveManager;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("CanvasManager is a singleton, can't be instantiated more than 1 times");
        } else
            instance = this;
    }

    private void Start() {
        _waveManager = gameObject.GetComponent<WaveManager>();   
    }

    private void Update() {
        if (_storeCanvas.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Mouse1)) {
                _storeCanvas.SetActive(false);

                InGameMenuManager.instance.Resume();
            }
        }

        _waveTag.text = _waveManager.GetWaveIndex().ToString();
    }

    public bool GetStoreCanvasActive() {
        return _storeCanvas.activeSelf;
    }
    public void SetStoreCanvas(bool _value) {
        if (_storeCanvas.activeSelf != _value) {
            _storeCanvas.SetActive(_value);
        }
    }

    public int GetMoneyTag() {
        if (int.TryParse(_moneyTag.text, out _money)) {
            return _money;
        }
        Debug.LogError("Money isn't a number");
        return 0;
    }
    public void SetMoneyTag(int _money) {
        _moneyTag.text = _money.ToString();
    }

    public void StoreCanvasManager() {
        PlayerManager.instance.turretSpawnig.activatePreBuy = false;

        if (!_storeCanvas.activeSelf) {
            _storeCanvas.SetActive(true);

            InGameMenuManager.instance.Pause();

        } else if (_storeCanvas.activeSelf) {
            _storeCanvas.SetActive(false);

            InGameMenuManager.instance.Resume();

        }

    }

}
