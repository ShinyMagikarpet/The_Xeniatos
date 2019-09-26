using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public enum PlayerState {
        Idle,
        Walking,
        Sprinting,
        Jumping,
        Shooting,
        Reloading,
        Dead
    };

    [HideInInspector]
    public bool mIsDead = false;
    public Weapon mPlayerWeapon;
    public Text ammoText;
    public PlayerState state;

    void Start()
    {
        mPlayerWeapon = GetComponentInChildren<Weapon>();
        mPlayerWeapon.mCam = GetComponentInChildren<Camera>();
        mPlayerWeapon.mOwner = this;
        state = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = mPlayerWeapon.mAmmoLoaded.ToString() + '/' +
            mPlayerWeapon.mAmmoHeld.ToString();

        if (Input.GetButtonDown("Fire1")) {
            mPlayerWeapon.Fire_Weapon();
        }

        if (Input.GetButtonDown("Reload")) {
            mPlayerWeapon.Reload_Weapon();
        }


    }
}
