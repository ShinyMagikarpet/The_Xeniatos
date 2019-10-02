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
    public int mMaxHealth = 100;
    public int mCurrentHealth = 100;
    public Weapon mPlayerWeapon;
    public Text ammoText;
    public Text healthText;
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

        healthText.text = mCurrentHealth.ToString() + '/' + mMaxHealth.ToString();

        if (Input.GetButtonDown("Fire1") && mPlayerWeapon.mFire_Type == Weapon.Fire_Type.single) {
            mPlayerWeapon.Fire_Weapon();
        }

        if(Input.GetButton("Fire1") && mPlayerWeapon.mFire_Type == Weapon.Fire_Type.fully_Auto) {
            Debug.Log("Fire fully auto");
            mPlayerWeapon.Fire_Weapon();
        }

        if (Input.GetButtonDown("Reload") || mPlayerWeapon.mAmmoLoaded == 0) {
            mPlayerWeapon.Reload_Weapon();
        }

        if (mPlayerWeapon.mOwner) {
            mPlayerWeapon.tag = "Player";
        }
        
    }
}
