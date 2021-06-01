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

    [Header("Weapons")]
    public GameObject WeaponParent;
    public enum WeaponType {
        Sword,
        Spear,
        Axe
    }
    public WeaponType weaponType;
    private Weapon weapon;

    [HideInInspector]
    public TurretSpawnig turretSpawnig;
    [HideInInspector]
    public SwordManager swordManager;
    //[HideInInspector]
    //public SpearManager spearManager;
    //[HideInInspector]
    //public AxeManager axeManager;

    private int _enemylayerMask = 1 << 11;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("PlayerManager is a singleton, can't be instantiated more than 1 times");
        } else
            instance = this;
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        turretSpawnig = gameObject.GetComponent<TurretSpawnig>();
        SetUpWeapon();
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

        //Attack
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5f, Color.red);

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (InGameMenuManager.instance.GameIsPaused)
                return;
            if (turretSpawnig.activatePreBuy)
                return;
            RaycastHit hit;
            switch (weaponType) {
                case WeaponType.Sword:
                weapon.PrepareAttack();
                SwordManager obj = WeaponParent.GetComponentInChildren<SwordManager>();
                //if (obj.canAttack && Physics.Raycast(transform.position, 
                //    transform.TransformDirection(Vector3.forward), out hit, obj.Range, _enemylayerMask)) {

                //    if (Input.GetKeyDown(KeyCode.Mouse0)) {
                //        obj.PrepareAttack(hit.transform.gameObject);
                //    }
                //} else {
                    
                //}
                break;
                case WeaponType.Spear:
                break;
                case WeaponType.Axe:
                break;
                default:
                break;
            }
        }
    }

    private bool IsGrounded() {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, 
            col.bounds.min.y, col.bounds.center.z), col.radius * .9f, groundLayers);
    }

    private void Attack(RaycastHit _hit, float _range) {

    }

    public void SetUpWeapon() {
        weapon = GetWeaponOnHand();
    }

    private Weapon GetWeaponOnHand() {
        switch (weaponType) {
            case WeaponType.Sword:
            return WeaponParent.GetComponentInChildren<Weapon>();
            break;
            case WeaponType.Spear:
            return null;
            break;
            case WeaponType.Axe:
            return null;
            break;
            default:
            return null;
            break;
        }
    }
}
