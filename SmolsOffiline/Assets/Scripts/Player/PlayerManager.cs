﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public static PlayerManager instance;

    [Header("Rotation")]
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    [Header("Movement")]
    public Rigidbody RB;

    public float speed = 12f; //12 by default

    [Header("Jump")]
    public LayerMask groundLayers;
    public CapsuleCollider col;

    public float jumpForce = 7; //7 by default

    [HideInInspector]
    public TurretSpawnig turretSpawnig;
    [HideInInspector]
    public SwordAnimationManager swordAnimationManager;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("GameManager is a singleton, can't be instantiated more than 1 times");
        } else
            instance = this;
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        turretSpawnig = gameObject.GetComponent<TurretSpawnig>();
        swordAnimationManager = gameObject.GetComponentInChildren<SwordAnimationManager>();
    }

    void Update() {

        //Rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation / mouseSensitivity, 0);

        transform.parent.localRotation = Quaternion.Euler(0, yRotation, 0);

        //Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = Vector3.right * x + Vector3.forward * z;

        transform.parent.Translate(move * Time.deltaTime * speed);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) {
            RB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        } else if (!Input.GetKey(KeyCode.Space) && IsGrounded()) {
            RB.velocity = new Vector3(0,RB.velocity.y,0);
        }

        //Open-Close Store
        if (Input.GetKeyDown(KeyCode.Tab)) {
            CanvasManager.instance.StoreCanvasManager();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            swordAnimationManager.SwordAttack();
        }

    }

    private bool IsGrounded() {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, 
            col.bounds.min.y, col.bounds.center.z), col.radius * .9f, groundLayers);
    }
}
