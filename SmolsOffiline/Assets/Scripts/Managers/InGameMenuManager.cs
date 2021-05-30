using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuManager : MonoBehaviour {

    public static InGameMenuManager instance;
    public bool GameIsPaused = false;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("InGameMenuManager is a singleton, can't be instantiated more than 1 times");
        } else
            instance = this;
    }

    public void Pause() {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        GameIsPaused = true;
    }
     
    public void Resume() {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameIsPaused = false;
    }
}
