using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {

    [SerializeField]
    private string dontDrawLayerName = "DontDraw";
    [SerializeField]
    private string DrawLayerName = "Player";
    [SerializeField]
    private GameObject playerGraphics;

    private void Start() {
        SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
    }

    private void SetLayerRecursively(GameObject _obj, int _newLayer) {
        _obj.layer = _newLayer;

        foreach (Transform _child in _obj.transform) {
            SetLayerRecursively(_child.gameObject, _newLayer);
        }
    }
}
